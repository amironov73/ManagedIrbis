// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforF.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text.RegularExpressions;

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Вернуть часть строки, начиная со следующего слова
    // после указанного и до конца строки – &uf('F')
    // Вид функции: F.
    // Назначение: Вернуть часть строки, начиная со следующего
    // слова после указанного и до конца строки.
    // Формат (передаваемая строка):
    // FN<строка>
    // где N – количество слов (одна цифра).
    //
    // Пример:
    //
    // &unifor("F3"v200^a)
    //

    static class UniforF
    {
        #region Private members

        static string GetLastWords
            (
                [CanBeNull] string text,
                int wordCount
            )
        {
            if (string.IsNullOrEmpty(text)
                || wordCount <= 0)
            {
                return string.Empty;
            }

            wordCount--;

            // TODO Use IrbisAlphabetTable?

            // ibatrak через ISISACW.TAB делать смысла нет
            // irbis64 ищет одиночные пробелы

            int[] positions = text.GetPositions(' ');

            if (wordCount >= positions.Length)
            {
                return text;
            }

            int end = positions[wordCount];
            string result = text.Substring
                (
                    end,
                    text.Length - end
                );

            return result;
        }


        #endregion

        #region Public methods

        /// <summary>
        /// Часть строки после N первых слов.
        /// </summary>
        public static void GetLastWords
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            Code.NotNull(context, "context");

            if (!string.IsNullOrEmpty(expression))
            {
                TextNavigator navigator = new TextNavigator(expression);
                string countText = navigator.ReadChar().ToString();
                int wordCount;
                if (NumericUtility.TryParseInt32(countText, out wordCount))
                {
                    string text = navigator.GetRemainingText();
                    string output = GetLastWords(text, wordCount);
                    context.WriteAndSetFlag(node, output);
                }
            }
        }

        #endregion
    }
}
