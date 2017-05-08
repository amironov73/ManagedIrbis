// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisMorphologyProvider.cs
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Linq;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Search.Infrastructure;

using MoonSharp.Interpreter;

#endregion

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
        #region Properties

        /// <summary>
        /// Client connection.
        /// </summary>
        [CanBeNull]
        public IrbisConnection Connection { get; set; }

        /// <summary>
        /// Search prefix.
        /// </summary>
        [CanBeNull]
        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string Database
        {
            get { return _database; }
            set { _database = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisMorphologyProvider()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisMorphologyProvider
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisMorphologyProvider
            (
                [NotNull] string prefix, 
                [NotNull] string database
            )
        {
            Code.NotNullNorEmpty(prefix, "prefix");
            Code.NotNullNorEmpty(database, "database");

            _prefix = prefix;
            _database = database;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisMorphologyProvider
            (
                [NotNull] string prefix, 
                [NotNull] string database, 
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNullNorEmpty(prefix, "prefix");
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(connection, "connection");

            _prefix = prefix;
            _database = database;
            Connection = connection;
        }

        #endregion

        #region Private members

        private string _prefix = "K=";

        private string _database = "MORPH";

        #endregion

        #region MorphologyProvider members

        /// <inheritdoc cref="MorphologyProvider.FindWord"/>
        public override MorphologyEntry[] FindWord
            (
                string word
            )
        {
            Code.NotNullNorEmpty(word, "word");

            IrbisConnection connection = Connection.ThrowIfNull("Connection");
            string database = Database.ThrowIfNull("Database");

            try
            {
                connection.PushDatabase(database);
                MarcRecord[] records = connection.SearchRead
                    (
                        "\"K={0}\"",
                        word
                    );
                MorphologyEntry[] result = records
                    // ReSharper disable ConvertClosureToMethodGroup
                    .Select(r => MorphologyEntry.Parse(r))
                    // ReSharper restore ConvertClosureToMethodGroup
                    .ToArray();
                return result;
            }
            finally
            {
                connection.PopDatabase();
            }
        }

        /// <inheritdoc cref="MorphologyProvider.RewriteQuery"/>
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
                foreach (string s in flatten)
                {
                    SearchLevel6 level6 = new SearchLevel6();
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
                    level7.AddItem(level6);

                    SearchTerm newTerm = new SearchTerm
                    {
                        Term = s,
                        Tail = string.Empty,
                        Context = oldTerm.Context
                    };
                    level0.Term = newTerm;

                    ISearchTree parent = oldTerm.Parent
                        .ThrowIfNull("oldTerm.Parent");
                    parent.ReplaceChild(oldTerm, newTerm);
                }
            }

            string result = program.ToString();

            return result;
        }

        #endregion
    }
}
