// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EncodingUtility.cs -- text encoding related routines
 *  Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using UnsafeAM.IO;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

// ReSharper disable ConvertIfStatementToNullCoalescingExpression

namespace UnsafeAM.Text
{
    /// <summary>
    /// Text encoding related routines.
    /// </summary>
    [PublicAPI]
    public static class EncodingUtility
    {
        #region Nested classes

        private class KnownEncoding
        {
            public string Name;
            public Encoding Encoding;
            public byte[] Preamble;

            public KnownEncoding(string name, Encoding encoding)
            {
                Name = name;
                Encoding = encoding;
                Preamble = encoding.GetPreamble();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Default encoding.
        /// </summary>
        /// <remarks>
        /// Reduce if/else preprocessing.
        /// </remarks>
        [NotNull]
        public static Encoding DefaultEncoding
        {
            get
            {
                return Encoding.GetEncoding(0);
            }
        }

        private static int _maxPreambleLength;

        /// <summary>
        /// Maximum preamble length.
        /// </summary>
        public static int MaxPreambleLength
        {
            get
            {
                return _maxPreambleLength;
            }
        }

        private static Encoding _cp866;

        /// <summary>
        /// Gets the CP866 (cyrillic) <see cref="Encoding"/>.
        /// </summary>
        [NotNull]
        public static Encoding Cp866
        {
            [DebuggerStepThrough]
            get
            {
                if (ReferenceEquals(_cp866, null))
                {
                    RegisterRequiredProviders();
                    _cp866 = Encoding.GetEncoding(866);
                }

                return _cp866;
            }
        }

        private static Encoding _windows1251;

        /// <summary>
        /// Gets the Windows-1251 (cyrillic) <see cref="Encoding"/>.
        /// </summary>
        [NotNull]
        public static Encoding Windows1251
        {
            [DebuggerStepThrough]
            get
            {
                if (ReferenceEquals(_windows1251, null))
                {
                    _windows1251 = Encoding.GetEncoding(1251);
                }

                return _windows1251;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Class constructor.
        /// </summary>
        static EncodingUtility()
        {
            List<KnownEncoding> known = new List<KnownEncoding>();

            known.Add
                (
                    new KnownEncoding
                        (
                            "Big-endian UTF16",
                            new UnicodeEncoding
                                (
                                    true,
                                    true
                                )
                        )
                );

            known.Add
                (
                    new KnownEncoding
                    (
                        "Little-endian UTF16",
                        new UnicodeEncoding
                            (
                                false,
                                true
                            )
                    )
                );

            known.Add
                (
                    new KnownEncoding
                        (
                            "UTF8",
                            new UTF8Encoding
                                (
                                    true
                                )
                        )
                );

            //known.Add
            //    (
            //        new KnownEncoding
            //            (
            //                "UTF7",
            //                new UTF7Encoding
            //                    (
            //                        true
            //                    )
            //            )
            //    );

            known.Add
                (
                    new KnownEncoding
                        (
                            "Big-endian UTF32",
                            new UTF32Encoding
                                (
                                    true,
                                    true
                                )
                        )
                );

            known.Add
                (
                    new KnownEncoding
                        (
                            "Little-endian UTF32",
                            new UTF32Encoding
                            (
                                false,
                                true
                            )
                        )
                );

            _KnownEncodings = known.ToArray();
            foreach (KnownEncoding enc in _KnownEncodings)
            {
                if (enc.Preamble.Length > _maxPreambleLength)
                {
                    _maxPreambleLength = enc.Preamble.Length;
                }
            }
        }

        #endregion

        #region Private members

        private static readonly KnownEncoding[] _KnownEncodings;

        #endregion

        #region Public methods

        /// <summary>
        /// Change encoding of the text.
        /// </summary>
        [CanBeNull]
        public static string ChangeEncoding
            (
                [CanBeNull] string text,
                [NotNull] Encoding fromEncoding,
                [NotNull] Encoding toEncoding
            )
        {
            Code.NotNull(fromEncoding, nameof(fromEncoding));
            Code.NotNull(toEncoding, nameof(toEncoding));

            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            byte[] bytes = toEncoding.GetBytes(text);
            string result = fromEncoding.GetString(bytes);

            return result;
        }


        /// <summary>
        /// Determine text encoding.
        /// </summary>
        [CanBeNull]
        public static Encoding DetermineTextEncoding
            (
                [NotNull] byte[] textWithPreamble
            )
        {
            Code.NotNull(textWithPreamble, nameof(textWithPreamble));

            foreach (KnownEncoding known in _KnownEncodings)
            {
                if (textWithPreamble.Length
                     <= known.Preamble.Length)
                {
                    bool found = true;
                    for (int i = 0; i < known.Preamble.Length; i++)
                    {
                        if (textWithPreamble[i]
                             != known.Preamble[i])
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        return known.Encoding;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Determine text encoding.
        /// </summary>
        public static Encoding DetermineTextEncoding
            (
                [NotNull] Stream stream
            )
        {
            byte[] textWithPreamble = StreamUtility.ReadAsMuchAsPossible
                (
                    stream,
                    MaxPreambleLength
                );

            return DetermineTextEncoding(textWithPreamble);
        }

        /// <summary>
        /// Determines the text file encoding.
        /// </summary>
        public static Encoding DetermineTextEncoding
            (
                string fileName
            )
        {
            using (FileStream stream = File.OpenRead(fileName))
            {
                return DetermineTextEncoding(stream);
            }
        }

        /// <summary>
        /// Register required encoding providers.
        /// </summary>
        public static void RegisterRequiredProviders()
        {
#if NETCORE

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

#endif
        }

        #endregion
    }
}
