// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisSearchQuery.cs -- IRBIS search query
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// IRBIS search query.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisSearchQuery
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
