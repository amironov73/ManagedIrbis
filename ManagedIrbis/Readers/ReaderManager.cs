/* ReaderManager.cs
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

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

        #region Properties

        /// <summary>
        /// Клиент, общающийся с сервером.
        /// </summary>
        [NotNull]
        public IrbisConnection Client
        {
            get { return _client; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderManager"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <exception cref="System.ArgumentNullException">client</exception>
        public ReaderManager
            (
                [NotNull] IrbisConnection client
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }

            _client = client;
        }

        #endregion

        #region Private members

        private readonly IrbisConnection _client;

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива всех (не удалённых) читателей из базы данных.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        [ItemNotNull]
        public ReaderInfo[] GetAllReaders()
        {
            throw new NotImplementedException();
#if NOTDEF
            List<ReaderInfo> result = new List<ReaderInfo> 
                (
                    Client.GetMaxMfn() + 1
                );
            BatchRecordReader batch = new BatchRecordReader(Client);
            foreach (IrbisRecord record in batch)
            {
                if (!ReferenceEquals(record, null))
                {
                    ReaderInfo reader = ReaderInfo.Parse(record);
                    result.Add(reader);
                }
            }

            return result.ToArray();
#endif
        }

        /// <summary>
        /// Получение записи читателя по его идентификатору.
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [CanBeNull]
        public ReaderInfo GetReader
            (
                [NotNull] string ticket
            )
        {
            if (string.IsNullOrEmpty(ticket))
            {
                throw new ArgumentNullException("ticket");
            }

            MarcRecord record = Client.SearchReadOneRecord
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

        #endregion
    }
}
