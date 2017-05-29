// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Search
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class SearchUtility
    {
        #region Constants

        /// <summary>
        /// Maximal term length (bytes, not characters!).
        /// </summary>
        public const int MaxTermLength = 255;

        #endregion

        #region Properties

        /// <summary>
        /// Special symbols.
        /// </summary>
        public static char[] SpecialSymbols =
        {
            ' ', '(', ')', '+', '*', '.', '"'
        };

        #endregion

        #region Public methods

        /// <summary>
        /// Concatenate some terms together.
        /// </summary>
        [NotNull]
        public static string ConcatTerms
            (
                [CanBeNull] string prefix,
                [CanBeNull] string operation,
                [NotNull] IEnumerable<string> terms
            )
        {
            Code.NotNull(terms, "terms");

            bool first = true;
            StringBuilder result = new StringBuilder();

            foreach (string term in terms)
            {
                if (!string.IsNullOrEmpty(term))
                {
                    if (!first)
                    {
                        result.Append(operation);
                    }

                    string wrapped = WrapTerm
                    (
                        TrimTerm(prefix + term)
                    );
                    result.Append(wrapped);

                    first = false;
                }
            }

            if (first)
            {
                Log.Error
                    (
                        "SearchUtility::ConcatTerms: "
                        + "empty list of terms"
                    );

                throw new IrbisException("Empty list of terms");
            }

            return result.ToString();
        }

        /// <summary>
        /// Escape quotation mark for Web-IRBIS.
        /// </summary>
        [CanBeNull]
        public static string EscapeQuotation
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (!text.Contains("\""))
            {
                return text;
            }

            string result = text.Replace
                (
                    "\"",
                    "<.>"
                );

            return result;
        }

        /// <summary>
        /// Trim the term (if exceeds <see cref="MaxTermLength"/>
        /// bytes).
        /// </summary>
        [NotNull]
        public static string TrimTerm
            (
                [NotNull] string term
            )
        {
            Code.NotNull(term, "term");

            int originalLength = term.Length;

            // Simple optimization
            if (originalLength < MaxTermLength / 2)
            {
                // Garanteed to fit into
                return term;
            }

            Encoding encoding = IrbisEncoding.Utf8;
            char[] charArray = term.ToCharArray();
            int currentLength = originalLength;

            while (currentLength > 0)
            {
                int count = encoding.GetByteCount
                    (
                        charArray,
                        0,
                        currentLength
                    );
                if (count <= MaxTermLength)
                {
                    break;
                }
                currentLength--;
            }

            string result = currentLength == originalLength
                ? term
                : term.Substring(0, currentLength);

            return result;
        }

        /// <summary>
        /// Unescape quotation mark for Web-IRBIS.
        /// </summary>
        [CanBeNull]
        public static string UnescapeQuotation
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (!text.Contains("<.>"))
            {
                return text;
            }

            string result = text.Replace
            (
                "<.>",
                "\""
            );

            return result;
        }

        /// <summary>
        /// Wrap the term if needed.
        /// </summary>
        [NotNull]
        public static string WrapTerm
            (
                [NotNull] string term
            )
        {
            Code.NotNull(term, "term");

            string result = term.ContainsAnySymbol(SpecialSymbols)
                ? "\"" + term + "\""
                : term;

            return result;
        }

        #endregion
    }
}
