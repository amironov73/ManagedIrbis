/* BatchRecordReader.cs -- batch record reader
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Batch
{
    /// <summary>
    /// Batch reader for <see cref="MarcRecord"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class BatchRecordReader
        : IEnumerable<MarcRecord>
    {
        #region Events

        /// <summary>
        /// Raised on batch reading.
        /// </summary>
        public event EventHandler BatchRead;

        #endregion

        #region Properties

        /// <summary>
        /// Batch size.
        /// </summary>
        public int BatchSize { get; private set; }

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
        /// Total number of records read.
        /// </summary>
        public int RecordsRead { get; private set; }

        /// <summary>
        /// Number of records to read.
        /// </summary>
        public int TotalRecords { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BatchRecordReader
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string database,
                int batchSize,
                [NotNull] IEnumerable<int> range
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(range, "range");
            if (batchSize < 1)
            {
                throw new ArgumentOutOfRangeException("batchSize");
            }

            Connection = connection;
            Database = database;
            BatchSize = batchSize;
            //_syncRoot = new object();

            _packages = range.Slice(batchSize).ToArray();
            TotalRecords = _packages.Sum(p => p.Length);
        }

        #endregion

        #region Private members

        //private readonly object _syncRoot;
        private readonly int[][] _packages;

        #endregion

        #region Public methods

        /// <summary>
        /// Считывает все записи сразу.
        /// </summary>
        [NotNull]
        public List<MarcRecord> ReadAll()
        {
            List<MarcRecord> result
                = new List<MarcRecord>(TotalRecords);

            foreach (MarcRecord record in this)
            {
                result.Add(record);
            }

            return result;
        }

        #endregion

        #region IEnumerable members

        /// <summary>
        /// Get enumrator.
        /// </summary>
        public IEnumerator<MarcRecord> GetEnumerator()
        {
            foreach (int[] package in _packages)
            {
                MarcRecord[] records = null;
                //try
                //{
                    records = Connection.ReadRecords
                        (
                            Database,
                            package
                        );
                    RecordsRead += records.Length;
                    BatchRead.Raise(this);
                //}
                //catch (Exception ex)
                //{
                //    _OnException(ex);
                //}
                if (records != null)
                {
                    foreach (MarcRecord record in records)
                    {
                        yield return record;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

    }
}
