// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BatchRecordWriter.cs -- batch record writer
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Batch
{
    /// <summary>
    /// Batch writer for <see cref="MarcRecord"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class BatchRecordWriter
        : IDisposable
    {
        #region Events

        /// <summary>
        /// Raised on batch write.
        /// </summary>
        public event EventHandler BatchWrite;

        #endregion

        #region Properties

        /// <summary>
        /// Actualize records when writing.
        /// </summary>
        public bool Actualize { get; set; }

        /// <summary>
        /// Capacity.
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IIrbisConnection Connection { get; private set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [NotNull]
        public string Database { get; private set; }

        /// <summary>
        /// Total number of records written.
        /// </summary>
        public int RecordsWritten { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BatchRecordWriter
            (
                [NotNull] IIrbisConnection connection,
                [NotNull] string database,
                int capacity
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");
            if (capacity < 1)
            {
                Log.Error
                    (
                        "BatchRecordWriter::Constructor: "
                        + "capacity="
                        + capacity
                    );

                throw new ArgumentOutOfRangeException("capacity");
            }

            Connection = connection;
            Database = database;
            Capacity = capacity;
            Actualize = true;
            _buffer = new List<MarcRecord>(capacity);
            _syncRoot = new object();
        }

        #endregion

        #region Private members

        private readonly List<MarcRecord> _buffer;

        private readonly object _syncRoot;

        #endregion

        #region Public methods

        /// <summary>
        /// Add many records.
        /// </summary>
        [NotNull]
        public BatchRecordWriter AddRange
            (
                [NotNull] IEnumerable<MarcRecord> records
            )
        {
            Code.NotNull(records, "records");

            lock (_syncRoot)
            {
                foreach (MarcRecord record in records)
                {
                    Append(record);
                }
            }

            return this;
        }

        /// <summary>
        /// Append one record.
        /// </summary>
        [NotNull]
        public BatchRecordWriter Append
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            lock(_syncRoot)
            {
                _buffer.Add(record);
                if (_buffer.Count >= Capacity)
                {
                    Flush();
                }
            }

            return this;
        }

        /// <summary>
        /// Flush the buffer.
        /// </summary>
        [NotNull]
        public BatchRecordWriter Flush()
        {
            lock(_syncRoot)
            {
                if (_buffer.Count != 0)
                {
                    try
                    {
                        Connection.PushDatabase(Database);
                        Connection.WriteRecords
                            (
                                _buffer.ToArray(),
                                false,
                                Actualize
                            );

                        RecordsWritten += _buffer.Count;
                    }
                    finally
                    {
                        Connection.PopDatabase();
                    }

                    BatchWrite.Raise(this);
                }

                _buffer.Clear();
            }

            return this;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Flush();
        }

        #endregion
    }
}
