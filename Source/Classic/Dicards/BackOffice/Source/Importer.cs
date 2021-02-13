// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Importer.cs -- pulls cards, imports readers
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 */

#region Using directives

using System;
using System.Linq;
using System.Text.RegularExpressions;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;

using CodeJam;

using DicardsConfig;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Readers;

using RestfulIrbis.OsmiCards;

#endregion

namespace BackOffice
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

        /// <summary>
        /// Конфигурация в целом.
        /// </summary>
        public static DicardsConfiguration Configuration { get; set; }

        #endregion

        #region Private members

        [NotNull]
        private static string DicardsJson()
        {
            var result = PathUtility.MapPath("dicards.json")
                .ThrowIfNull("MapPath (\"dicards.json\")");

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Загружаем конфигурацию.
        /// </summary>
        public static void LoadConfiguration()
        {
            Log.Trace("Importer::LoadConfiguration: enter");

            var config
                = DicardsConfiguration.LoadConfiguration(DicardsJson());
            config.Verify(true);
            Configuration = config;

            ConnectionString = config.ConnectionString.ThrowIfNull("ConnectionString");
            var settings = new ConnectionSettings();
            settings.ParseConnectionString(ConnectionString);
            if (!settings.Verify(false))
            {
                throw new ApplicationException("Bad connection string");
            }

            ApiUrl = config.BaseUri;
            ApiId = config.ApiId;
            ApiKey = config.ApiKey;
            ApiGroup = config.Group;
            TicketPrefix = config.Prefix;
            ApiCategory = config.Category;

            Log.Trace("Importer::LoadConfiguration: exit");
        }

        /// <summary>
        /// Проделываем работу по импорту.
        /// </summary>
        public static void DoWork()
        {
            try
            {
                Log.Trace("Importer::DoWork: enter");

                var registrations = GetRegistrations();
                if (registrations.IsNullOrEmpty())
                {
                    Log.Info("Importer::DoWork: no registrations");
                    goto DONE;
                }

                var counter = 0;
                using (var connection = CreateConnection())
                {
                    var manager = new ReaderManager(connection);
                    var client = CreateClient();
                    foreach (var questionnaire in registrations)
                    {
                        var reader = ToReader(questionnaire);
                        if (CanImport(manager, reader))
                        {
                            ImportReader
                                (
                                    manager,
                                    reader,
                                    client
                                );
                            ++counter;
                        }
                    }
                }

                Log.Info($"Importer::DoWork: total imported={counter}");

                DeleteRegistrations(registrations);
                Log.Info("Importer::DoWork: registrations deleted");

                DONE:
                Log.Trace("Importer::DoWork: exit");
            }
            catch (Exception exception)
            {
                Log.TraceException("Importer::DoWork", exception);
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
                    $"Importer::GetRegistrations: got {result.Length} records"
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
                [NotNull] ReaderInfo reader
            )
        {
            Code.NotNull(manager, nameof(manager));
            Code.NotNull(reader, nameof(reader));

            Log.Trace("Importer::CanImport: enter");

            var found = FindReader(manager, reader);
            var result = found is null;

            /*

            Временно выключаем проверку E-mail,
            т. к. тестовый стенд от DICARDS не умеет запрашивать почту.

            if (string.IsNullOrEmpty(questionnaire.Email))
            {
                Log.Info
                    (
                        "Importer::CanImport: missing email for "
                        + questionnaire.SerialNumber
                    );
                result = false;
            }

            */

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
            Code.NotNull(questionnaire, nameof(questionnaire));

            var gender = GenderUtility.Parse(questionnaire.Gender);
            var birthYear = Regex.Match
                (
                    questionnaire.BirthDate,
                    @"\d{4}"
                )
                .Value;
            var result = new ReaderInfo
            {
                Ticket = TicketPrefix + questionnaire.SerialNumber,
                FirstName = questionnaire.Name,
                Patronymic = questionnaire.MiddleName,
                FamilyName = questionnaire.Surname,
                Gender = GenderUtility.ToIrbis(gender),
                Category = ApiCategory,
                DateOfBirth = birthYear,
                HomePhone = questionnaire.Phone
            };

            var record = result.ToRecord();
            record.RemoveField(30).RemoveField(22);
            var ticketTag = Configuration.ReaderId.SafeToInt32(30);
            record.SetField(ticketTag, result.Ticket);
            var barcodeTag = Configuration.Ticket.SafeToInt32(22);
            record.SetField(barcodeTag, result.Ticket);
            result.Record = record;

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
            Code.NotNull(manager, nameof(manager));
            Code.NotNull(reader, nameof(reader));

            ReaderInfo result;

            if (!string.IsNullOrEmpty(reader.Ticket))
            {
                result = manager.GetReader(reader.Ticket);
                if (!ReferenceEquals(result, null))
                {
                    Log.Trace($"Importer::FindReader: already have ticket {reader.Ticket}");
                    return result;
                }
            }

            if (!string.IsNullOrEmpty(reader.Email))
            {
                result = manager.FindReader("\"EMAIL={0}\"", reader.Email);
                if (!ReferenceEquals(result, null))
                {
                    Log.Trace($"Importer::FindReader: already have email {reader.Email}");
                    return result;
                }

                result = manager.FindReader("\"MAIL={0}\"", reader.Email);
                if (!ReferenceEquals(result, null))
                {
                    Log.Trace($"Importer::FindReader: already have email {reader.Email}");
                    return result;
                }
            }

            if (!string.IsNullOrEmpty(reader.HomePhone))
            {
                result = manager.FindReader("\"PHONE={0}\"", reader.HomePhone);
                if (!ReferenceEquals(result, null))
                {
                    Log.Trace($"Importer::FindReader: already have phone {reader.HomePhone}");
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Собственно импорт читателя в базу RDR.
        /// </summary>
        public static bool ImportReader
            (
                [NotNull] ReaderManager manager,
                [NotNull] ReaderInfo reader,
                [NotNull] OsmiCardsClient client
            )
        {
            Code.NotNull(manager, nameof(manager));
            Code.NotNull(reader, nameof(reader));

            Log.Trace("Importer::ImportReader: enter");

            var result = false;
            var ticket = OsmiUtility.GetReaderId(reader, Configuration);
            Log.Trace($"Importer::ImportReader: ticket={ticket}");

            if (!reader.Verify(false))
            {
                Log.Info
                    (
                        $"Importer::ImportReader: reader not verified: {ticket}"
                    );
                goto DONE;
            }

            var record = reader.Record.ThrowIfNull("reader.Record");
            if (!record.Verify(false))
            {
                Log.Info
                    (
                        $"Importer::ImportReader: record not verified: {ticket}"
                    );
                goto DONE;
            }

            reader.Visits = Array.Empty<VisitInfo>();
            manager.Connection.WriteRecord(record);

            // Заполняем поля карточки читателя
            CardUpdater.UpdateReaderCard
                (
                    reader,
                    Configuration,
                    manager.Connection,
                    client
                );

            result = true;

            DONE:
            Log.Info($"Importer::ImportReader: ticket={ticket},  result={result}");
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
