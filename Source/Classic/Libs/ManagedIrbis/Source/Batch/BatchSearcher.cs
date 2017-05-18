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

using AM;
using AM.Logging;

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
        #region Constants

        /// <summary>
        /// Default batch size.
        /// </summary>
        public const int DefaultBatchSize = 100;

        #endregion

        #region Properties

        /// <summary>
        /// Batch size.
        /// </summary>
        public int BatchSize { get; set; }

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

            BatchSize = DefaultBatchSize;
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

            int batchSize = BatchSize;
            if (batchSize < 1)
            {
                Log.Trace
                    (
                        "BatchSearcher::Search: "
                        + "batchSize="
                        + batchSize
                    );

                throw new ArsMagnaException("BatchSize");
            }

            string[][] packages = terms
                .Slice(batchSize)
                .ToArray();
            int totalSize = packages.Sum(p => p.Length);
            if (totalSize == 0)
            {
                return new int[0];
            }

            List<int> result = new List<int>(totalSize);
            foreach (string[] package in packages)
            {
                string expression = BuildExpression(package);
                int[] found = Connection.Search(expression);
                result.AddRange(found);
            }

            return result.ToArray();
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

            int batchSize = BatchSize;
            if (batchSize < 1)
            {
                Log.Trace
                    (
                        "BatchSearcher::SearchRead: "
                        + "batchSize="
                        + batchSize
                    );

                throw new ArsMagnaException("BatchSize");
            }

            string[][] packages = terms
                .Slice(batchSize)
                .ToArray();
            int totalSize = packages.Sum(p => p.Length);
            if (totalSize == 0)
            {
                return new MarcRecord[0];
            }

            List<MarcRecord> result
                = new List<MarcRecord>(totalSize);
            foreach (string[] package in packages)
            {
                string expression = BuildExpression(package);
                MarcRecord[] found
                    = Connection.SearchRead(expression);
                result.AddRange(found);
            }

            return result.ToArray();
        }

        #endregion
    }
}
