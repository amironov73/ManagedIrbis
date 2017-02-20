// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReaderManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using AM;
using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Batch;

#endregion

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Основные операции с читателями.
    /// </summary>
    [PublicAPI]
    public sealed class ReaderManager
    {
        #region Constants

        /// <summary>
        /// Стандартный префикс идентификатора читателя.
        /// </summary>
        // TODO: брать индекс из настроек клиента
        public const string ReaderIdentifier = "RI=";

        #endregion

        #region Events

        /// <summary>
        /// Fired on batch read.
        /// </summary>
        public event EventHandler BatchRead;

        #endregion

        #region Properties

        /// <summary>
        /// Клиент, общающийся с сервером.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderManager"/> class.
        /// </summary>
        public ReaderManager
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
        }

        #endregion

        #region Private members

        private void HandleBatchRead
            (
                object sender,
                EventArgs eventArgs
            )
        {
            BatchRead.Raise(sender, eventArgs);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива всех (не удалённых) читателей из базы данных.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public ReaderInfo[] GetAllReaders
            (
                [NotNull] string database
            )
        {
            Code.NotNullNorEmpty(database, "database");

            List<ReaderInfo> result = new List<ReaderInfo> 
                (
                    Connection.GetMaxMfn() + 1
                );

            IEnumerable<MarcRecord> batch = BatchRecordReader.WholeDatabase
                (
                    Connection,
                    database,
                    500
                );

            BatchRecordReader batch2 = batch as BatchRecordReader;
            if (!ReferenceEquals(batch2, null))
            {
                batch2.BatchRead += HandleBatchRead;
            }

            foreach (MarcRecord record in batch)
            {
                if (!ReferenceEquals(record, null))
                {
                    ReaderInfo reader = ReaderInfo.Parse(record);
                    result.Add(reader);
                }
            }

            if (!ReferenceEquals(batch2, null))
            {
                batch2.BatchRead -= HandleBatchRead;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получение массива всех (не удалённых) читателей из базы данных.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public ReaderInfo[] GetReaders
            (
                [NotNull] string database,
                [NotNull] IEnumerable<int> mfns
            )
        {
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(mfns, "mfns");

            List<ReaderInfo> result = new List<ReaderInfo>();

            IEnumerable<MarcRecord> batch = new BatchRecordReader
                (
                    Connection,
                    database,
                    500,
                    mfns
                );

            BatchRecordReader batch2 = batch as BatchRecordReader;
            batch2.BatchRead += HandleBatchRead;

            foreach (MarcRecord record in batch)
            {
                if (!ReferenceEquals(record, null))
                {
                    ReaderInfo reader = ReaderInfo.Parse(record);
                    result.Add(reader);
                }
            }

            batch2.BatchRead -= HandleBatchRead;

            return result.ToArray();
        }

        /// <summary>
        /// Получение записи читателя по его идентификатору.
        /// </summary>
        [CanBeNull]
        public ReaderInfo GetReader
            (
                [NotNull] string ticket
            )
        {
            Code.NotNullNorEmpty(ticket, "ticket");

            MarcRecord record = Connection.SearchReadOneRecord
                (
                    "{0}{1}",
                    ReaderIdentifier,
                    ticket
                );
            if (ReferenceEquals(record, null))
            {
                return null;
            }

            ReaderInfo result = ReaderInfo.Parse(record);

            return result;
        }

        /// <summary>
        /// Merge readers (i. e. from some databases).
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public ReaderInfo[] MergeReaders
            (
                [NotNull] ReaderInfo[] readers
            )
        {
            var grouped = readers
                .Where(r => !string.IsNullOrEmpty(r.Ticket))
                .GroupBy(r => r.Ticket);

            List<ReaderInfo> result = new List<ReaderInfo>(readers.Length);

            foreach (var grp in grouped)
            {
                ReaderInfo first = grp.First();
                first.Visits = grp
                    .SelectMany(r => r.Visits)
                    .ToArray();
                result.Add(first);
            }

            return result.ToArray();
        }

        #endregion
    }
}
