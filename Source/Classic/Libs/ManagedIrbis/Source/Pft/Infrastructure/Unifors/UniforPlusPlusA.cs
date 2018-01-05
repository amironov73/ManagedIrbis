// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusPlusA.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text.RegularExpressions;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak неописанная функция unifor('++A')
    //

    static class UniforPlusPlusA
    {
        #region Public methods

        public static void GetPhrase
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            //
            // Ищет четыре подряд и более алфавитных символа,
            // первый из которых должен быть прописным,
            // остальные могут быть и строчными и прописными.
            // Возвращает остаток строки, начиная с первого
            // подходящего фрагмента
            //

            if (!string.IsNullOrEmpty(expression))
            {
                Regex regex = new Regex("[A-ZЁА-Я][a-zёа-я]{3}");
                Match match = regex.Match(expression);
                if (match.Success)
                {
                    string output = match.Index == 0
                        ? expression
                        : expression.Substring(match.Index);
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        #endregion
    }
}
