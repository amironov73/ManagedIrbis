// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforE.cs --
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

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Вернуть заданное количество слов с начала строки – &uf('E…
    // Вид функции: E.
    // Назначение: Вернуть заданное количество слов с начала строки.
    // Формат (передаваемая строка):
    // EN<строка>
    // где N – количество слов (одна цифра).
    //
    // Примеры:
    // &unifor("E3"v200^a)
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class UniforE
    {
        #region Private members

        //
        // TODO Use ISISACW.TAB
        //

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

            MatchCollection matches = Regex.Matches
                (
                    text,
                    @"\w+"
                );
            if (wordCount >= matches.Count)
            {
                return string.Empty;
            }
            Match match = matches[wordCount];
            int end = match.Index + match.Length;
            string result = text.Substring(0, end);

            return result;
        }

        internal static string GetLastWords
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

            MatchCollection matches = Regex.Matches
                (
                    text,
                    @"\w+"
                );
            if (wordCount >= matches.Count)
            {
                return string.Empty;
            }
            Match match = matches[wordCount];
            int end = match.Index + match.Length;
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
                string countText = navigator.ReadInteger();
                if (!string.IsNullOrEmpty(countText))
                {
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
        }

        #endregion
    }
}
