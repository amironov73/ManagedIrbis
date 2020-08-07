// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Importer.cs -- pulls cards, imports readers
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

using AM.Collections;
using AM.Configuration;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Readers;

using RestfulIrbis.OsmiCards;

#endregion

// ReSharper disable LocalizableElement

namespace OsmiImport
{

    /*
     * API URL - https://api.dicards.ru/v2
     * API ID-  IB2Y7H3OJK01AGJ1SS3W
     * API KEY - 015ad52cd62104479f0dd1df79e1e244d48222fa
     * Ссылка на анкету для установки карт
     * https://hub.dicards.ru/a/220d94259e658a7c874e142f56583795b5b8339e/0
     *
     * Шаблон карты - IRBIS 
     * Регистрационная группа - IRBIS
     */

    /// <summary>
    /// Занимается выгрузкой регистрационных данных из DiCARDS, импортом в ИРБИС64,
    /// удаляет импортированные карты.
    /// </summary>
    static class Importer
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
        /// Префикс номера читательского.
        /// </summary>
        public static string TicketPrefix { get; set; }

        /// <summary>
        /// API group name.
        /// </summary>
        public static string ApiGroup { get; set; }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Загружаем конфигурацию.
        /// </summary>
        public static void LoadConfiguration
            (
                string[] args
            )
        {
            Log.Trace("Importer::LoadConfiguration: enter");

            ConnectionString = IrbisConnectionUtility
                .GetStandardConnectionString();
            if (string.IsNullOrEmpty(ConnectionString))
            {
                ConfigurationUtility.ThrowKeyNotSet("connectionString");
            }

            ConnectionSettings settings = new ConnectionSettings();
            settings.ParseConnectionString(ConnectionString);
            if (!settings.Verify(false))
            {
                ConfigurationUtility.ThrowKeyNotSet("connectionString");
            }

            ApiUrl = ConfigurationUtility.RequireString("baseUri");
            ApiId = ConfigurationUtility.RequireString("apiId");
            ApiKey = ConfigurationUtility.RequireString("apiKey");
            ApiGroup = ConfigurationUtility.RequireString("group");
            TicketPrefix = ConfigurationUtility.GetString("prefix");

            Log.Trace("Importer::LoadConfiguration: exit");
        }

        /// <summary>
        /// Проделываем работу по импорту.
        /// </summary>
        public static void DoWork()
        {
            Log.Trace("Importer::DoWork: enter");

            var registrations = GetRegistrations();
            if (registrations.IsNullOrEmpty())
            {
                Log.Info("no registrations");
                goto DONE;
            }

            int counter = 0;
            using (var connection = CreateConnection())
            {
                ReaderManager manager = new ReaderManager(connection);
                foreach (var questionnaire in registrations)
                {
                    if (CanImport(manager, questionnaire))
                    {
                        ImportReader
                            (
                                manager,
                                questionnaire
                            );
                        ++counter;
                    }
                }
            }

            Log.Info("Importer::DoWork: total imported=" + counter);

            DeleteRegistrations(registrations);
            Log.Info("Importer::DoWork: registrations deleted");

DONE:
            Log.Trace("Importer::DoWork: exit");
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

        /// <summary>
        /// Получаем список регистраций.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public static OsmiRegistrationInfo[] GetRegistrations()
        {
            Log.Trace("Importer::GetRegistrations: enter");

            var client = CreateClient();
            var result = client.GetRegistrations(ApiGroup);
            Log.Info
                (
                    string.Format
                        (
                            "Importer::GetRegistrations: got {0} records",
                            result.Length
                        )
                );

            Log.Trace("Importer::GetRegistrations: exit");

            return result;
        }

        /// <summary>
        /// Можно ли импортировать указанную анкету?
        /// </summary>
        public static bool CanImport
            (
                [NotNull] ReaderManager manager,
                [NotNull] OsmiRegistrationInfo questionnaire
            )
        {
            Code.NotNull(manager, "manager");
            Code.NotNull(questionnaire, "questionnaire");

            Log.Trace("Importer::CanImport: enter");

            bool result = true;

            if (string.IsNullOrEmpty(questionnaire.Email))
            {
                Log.Info("Import::CanImport: missing email for " + questionnaire.SerialNumber);
                result = false;
            }

            Log.Trace("Importer::CanImport: result=" + result);
            Log.Trace("Importer::CanImport: exit");

            return result;
        }

        public static bool ImportReader
            (
                [NotNull] ReaderManager manager,
                [NotNull] OsmiRegistrationInfo questionnaire
            )
        {
            Code.NotNull(manager, "manager");
            Code.NotNull(questionnaire, "questionnaire");

            Log.Trace("Importer::ImportReader: enter");

            bool result = false;
            ReaderInfo reader = new ReaderInfo
            {
                Ticket = TicketPrefix + questionnaire.SerialNumber
            };
            MarcRecord record = reader.ToRecord();
            manager.Connection.WriteRecord(record);

            Log.Trace("Importer::ImportReader: result=" + result);
            Log.Trace("Importer::ImportReader: exit");

            return result;
        }

        public static void DeleteRegistrations
            (
                [NotNull] OsmiRegistrationInfo[] registrations
            )
        {
            Log.Trace("Importer::DeleteRegistrations: enter");

            var client = CreateClient();
            var numbers = registrations
                .Select(one => one.SerialNumber)
                .ToArray();
            client.DeleteRegistrations(numbers);

            Log.Trace("Importer::DeleteRegistrations: exit");
        }

        #endregion
    }
}
