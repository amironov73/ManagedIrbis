// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisMorphologyProvider.cs
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

using ManagedIrbis.Client;
using ManagedIrbis.Search.Infrastructure;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable ConvertClosureToMethodGroup

namespace ManagedIrbis.Morphology
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisMorphologyProvider
        : MorphologyProvider
    {
        #region Constants

        /// <summary>
        /// Default database name.
        /// </summary>
        public const string DefaultDatabase = "MORPH";

        /// <summary>
        /// Default prefix.
        /// </summary>
        public const string DefaultPrefix = "K=";

        #endregion

        #region Properties

        /// <summary>
        /// Client connection.
        /// </summary>
        [CanBeNull]
        public IrbisProvider Provider { get; set; }

        /// <summary>
        /// Search prefix.
        /// </summary>
        [CanBeNull]
        public string Prefix { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisMorphologyProvider()
        {
            Prefix = DefaultPrefix;
            Database = DefaultDatabase;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisMorphologyProvider
            (
                [NotNull] IrbisProvider provider
            )
            : this
                (
                    DefaultPrefix,
                    DefaultDatabase,
                    provider
                )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisMorphologyProvider
            (
                [NotNull] string prefix,
                [NotNull] string database,
                [NotNull] IrbisProvider provider
            )
        {
            Code.NotNullNorEmpty(prefix, "prefix");
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(provider, "provider");

            Prefix = prefix;
            Database = database;
            Provider = provider;
        }

        #endregion

        #region MorphologyProvider members

        /// <inheritdoc cref="MorphologyProvider.FindWord" />
        public override MorphologyEntry[] FindWord
            (
                string word
            )
        {
            Code.NotNullNorEmpty(word, "word");

            IrbisProvider connection = Provider.ThrowIfNull("Connection");
            string database = Database.ThrowIfNull("Database");

            string saveDatabase = connection.Database;
            try
            {
                connection.Database = database;
                string expression = string.Format
                    (
                        "\"{0}{1}\"",
                        Prefix,
                        word
                    );

                int[] found = connection.Search(expression);
                if (found.Length == 0)
                {
                    return EmptyArray<MorphologyEntry>.Value;
                }

                List<MarcRecord> records = new List<MarcRecord>(found.Length);
                foreach (int mfn in found)
                {
                    MarcRecord record = connection.ReadRecord(mfn);
                    if (!ReferenceEquals(record, null))
                    {
                        records.Add(record);
                    }
                }

                if (records.Count == 0)
                {
                    return EmptyArray<MorphologyEntry>.Value;
                }

                MorphologyEntry[] result = records
                    .Select(r => MorphologyEntry.Parse(r))
                    .ToArray();

                return result;
            }
            finally
            {
                connection.Database = saveDatabase;
            }
        }

        /// <inheritdoc cref="MorphologyProvider.RewriteQuery" />
        public override string RewriteQuery
            (
                string queryExpression
            )
        {
            Code.NotNullNorEmpty(queryExpression, "queryExpression");

            SearchTokenList tokens
                = SearchQueryLexer.Tokenize(queryExpression);
            SearchQueryParser parser = new SearchQueryParser(tokens);
            SearchProgram program = parser.Parse();
            SearchTerm[] terms = SearchQueryUtility.ExtractTerms(program);

            string prefix = Prefix.ThrowIfNull("Prefix");
            int prefixLength = prefix.Length;

            foreach (SearchTerm oldTerm in terms)
            {
                string word = oldTerm.Term;
                if (string.IsNullOrEmpty(word))
                {
                    continue;
                }
                if (oldTerm.Tail == "$"
                    || !word.StartsWith(prefix))
                {
                    continue;
                }
                word = word.Substring(prefixLength);
                if (string.IsNullOrEmpty(word))
                {
                    continue;
                }
                MorphologyEntry[] entries = FindWord(word);
                string[] flatten = Flatten(word, entries);
                if (flatten.Length < 2)
                {
                    continue;
                }

                SearchLevel7 level7 = new SearchLevel7();
                SearchLevel6 level6 = new SearchLevel6();
                level7.AddItem(level6);
                foreach (string s in flatten)
                {
                    SearchLevel5 level5 = new SearchLevel5();
                    SearchLevel4 level4 = new SearchLevel4();
                    SearchLevel3 level3 = new SearchLevel3();
                    SearchLevel2 level2 = new SearchLevel2();
                    SearchLevel1 level1 = new SearchLevel1();
                    SearchLevel0 level0 = new SearchLevel0();
                    level1.AddItem(level0);
                    level2.AddItem(level1);
                    level3.AddItem(level2);
                    level4.AddItem(level3);
                    level5.AddItem(level4);
                    level6.AddItem(level5);

                    SearchTerm newTerm = new SearchTerm
                    {

                        Term = Prefix + s,
                        Tail = string.Empty,
                        Context = oldTerm.Context
                    };
                    level0.Term = newTerm;
                }

                SearchLevel0 parent = (SearchLevel0) oldTerm.Parent
                    .ThrowIfNull("oldTerm.Parent");
                parent.Term = null;
                parent.Parenthesis = level7;
            }

            string result = program.ToString();

            return result;
        }

        #endregion
    }
}
