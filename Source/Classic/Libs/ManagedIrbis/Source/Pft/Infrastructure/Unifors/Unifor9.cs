// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Unifor9.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Удалить двойные кавычки из заданной строки – &uf('9…
    // Вид функции: 9.
    // Назначение: Удалить двойные кавычки из заданной строки.
    // Формат (передаваемая строка):
    // 9<исх.строка>
    // Примеры:
    // &unifor("9"v200^a)
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class Unifor9
    {
        #region Public methods

        /// <summary>
        /// Remove double quotes from the string.
        /// </summary>
        public static void RemoveDoubleQuotes
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            Code.NotNull(context, "context");

            if (!String.IsNullOrEmpty(expression))
            {
                string clear = expression.Replace("\"", String.Empty);
                context.Write(node, clear);
                context.OutputFlag = true;
            }
        }

        #endregion
    }
}
