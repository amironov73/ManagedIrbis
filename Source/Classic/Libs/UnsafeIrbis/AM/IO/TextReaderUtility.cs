// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextReaderUtility.cs -- helpers for TextReader
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Text;

using UnsafeAM.Logging;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.IO
{
    /// <summary>
    /// Helpers for <see cref="TextReader"/>.
    /// </summary>
    [PublicAPI]
    public static class TextReaderUtility
    {
        #region Public methods

        /// <summary>
        /// Open file for reading.
        /// </summary>
        [NotNull]
        public static StreamReader OpenRead
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNullNorEmpty(fileName, nameof(fileName));
            Code.NotNull(encoding, nameof(encoding));

            StreamReader result = new StreamReader
                (
                    File.OpenRead(fileName),
                    encoding
                );

            return result;
        }

        /// <summary>
        /// Обязательное чтение строки.
        /// </summary>
        [NotNull]
        public static string RequireLine
            (
                [NotNull] this TextReader reader
            )
        {
            Code.NotNull(reader, nameof(reader));

            string result = reader.ReadLine();
            if (ReferenceEquals(result, null))
            {
                Log.Error
                    (
                        "TextReaderUtility::RequireLine: "
                        + "unexpected end of stream"
                    );

                throw new ArsMagnaException
                    (
                        "Unexpected end of stream"
                    );
            }

            return result;
        }

        #endregion
    }
}
