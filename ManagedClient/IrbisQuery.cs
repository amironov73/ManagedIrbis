/* IrbisQuery.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Diagnostics;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisQuery
    {
        #region Properties

        /// <summary>
        /// Запрещенные символы.
        /// </summary>
        public static char[] ForbiddenCharacters = { '\r', '\n', '\t' };

        #endregion

        #region Public methods

        /// <summary>
        /// Подготавливает строку запроса,
        /// заменяя запрещённые символы на пробелы.
        /// </summary>
        [CanBeNull]
        public static string PrepareQuery
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            return text
                .Replace('\r', ' ')
                .Replace('\n', ' ')
                .Replace('\t', ' ');
        }

        #endregion
    }
}
