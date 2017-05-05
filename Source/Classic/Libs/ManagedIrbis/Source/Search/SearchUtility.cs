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
                if (!first)
                {
                    result.Append(operation);
                }

                string wrapped = WrapTerm(prefix + term);
                result.Append(wrapped);

                first = false;
            }

            if (first)
            {
                throw new IrbisException("Empty list of terms");
            }

            return result.ToString();
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
