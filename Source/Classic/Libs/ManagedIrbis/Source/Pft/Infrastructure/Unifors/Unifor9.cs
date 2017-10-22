// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Unifor9.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Удалить двойные кавычки из заданной строки – &uf('9
    // Вид функции: 9.
    // Назначение: Удалить двойные кавычки из заданной строки.
    // Формат (передаваемая строка):
    // 9<исх.строка>
    //
    // Пример:
    //
    // &unifor("9"v200^a)
    //

    static class Unifor9
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

            if (!string.IsNullOrEmpty(expression))
            {
                string clear = expression.Replace("\"", string.Empty);
                context.Write(node, clear);
                context.OutputFlag = true;
            }
        }

        #endregion
    }
}
