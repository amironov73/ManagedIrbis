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

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Search;
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


        //private QAst _MakeAst
        //    (
        //        string[] array,
        //        IEnumerable<string> tags
        //    )
        //{
        //    if (array.Length == 1)
        //    {
        //        return new QAstEntry(tags)
        //        {
        //            Expression = Prefix + array[0],
        //            Quoted = true
        //        };
        //    }
        //    QAstPlusOperator result = new QAstPlusOperator
        //    {
        //        LeftOperand = new QAstEntry
        //        {
        //            Expression = Prefix + array[0],
        //            Quoted = true,
        //        },
        //        RightOperand = _MakeAst
        //            (
        //                array.Skip(1).ToArray(),
        //                tags
        //            )
        //    };
        //    result.Children.Add(result.LeftOperand);
        //    result.Children.Add(result.RightOperand);
        //    return result;
        //}

        //private bool _QueryWalker
        //    (
        //        QAst ast
        //    )
        //{
        //    for (int i = 0; i < ast.Children.Count; i++)
        //    {
        //        QAst child = ast.Children[i];
        //        QAstEntry entry = child as QAstEntry;
        //        if (entry != null)
        //        {
        //            if (entry.Expression.StartsWith(Prefix)
        //                && entry.Ending == EndingKind.NoTrim)
        //            {
        //                string word = string.IsNullOrEmpty(Prefix)
        //                    ? entry.Expression
        //                    : entry.Expression.Substring(Prefix.Length);
        //                MorphologyEntry[] entries = FindWord(word);
        //                string[] flatten = Flatten(word, entries);
        //                if (flatten.Length > 1)
        //                {
        //                    QAstParen paren = new QAstParen();
        //                    QAst newAst = _MakeAst(flatten, entry.Tags);
        //                    paren.Children.Add(newAst);
        //                    ast.Children[i] = paren;
        //                    return false;
        //                }
        //            }
        //        }
        //    }
        //    return true;
        //}

        #endregion

        #region MorphologyProvider members

        /// <inheritdoc cref="MorphologyProvider.FindWord"/>
        public override MorphologyEntry[] FindWord
            (
                string word
            )
        {
            Code.NotNullNorEmpty(word, "word");

            try
            {
                Connection.PushDatabase(Database);
                MarcRecord[] records = Connection.SearchRead
                    (
                        "\"K={0}\"",
                        word
                    );
                MorphologyEntry[] result = records
                    .Select(r => MorphologyEntry.Parse(r))
                    .ToArray();
                return result;
            }
            finally
            {
                Connection.PopDatabase();
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

            // Do something with terms

            return queryExpression;
        }

        #endregion
    }
}
