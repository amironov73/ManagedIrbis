/* UniforE.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text.RegularExpressions;

using AM.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
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

        public static void GetFirstWords
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                TextNavigator navigator = new TextNavigator(expression);
                string countText = navigator.ReadInteger();
                if (!string.IsNullOrEmpty(countText))
                {
                    int wordCount;
                    if (int.TryParse(countText, out wordCount))
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

        public static void GetLastWords
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                TextNavigator navigator = new TextNavigator(expression);
                string countText = navigator.ReadInteger();
                if (!string.IsNullOrEmpty(countText))
                {
                    int wordCount;
                    if (int.TryParse(countText, out wordCount))
                    {
                        string text = navigator.GetRemainingText();
                        string output = GetLastWords(text, wordCount);
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
