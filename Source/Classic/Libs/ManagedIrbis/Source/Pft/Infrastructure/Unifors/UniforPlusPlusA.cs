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
                // Первый символ - прописная кириллическая или латинская буква,
                // три последующих - любые кириллические или латинские буквы
                // Кириллическими прописными считаются русские А-Я + ЂЃЉЊЌЋЏЎЈҐЁЄЇІ
                // кириллическими строчными а-я + ђѓљњќћџўјґёєїі
                // см. официальную таблицу кодовой страницы 1251
                // в https://ru.wikipedia.org/wiki/Windows-1251
                Regex regex = new Regex
                    (
                        @"[A-Z\u0410-\u042F\u0402\u0403\u0409\u040A\u040C"
                      + @"\u040B\u040F\u040E\u0408\u0490\u0401\u0404\u0407"
                      + @"\u0406][A-Za-z\u0410-\u044F\u0402\u0403\u0409"
                      + @"\u040A\u040C\u040B\u040F\u040E\u0408\u0490\u0401"
                      + @"\u0404\u0407\u0406\u0452\u0453\u0459\u045A\u045C"
                      + @"\u045B\u045F\u045E\u0458\u0491\u0451\u0454\u0457"
                      + @"\u0456]{3}"
                    );
                Match match = regex.Match(expression);
                if (match.Success)
                {
                    // If match.Index == 0 FW just returns the untouched string
                    string output = expression.Substring(match.Index);
                    context.Write(node, output);
                    context.OutputFlag = true;
                }
            }
        }

        #endregion
    }
}
