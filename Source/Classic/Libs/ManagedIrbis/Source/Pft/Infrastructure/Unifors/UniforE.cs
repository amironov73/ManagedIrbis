// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforE.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Вернуть заданное количество слов с начала строки – &uf('E')
    // Вид функции: E.
    // Назначение: Вернуть заданное количество слов с начала строки.
    // Формат (передаваемая строка):
    // EN<строка>
    // где N – количество слов (одна цифра).
    //
    // Пример:
    //
    // &unifor("E3"v200^a)
    //

    static class UniforE
    {
        #region Private members

        internal static string GetFirstWords
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

            // ibatrak через ISISACW.TAB делать смысла нет
            // irbis64 ищет одиночные пробелы

            int[] positions = text.GetPositions(' ');

            if (wordCount >= positions.Length)
            {
                return text;
            }

            int end = positions[wordCount];
            string result = text.Substring(0, end);

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Первые N слов в строке.
        /// </summary>
        public static void GetFirstWords
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
                if (countText == "0")
                {
                    countText = "10";
                }
                int wordCount;
                if (NumericUtility.TryParseInt32(countText, out wordCount))
                {
                    string text = navigator.GetRemainingText();
                    string output = GetFirstWords(text, wordCount);
                    if (!string.IsNullOrEmpty(output))
                    {
                        context.Write(node, output);
                        context.OutputFlag = true;
                    }
                }
            }
        }

        #endregion
    }
}
