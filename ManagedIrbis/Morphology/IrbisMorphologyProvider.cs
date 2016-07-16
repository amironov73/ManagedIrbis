/* IrbisMorphologyProvider.cs
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

#endregion

namespace ManagedIrbis.Morphology
{
#if NOTDEF

    using Query;

    public sealed class IrbisMorphologyProvider
        : MorphologyProvider
    {
        #region Properties

        public ManagedClient64 Client
        {
            get { return _client; }
            set { _client = value; }
        }

        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }

        public string Database
        {
            get { return _database; }
            set { _database = value; }
        }

        #endregion

        #region Construction

        public IrbisMorphologyProvider()
        {
        }

        public IrbisMorphologyProvider
            (
                ManagedClient64 client
            )
        {
            _client = client;
        }

        public IrbisMorphologyProvider
            (
                string prefix, 
                string database
            )
        {
            _prefix = prefix;
            _database = database;
        }

        public IrbisMorphologyProvider
            (
                string prefix, 
                string database, 
                ManagedClient64 client
            )
        {
            _prefix = prefix;
            _database = database;
            _client = client;
        }

        #endregion

        #region Private members

        private string _prefix = "K=";

        private string _database = "MORPH";

        private ManagedClient64 _client;

        private QAst _MakeAst
            (
                string[] array,
                IEnumerable<string> tags
            )
        {
            if (array.Length == 1)
            {
                return new QAstEntry(tags)
                {
                    Expression = Prefix + array[0],
                    Quoted = true
                };
            }
            QAstPlusOperator result = new QAstPlusOperator
            {
                LeftOperand = new QAstEntry
                {
                    Expression = Prefix + array[0],
                    Quoted = true,
                },
                RightOperand = _MakeAst
                    (
                        array.Skip(1).ToArray(),
                        tags
                    )
            };
            result.Children.Add(result.LeftOperand);
            result.Children.Add(result.RightOperand);
            return result;
        }

        private bool _QueryWalker
            (
                QAst ast
            )
        {
            for (int i = 0; i < ast.Children.Count; i++)
            {
                QAst child = ast.Children[i];
                QAstEntry entry = child as QAstEntry;
                if (entry != null)
                {
                    if (entry.Expression.StartsWith(Prefix)
                        && entry.Ending == EndingKind.NoTrim)
                    {
                        string word = string.IsNullOrEmpty(Prefix)
                            ? entry.Expression
                            : entry.Expression.Substring(Prefix.Length);
                        MorphologyEntry[] entries = FindWord(word);
                        string[] flatten = Flatten(word, entries);
                        if (flatten.Length > 1)
                        {
                            QAstParen paren = new QAstParen();
                            QAst newAst = _MakeAst(flatten, entry.Tags);
                            paren.Children.Add(newAst);
                            ast.Children[i] = paren;
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        #endregion

        #region MorphologyProvider members

        public override MorphologyEntry[] FindWord
            (
                string word
            )
        {
            try
            {
                _client.PushDatabase(Database);
                IrbisRecord[] records = _client.SearchRead
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
                _client.PopDatabase();
            }
        }

        public override string RewriteQuery
            (
                string queryExpression
            )
        {
            QueryManager manager = new QueryManager();
            QAst ast = manager.ParseQuery(queryExpression);
            ast.Walk(_QueryWalker);
            string result = manager.SerializeAst(ast);
            return result;
        }

        #endregion
    }

#endif
}
