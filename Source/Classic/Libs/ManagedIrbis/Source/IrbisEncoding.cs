﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisEncoding.cs -- encodings used by IRBIS
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * State: moderate
 */

#region Using directives

using System;
using System.Text;

using AM;
using AM.Logging;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#if CLASSIC || NETCORE

using CM=System.Configuration.ConfigurationManager;

#endif

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Encoding used by IRBIS
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class IrbisEncoding
    {
        #region Properties

        /// <summary>
        /// Default single-byte encoding.
        /// </summary>
        [NotNull]
        public static Encoding Ansi { get { return _ansi; } }

        /// <summary>
        /// OEM encoding.
        /// </summary>
        [NotNull]
        public static Encoding Oem { get { return _oem; } }

        /// <summary>
        /// UTF8 encoding.
        /// </summary>
        [NotNull]
        public static Encoding Utf8 { get { return _utf8; } }

        #endregion

        #region Construction

        static IrbisEncoding()
        {
            EncodingUtility.RegisterRequiredProviders();
            _ansi = EncodingUtility.Windows1251;
            _oem = EncodingUtility.Cp866;
            _utf8 = new UTF8Encoding
                (
                    false, // don't emit UTF-8 prefix
                    true   // throw on invalid bytes
                );
        }

        #endregion

        #region Private members

        private static Encoding _ansi;

        private static Encoding _oem;

        private static Encoding _utf8;

        #endregion

        #region Public methods

        /// <summary>
        /// Get encoding by name.
        /// </summary>
        [NotNull]
        public static Encoding ByName
            (
                [CanBeNull] string name
            )
        {
            if (string.IsNullOrEmpty(name))
            {
                return Utf8;
            }

            if (name.SameString("Ansi"))
            {
                return Ansi;
            }

            if (name.SameString("Dos")
                || name.SameString("MsDos")
                || name.SameString("Oem"))
            {
                return Oem;
            }

            if (name.SameString("Utf")
                || name.SameString("Utf8")
                || name.SameString("Utf-8"))
            {
                return Utf8;
            }

            Encoding result = Encoding.GetEncoding(name);

            return result;
        }

#if CLASSIC || NETCORE

        /// <summary>
        /// Get encoding from config file.
        /// </summary>
        [NotNull]
        public static Encoding FromConfig
            (
                [NotNull] string key
            )
        {
            Code.NotNullNorEmpty(key, "key");

            string name = CM.AppSettings[key];
            Encoding result = ByName(name);

            return result;
        }

#endif

        /// <summary>
        /// Relax UTF-8 decoder, do not throw exceptions
        /// on invalid bytes.
        /// </summary>
        public static void RelaxUtf8()
        {
            _utf8 = new UTF8Encoding
                (
                    false, // don't emit UTF-8 prefix,
                    false  // don't throw on invalid bytes
                );
        }

        /// <summary>
        /// Strong UTF-8 decoder, throw exceptions
        /// on invalid bytes.
        /// </summary>
        public static void StrongUtf8()
        {
            _utf8 = new UTF8Encoding
                (
                    false, // don't emit UTF-8 prefix,
                    true   // throw on invalid bytes
                );
        }

        /// <summary>
        /// Override default single-byte encoding.
        /// </summary>
        public static void SetAnsiEncoding
            (
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(encoding, "encoding");

#if !WINMOBILE && !PocketPC

            if (!encoding.IsSingleByte)
            {
                Log.Error
                    (
                        "IrbisEncoding::SetAnsiEncoding: "
                        + "not single-byte encoding"
                    );

                throw new ArgumentOutOfRangeException("encoding");
            }

#endif

            _ansi = encoding;
        }

        /// <summary>
        /// Override OEM encoding.
        /// </summary>
        public static void SetOemEncoding
            (
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(encoding, "encoding");

#if !WINMOBILE && !PocketPC

            if (!encoding.IsSingleByte)
            {
                Log.Error
                    (
                        "IrbisEncoding::SetOemEncoding: "
                        + "not single-byte encoding"
                    );

                throw new ArgumentOutOfRangeException("encoding");
            }

#endif

            _oem = encoding;
        }

        #endregion
    }
}
