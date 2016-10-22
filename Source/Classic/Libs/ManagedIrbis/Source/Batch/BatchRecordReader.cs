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

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

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
#if !WINMOBILE && !PocketPC
        [CanBeNull]
#endif
        public event EventHandler BatchRead;

        /// <summary>
        /// Raised when exception occurs.
        /// </summary>
#if !WINMOBILE && !PocketPC
        [CanBeNull]
#endif
        public event EventHandler<ExceptionEventArgs<Exception>> Exception;

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

        private readonly int[][] _packages;

        private bool _HandleException
            (
                Exception exception
            )
        {
            EventHandler<ExceptionEventArgs<Exception>> handler
                = Exception;

            if (handler == null)
            {
                return false;
            }

            ExceptionEventArgs<Exception> arguments
                = new ExceptionEventArgs<Exception>(exception);
            handler(this, arguments);

            return arguments.Handled;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Read interval of records
        /// </summary>
        [NotNull]
        public static IEnumerable<MarcRecord> Interval
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string database,
                int firstMfn,
                int lastMfn,
                int batchSize
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");
            Code.Positive(firstMfn, "firstMfn");
            Code.Positive(lastMfn, "lastMfn");
            if (batchSize < 1)
            {
                throw new ArgumentOutOfRangeException("batchSize");
            }

            int maxMfn = connection.GetMaxMfn(database) - 1;
            if (maxMfn == 0)
            {
                return new MarcRecord[0];
            }

            lastMfn = Math.Min(lastMfn, maxMfn);
            if (firstMfn > lastMfn)
            {
                return new MarcRecord[0];
            }

            BatchRecordReader result = new BatchRecordReader
                (
                    connection,
                    database,
                    batchSize,
                    Enumerable.Range
                    (
                        firstMfn,
                        lastMfn - firstMfn + 1
                    )
                );

            return result;
        }

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

        /// <summary>
        /// Search and read records.
        /// </summary>
        [NotNull]
        public static IEnumerable<MarcRecord> Search
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string database,
                [NotNull] string searchExpression,
                int batchSize
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");
            Code.NotNullNorEmpty(searchExpression, "searchExpression");
            if (batchSize < 1)
            {
                throw new ArgumentOutOfRangeException("batchSize");
            }

            int[] found = connection.Search(searchExpression);
            if (found.Length == 0)
            {
                return new MarcRecord[0];
            }

            BatchRecordReader reader = new BatchRecordReader
                (
                    connection,
                    database,
                    batchSize,
                    found
                );

            return reader;
        }

        /// <summary>
        /// Read whole database
        /// </summary>
        [NotNull]
        public static IEnumerable<MarcRecord> WholeDatabase
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string database,
                int batchSize
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");
            if (batchSize < 1)
            {
                throw new ArgumentOutOfRangeException("batchSize");
            }

            int maxMfn = connection.GetMaxMfn(database) - 1;
            if (maxMfn == 0)
            {
                return new MarcRecord[0];
            }

            BatchRecordReader result = new BatchRecordReader
                (
                    connection,
                    database,
                    batchSize,
                    Enumerable.Range(1, maxMfn)
                );

            return result;
        }

        #endregion

        #region IEnumerable members

        /// <summary>
        /// Get enumerator.
        /// </summary>
        public IEnumerator<MarcRecord> GetEnumerator()
        {
            foreach (int[] package in _packages)
            {
                MarcRecord[] records = null;
                try
                {
                    records = Connection.ReadRecords
                        (
                            Database,
                            package
                        );
                    RecordsRead += records.Length;
                    BatchRead.Raise(this);
                }
                catch (Exception exception)
                {
                    if (!_HandleException(exception))
                    {
                        throw;
                    }
                }
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
