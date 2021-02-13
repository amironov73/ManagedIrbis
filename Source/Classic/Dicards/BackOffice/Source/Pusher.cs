// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Pusher.cs -- пушит сообщения читателям
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Logging;
using DevExpress.Utils.Text;
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
    /// Пушит читателям сообщения "Пора вернуть книги в библиотеку"
    /// </summary>
    static class Pusher
    {
        #region Properties

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
        /// Текст сообщения.
        /// </summary>
        public static string Message { get; set; }

        public static DicardsConfiguration Config { get; set; }

        #endregion

        #region Private members

        private static string[] GetDebtors
            (
                string[] cards,
                IrbisConnection connection
            )
        {
            var deadline = DateTime.Today.AddDays(-1.0);

            var result = new List<string>();
            // Получаем всех читателей из базы RDR
            IEnumerable<MarcRecord> batch = BatchRecordReader.Search
                (
                    connection,
                    "RDR",
                    "RB=$",
                    1000
                );

            foreach (MarcRecord record in batch)
            {
                var reader = ReaderInfo.Parse(record);
                string ticket = OsmiUtility.GetReaderId(reader, Config);
                if (!ticket.OneOf(cards))
                {
                    continue;
                }

                // Массив просроченных книг
                var outdated = reader.Visits
                    .Where(v => !v.IsVisit)
                    .Where(v => !v.IsReturned)
                    .Where(v => v.DateExpected < deadline)
                    .ToArray();

                if (outdated.Length != 0)
                {
                    result.Add(reader.Ticket);
                }
            }

            return result.ToArray();
        }

        private static void SendMessageToReader
            (
                [NotNull] OsmiCardsClient client,
                [NotNull] string reader,
                [NotNull] string field,
                [NotNull] string message,
                bool push
            )
        {
            var card = client.GetRawCard(reader);
            if (card != null)
            {
                CardUpdater.UpdateField
                    (
                        card,
                        field,
                        message
                    );
                client.UpdateCard
                    (
                        reader,
                        card.ToString(),
                        push
                    );
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Загружаем конфигурацию.
        /// </summary>
        public static void LoadConfiguration()
        {
            Log.Trace("Pusher::LoadConfiguration: enter");

            var config
                = DicardsConfiguration.LoadConfiguration(OsmiUtility.DicardsJson());
            config.Verify(true);
            Config = config;

            ConnectionString = config.ConnectionString.ThrowIfNull("ConnectionString");
            ConnectionSettings settings = new ConnectionSettings();
            settings.ParseConnectionString(ConnectionString);
            if (!settings.Verify(false))
            {
                throw new ApplicationException("Bad connection string");
            }

            ApiUrl = config.BaseUri;
            ApiId = config.ApiId;
            ApiKey = config.ApiKey;
            Message = config.ReminderMessage;

            if (string.IsNullOrEmpty(Message))
            {
                throw new ApplicationException("Bad message text");
            }

            Log.Trace("Pusher::LoadConfiguration: exit");
        }

        /// <summary>
        /// Проделываем работу по импорту.
        /// </summary>
        public static void DoWork()
        {
            try
            {
                Log.Trace("Pusher::DoWork: enter");

                var client = CreateClient();
                var cards = client.GetCardList();
                string[] debtors;
                using (var connection = CreateConnection())
                {
                    debtors = GetDebtors(cards, connection);
                }

                if (debtors.Length == 0)
                {
                    Log.Info("Pusher: no debtors");
                }
                else
                {
                    /*
                    client.SendPushMessage
                        (
                            debtors,
                            Message
                        );
                    Log.Info("Pusher: SendPushMessage OK");
                    */

                    var messageField = Config.ReminderField;
                    var messageText = Config.ReminderMessage;
                    if (!string.IsNullOrEmpty(messageField)
                        && !string.IsNullOrEmpty(messageText))
                    {
                        foreach (var debtor in debtors)
                        {
                            SendMessageToReader
                                (
                                    client,
                                    debtor,
                                    messageField,
                                    messageText,
                                    true
                                );
                        }

                        var readers = client.GetCardList();
                        var nonDebtors = readers.Except(debtors);

                        foreach (var nonDebtor in nonDebtors)
                        {
                            SendMessageToReader
                            (
                                client,
                                nonDebtor,
                                messageField,
                                string.Empty,
                                false
                            );
                        }

                        Log.Info("Pusher: SendMessageToReader OK");
                    }
                }


                Log.Trace("Pusher::DoWork: exit");
            }
            catch (Exception exception)
            {
                Log.TraceException("Pusher::DoWork", exception);
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
