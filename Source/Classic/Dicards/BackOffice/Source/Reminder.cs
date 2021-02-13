// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* Reminder.cs -- напоминает читателям, какие книги у них на руках
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;
using AM.Logging;

using DicardsConfig;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Readers;

using RestfulIrbis.OsmiCards;

#endregion

namespace BackOffice
{
    /// <summary>
    /// Напоминает читателям, какие книги у них на руках.
    /// </summary>
    static class Reminder
    {
        #region Properties

        /// <summary>
        /// Конфигурация.
        /// </summary>
        public static DicardsConfiguration Configuration { get; set; }

        /// <summary>
        /// Строка подключения к ИРБИС64.
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// API ID.
        /// </summary>
        public static string ApiId { get; set; }

        /// <summary>
        /// API key.
        /// </summary>
        public static string ApiKey { get; set; }

        /// <summary>
        /// API URL.
        /// </summary>
        public static string ApiUrl { get; set; }

        /// <summary>
        /// Метка поля в базе данных читателей,
        /// в котором хранится идентификатор читателя.
        /// По умолчанию это поле 30.
        /// </summary>
        public static int IdTag { get; set; }

        #endregion

        #region Private members

        private static void ProcessReader
            (
                ReaderInfo reader,
                IrbisConnection connection,
                OsmiCardsClient client
            )
        {
            CardUpdater.UpdateReaderCard(reader, Configuration, connection, client);
        }

        private static void ProcessReaders
            (
                string[] cards,
                OsmiCardsClient client
            )
        {
            using (var connection = CreateConnection())
            {
                // Получаем всех читателей из базы RDR
                IEnumerable<MarcRecord> batch = BatchRecordReader.Search
                    (
                        connection,
                        "RDR",
                        "RB=$",
                        1000
                    );

                foreach (var record in batch)
                {
                    var reader = ReaderInfo.Parse(record);
                    var ticket = OsmiUtility.GetReaderId(record, Configuration);
                    if (ticket.OneOf(cards))
                    {
                        ProcessReader(reader, connection, client);
                    }
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Загружаем конфигурацию.
        /// </summary>
        public static void LoadConfiguration()
        {
            Log.Trace("Reminder::LoadConfiguration: enter");

            var config
                = DicardsConfiguration.LoadConfiguration(OsmiUtility.DicardsJson());
            config.Verify(true);
            Configuration = config;

            ConnectionString = config.ConnectionString.ThrowIfNull("config.ConnectionString");
            var settings = new ConnectionSettings();
            settings.ParseConnectionString(ConnectionString);
            if (!settings.Verify(false))
            {
                throw new ApplicationException("Bad connection string");
            }

            ApiUrl = config.BaseUri;
            ApiId = config.ApiId;
            ApiKey = config.ApiKey;
            IdTag = config.ReaderId.SafeToInt32(30);

            Log.Trace("Reminder::LoadConfiguration: exit");
        }

        /// <summary>
        /// Проделываем работу по импорту.
        /// </summary>
        public static void DoWork()
        {
            try
            {
                Log.Trace("Reminder::DoWork: enter");

                var client = CreateClient();
                var cards = client.GetCardList();
                ProcessReaders(cards, client);

                Log.Trace("Reminder::DoWork: exit");
            }
            catch (Exception exception)
            {
                Log.TraceException("Reminder::DoWork", exception);
            }
        }

        /// <summary>
        /// Создаём клиента API DiCARDS.
        /// </summary>
        [NotNull]
        public static OsmiCardsClient CreateClient()
        {
            var result = new OsmiCardsClient
                (
                    ApiUrl,
                    ApiId,
                    ApiKey
                );

            return result;
        }

        /// <summary>
        /// Подключаемся к серверу ИРБИС64.
        /// </summary>
        [NotNull]
        public static IrbisConnection CreateConnection()
        {
            var result = new IrbisConnection(ConnectionString);

            return result;
        }

        #endregion

    }
}
