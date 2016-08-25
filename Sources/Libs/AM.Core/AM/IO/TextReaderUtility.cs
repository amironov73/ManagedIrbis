/* TextReaderUtility.cs -- helpers for TextReader
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;

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
