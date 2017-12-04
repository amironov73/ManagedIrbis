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

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.IO
{
    /// <summary>
    /// Helpers for <see cref="TextReader"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
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
#if WIN81 || PORTABLE

            throw new System.NotImplementedException();

#else

            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(encoding, "encoding");

            StreamReader result = new StreamReader
                (
                    File.OpenRead(fileName),
                    encoding
                );

            return result;

#endif
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
            Code.NotNull(reader, "reader");

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
