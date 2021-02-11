// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* Pusher.cs -- пушит сообщения читателям
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;
using AM.IO;
using AM.Logging;

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

        /// <summary>
        /// Метка поля в базе данных читателей,
        /// в котором хранится идентификатор читателя.
        /// По умолчанию это поле 30.
        /// </summary>
        public static int IdTag { get; set; }

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
                string ticket = record.FM(IdTag).ThrowIfNull("ticket");
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

        private static string DicardsJson()
        {
            var result = PathUtility.MapPath("dicards.json");

            return result;
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
                = DicardsConfiguration.LoadConfiguration(DicardsJson());
            config.Verify(true);

            ConnectionString = config.ConnectionString;
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
            IdTag = config.ReaderId.SafeToInt32(30);

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

                if (debtors.Length != 0)
                {
                    client.SendPushMessage
                        (
                            debtors,
                            Message
                        );
                    Console.WriteLine("Send OK");
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
