// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BatchSearcher.cs --
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

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Batch
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class BatchSearcher
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [NotNull]
        public string Database { get; private set; }

        /// <summary>
        /// Operation.
        /// </summary>
        [NotNull]
        public string Operation { get; set; }

        /// <summary>
        /// Prefix
        /// </summary>
        [CanBeNull]
        public string Prefix { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BatchSearcher
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string database,
                [CanBeNull] string prefix
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");

            Connection = connection;
            Database = database;
            Prefix = prefix;
            Operation = "+";
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Build search query from specified terms.
        /// </summary>
        [NotNull]
        public string BuildExpression
            (
                [NotNull] IEnumerable<string> terms
            )
        {
            Code.NotNull(terms, "terms");

            string result = SearchUtility.ConcatTerms
                (
                    Prefix,
                    Operation,
                    terms
                );

            return result;
        }

        /// <summary>
        /// Search for specified terms.
        /// </summary>
        [NotNull]
        public int[] Search
            (
                [NotNull] IEnumerable<string> terms
            )
        {
            Code.NotNull(terms, "terms");

            string expression = BuildExpression(terms);
            int[] result = Connection.Search(expression);

            return result;
        }

        /// <summary>
        /// Search for specified terms.
        /// </summary>
        [NotNull]
        public MarcRecord[] SearchRead
            (
                [NotNull] IEnumerable<string> terms
            )
        {
            Code.NotNull(terms, "terms");

            string expression = BuildExpression(terms);
            MarcRecord[] result
                = Connection.SearchRead(expression);

            return result;
        }

        #endregion
    }
}
