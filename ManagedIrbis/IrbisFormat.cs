/* IrbisFormat.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System.Text.RegularExpressions;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisFormat
    {
        #region Properties

        /// <summary>
        /// Запрещенные символы.
        /// </summary>
        public static char[] ForbiddenCharacters
            = {'\r', '\n', '\t', '\x1F', '\x1E'};

        #endregion

        #region Public methods

        /// <summary>
        /// Подготавливает строку запроса
        /// </summary>
        /// <param name="text"></param>
        /// <remarks>Строка формата не должна
        /// содержать комментариев и переводов
        /// строки (настоящих и ирбисных)
        /// </remarks>
        /// <returns></returns>
        [CanBeNull]
        public static string PrepareFormat
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            text = Regex.Replace
                (
                    text,
                    "/[*].*?[\r\n]",
                    " "
                )
                .Replace('\r', ' ')
                .Replace('\n', ' ')
                .Replace('\t', ' ')
                .Replace('\x1F', ' ')
                .Replace('\x1E', ' ');

            return text;
        }

        #endregion
    }
}
