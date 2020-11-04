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
 * Status: poor
 */

#region Using directives

using System;
using System.Globalization;
using System.Linq;

using AM;
using AM.Collections;
using AM.Configuration;
using AM.IO;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
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
        /// Формат для библиографического описания книги.
        /// </summary>
        public static string Format { get; set; }

        /// <summary>
        /// Поле для размещения списка книг.
        /// </summary>
        public static string Field { get; set; }

        #endregion

        #region Private members

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
            Log.Trace("Reminder::LoadConfiguration: enter");

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
            Format = config.Format;
            Field = config.Field;

            if (string.IsNullOrEmpty(Format))
            {
                throw new ApplicationException("Bad format specified");
            }

            if (string.IsNullOrEmpty(Field))
            {
                throw new ApplicationException("Bad card field specified");
            }

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


            DONE:
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
