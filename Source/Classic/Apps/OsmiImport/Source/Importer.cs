// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement

/* Importer.cs -- pulls cards, imports readers
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM;
using AM.Collections;
using AM.Configuration;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Readers;

using RestfulIrbis.OsmiCards;
// ReSharper disable UseNameofExpression

#endregion

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

    /*
     * Типичная регистрация:
     *
     * {
     *   "registrations": [
     *     {
     *       "Имя": "Владимир",
     *       "Фамилия": "Маяковский",
     *       "Отчество": "-empty-",
     *       "Пол": "Мужской",
     *       "Дата_рождения": "01.08.1959",
     *       "Телефон": "77892071374",
     *       "Телефон_проверен": "-empty-",
     *       "Оферта_принята": "1",
     *       "Предложения": "-empty-",
     *       "Мероприятия": "-empty-",
     *       "DTS": "2020-08-07T21:39:52Z",
     *       "serialNo": "IR00008"
     *     }
     *   ]
     * }
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

        /// <summary>
        /// Категория, присваиваемая импортированным читателям.
        /// </summary>
        public static string ApiCategory { get; set; }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Загружаем конфигурацию.
        /// </summary>
        public static void LoadConfiguration
            (
                [NotNull] string[] args
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
            ApiCategory = ConfigurationUtility.GetString("category");

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

            var result = true;

            if (string.IsNullOrEmpty(questionnaire.Email))
            {
                Log.Info
                    (
                        "Import::CanImport: missing email for "
                        + questionnaire.SerialNumber
                    );
                result = false;
            }

            Log.Trace("Importer::CanImport: result=" + result);
            Log.Trace("Importer::CanImport: exit");

            return result;
        }

        /// <summary>
        /// Преобразуем анкету в данные о читателе.
        /// </summary>
        [NotNull]
        public static ReaderInfo ToReader
            (
                [NotNull] OsmiRegistrationInfo questionnaire
            )
        {
            Code.NotNull(questionnaire, "questionnaire");

            var gender = GenderUtility.Parse(questionnaire.Gender);
            DateTime birth;
            bool haveBirth = DateTime.TryParseExact
                (
                    questionnaire.BirthDate,
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out birth
                );
            var result = new ReaderInfo
            {
                Ticket = TicketPrefix + questionnaire.SerialNumber,
                FirstName = questionnaire.Name,
                Patronymic = questionnaire.MiddleName,
                FamilyName = questionnaire.Surname,
                Gender = GenderUtility.ToIrbis(gender),
                Category = ApiCategory,
            };

            if (haveBirth)
            {
                result.DateOfBirth = birth.ToString
                    (
                        "yyyy",
                        CultureInfo.InvariantCulture
                    );
            }

            return result;
        }

        /// <summary>
        /// Поиск, нет ли у нас в базе такого читателя.
        /// </summary>
        [CanBeNull]
        public static ReaderInfo FindReader
            (
                [NotNull] ReaderManager manager,
                [NotNull] ReaderInfo reader
            )
        {
            Code.NotNull(manager, "manager");
            Code.NotNull(reader, "reader");

            ReaderInfo result = null;

            if (!string.IsNullOrEmpty(reader.Ticket))
            {
                result = manager.GetReader(reader.Ticket);
                if (!ReferenceEquals(result, null))
                {
                    return result;
                }
            }

            if (!string.IsNullOrEmpty(reader.Email))
            {
                result = manager.FindReader("\"EMAIL={0}\"", reader.Email);
                if (!ReferenceEquals(result, null))
                {
                    return result;
                }

                result = manager.FindReader("\"MAIL={0}\"", reader.Email);
                if (!ReferenceEquals(result, null))
                {
                    return result;
                }
            }

            if (!string.IsNullOrEmpty(reader.HomePhone))
            {
                result = manager.FindReader("\"PHONE={0}\"", reader.HomePhone);
                if (!ReferenceEquals(result, null))
                {
                    return result;
                }
            }

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

            var result = false;
            var reader = ToReader(questionnaire);
            if (!reader.Verify(false))
            {
                Log.Info
                    (
                        "Importer::ImportReader: reader not verified: "
                         + questionnaire.SerialNumber
                    );
                goto DONE;
            }

            var record = reader.ToRecord();
            if (!record.Verify(false))
            {
                Log.Info
                    (
                        "Importer::ImportReader: record not verified: "
                        + questionnaire.SerialNumber
                    );
                goto DONE;
            }

            manager.Connection.WriteRecord(record);

            result = true;

            DONE:
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
