// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* KeywordPermutator.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Search
{
    /// <summary>
    /// Permutates keywords in search query.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class KeywordPermutator
    {
        #region Properties

        /// <summary>
        /// Alphabet table.
        /// </summary>
        [NotNull]
        public IrbisAlphabetTable AlphabetTable
        {
            get;
            private set;
        }

        /// <summary>
        /// Stopword list.
        /// </summary>
        [NotNull]
        public IrbisStopWords StopWords { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public KeywordPermutator
            (
                [NotNull] IrbisAlphabetTable alphabetTable,
                [NotNull] IrbisStopWords stopWords
            )
        {
            Code.NotNull(alphabetTable, "alphabetTable");
            Code.NotNull(stopWords, "stopWords");

            AlphabetTable = alphabetTable;
            StopWords = stopWords;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Permutate words from text
        /// </summary>
        [NotNull]
        public string[] PermutateWords
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            List<string> result = new List<string>()
            {
                text
            };

            IEqualityComparer<string> comparer =
                StringUtility.GetCaseInsensitiveComparer();

            string[] words = AlphabetTable
                .SplitWords(text)
                .Distinct(comparer)
                .Where(word => !StopWords.IsStopWord(word))
                .ToArray();

            foreach (string word in words)
            {
                if (!result.Contains(word, comparer))
                {
                    result.Add(word);
                }
            }

            return result.ToArray();
        }

        #endregion
    }
}
