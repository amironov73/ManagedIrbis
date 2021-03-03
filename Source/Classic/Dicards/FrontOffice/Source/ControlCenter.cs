// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable SuggestVarOrType_BuiltInTypes
// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_Elsewhere
// ReSharper disable UseNameofExpression

/* ControlCenter.cs -- операции с серверами DICARDS и ИРБИС.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 */

#region Using directives

using System;
using System.Globalization;

using AM;
using AM.IO;
using AM.Logging;
using AM.Net;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Readers;

using Newtonsoft.Json.Linq;

using RestfulIrbis.OsmiCards;

#endregion

namespace FrontOffice
{
    /// <summary>
    /// Операции с серверами DICARDS и ИРБИС.
    /// </summary>
    [PublicAPI]
    public static class ControlCenter
    {
        #region Properties

        /// <summary>
        /// OSMICards client.
        /// </summary>
        public static OsmiCardsClient Client { get; set; }

         /// <summary>
         /// Configuration.
         /// </summary>
        public static DicardsConfiguration Config { get; set; }

        /// <summary>
        /// Connection string for IRBIS-server.
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// Debug output.
        /// </summary>
        public static AbstractOutput Output { get; set; }

        /// <summary>
        /// Поле для хранения идентификатора читателя.
        /// </summary>
        public static int ReaderIdTag { get; set; }

        /// <summary>
        /// Поле для хранения номера пропуска читателя.
        /// </summary>
        public static int TicketTag { get; set; }

        /// <summary>
        /// Собственно шаблон.
        /// </summary>
        public static JObject Template { get; set; }

        /// <summary>
        /// Имя шаблона.
        /// </summary>
        public static string TemplateName { get; set; }

        /// <summary>
        /// Имя поля в карточке читателя, в которое будет
        /// помещено ФИО читателя.
        /// </summary>
        public static string FioField { get; set; }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public static JObject BuildCard
            (
                [NotNull] ReaderInfo reader,
                [NotNull] string ticket
            )
        {
            Code.NotNull(reader, "reader");

            var result = OsmiUtility.BuildCardForReader
                (
                    Template,
                    reader,
                    ticket,
                    Config
                );

            Log.Debug("ControlCenter::BuildCard for ticket=" + ticket);
            Log.Debug("Result=" + result);

            return result;
        }

        public static bool CardExists
            (
                [NotNull] string cardNumber
            )
        {
            Code.NotNullNorEmpty(cardNumber, "cardNumber");

            try
            {
                OsmiCard card = Client.GetCardInfo(cardNumber);
                bool result = !ReferenceEquals(card, null);

                Log.Debug("ControlCenter::CardExists for ticket=" + cardNumber);
                Log.Debug("Result=" + result);

                WriteLine
                    (
                        "Карта: {0}",
                        result ? "существует" : "не существует"
                    );

                return result;
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                    "ControlCenter::CardExists for ticket=" + cardNumber,
                        exception
                    );

                return false;
            }
        }

        [CanBeNull]
        public static string GetTicket
            (
                [NotNull] ReaderInfo reader
            )
        {
            return reader.Record
                .ThrowIfNull("reader.Record")
                .FM(ReaderIdTag);
        }

        [CanBeNull]
        public static string GetReaderId
            (
                [NotNull] ReaderInfo reader
            )
        {
            return reader.Record
                .ThrowIfNull("reader.Record")
                .FM(TicketTag);
        }

        [NotNull]
        public static string GetReaderName
            (
                [NotNull] ReaderInfo reader
            )
        {
            return reader.FamilyName
                .ThrowIfNull("reader.FamilyName");
        }

        [CanBeNull]
        public static string GetIdentifier
            (
                [NotNull] ReaderInfo reader
            )
        {
            return GetReaderId(reader) ?? GetTicket(reader);
        }

        public static bool CheckReader
            (
                [NotNull] ReaderInfo reader
            )
        {
            Code.NotNull(reader, "reader");

            WriteLine(string.Empty);

            var ticket = GetIdentifier(reader);
            if (string.IsNullOrEmpty(ticket))
            {
                WriteLine("Отсутствует идентификатор!");
                return false;
            }

            WriteLine("Читатель {0}:", GetReaderName(reader));
            if (string.IsNullOrEmpty(reader.FamilyName))
            {
                WriteLine("Отсутствует фамилия!");
                return false;
            }

            string email = reader.Email;

            if (string.IsNullOrEmpty(email))
            {
                WriteLine("Отсутствует email!");
                return false;
            }

            if (!MailUtility.VerifyEmail(email))
            {
                WriteLine("Неверный email!");
                return false;
            }

            WriteLine("Читатель удовлетворяет требованиям DiCARDS");

            return true;
        }

        public static void CreateCard
            (
                [NotNull] string cardNumber,
                [NotNull] JObject card
            )
        {
            Code.NotNullNorEmpty(cardNumber, "cardNumber");
            Code.NotNull(card, "card");

            Client.CreateCard
                (
                    cardNumber,
                    TemplateName,
                    card.ToString()
                );

            Log.Debug("ControlCenter::CreateCard for ticket=" + cardNumber);
            Log.Debug("Result=True");

            WriteLine("Карта создана");
        }

        public static void DeleteCard
            (
                [NotNull] string ticket
            )
        {
            Client.DeleteCard
                (
                    ticket,
                    true
                );

            Log.Debug("ControlCenter::DeleteCard for ticket=" + ticket);
            Log.Debug("Result=True");
        }

        /// <summary>
        /// Get <see cref="IrbisConnection"/>.
        /// </summary>
        [NotNull]
        public static IrbisConnection GetIrbisConnection()
        {
            IrbisConnection result
                = new IrbisConnection(ConnectionString);

            return result;
        }

        /// <summary>
        /// Get reader by ticket.
        /// </summary>
        [CanBeNull]
        public static ReaderInfo GetReader
            (
                [NotNull] string ticket
            )
        {
            Code.NotNullNorEmpty(ticket, "ticket");

            using (IrbisConnection connection = GetIrbisConnection())
            {
                ReaderManager manager = new ReaderManager(connection);
                ReaderInfo result = manager.GetReader(ticket);

                Log.Debug("ControlCenter::GetReader for ticket=" + ticket);
                Log.Debug("Result=" + result);

                return result;
            }
        }

        /// <summary>
        /// Обновляет запись на сервере.
        /// </summary>
        public static void UpdateRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            Log.Debug("ControlCenter::UpdateRecord for record=" + record.Mfn);
            using (IrbisConnection connection = GetIrbisConnection())
            {
                connection.WriteRecord(record);

                Log.Debug("Result=True");

                WriteLine("Запись обновлена на сервере");
            }
        }

        /// <summary>
        /// Format reader.
        /// </summary>
        public static string FormatReader
            (
                [NotNull] ReaderInfo reader
            )
        {
            Code.NotNull(reader, "reader");

            using (IrbisConnection connection = GetIrbisConnection())
            {
                string result = connection.FormatRecord("@", reader.Mfn);

                return result;
            }
        }

        private static bool CheckIrbis()
        {
            Log.Debug("ControlCenter::CheckIrbis");

            try
            {
                using (var connection = GetIrbisConnection())
                {
                    connection.Connect();
                    var serverVersion = connection.GetServerVersion();
                    WriteLine("ИРБИС64: {0}\r\n", serverVersion);
                    Log.Debug("Result=" + serverVersion);
                    int maxMfn = connection.GetMaxMfn(connection.Database);
                    WriteLine("Max MFN={0}\r\n", maxMfn);
                }
            }
            catch (Exception exception)
            {
                Log.TraceException("ControlCenter::CheckIrbis", exception);
                WriteLine("ERROR: {0}", exception.Message);
                return false;
            }

            return true;
        }

        private static bool CheckOsmi()
        {
            Log.Debug("ControlCenter::CheckOsmi");

            try
            {
                WriteLine("Подключаемся к API. Считываем шаблон\r\n");
                var template = Client.GetTemplateInfo(TemplateName).ToString();
                Log.Debug("Result=" + template.ToVisibleString());
                if (string.IsNullOrEmpty(template))
                {
                    WriteLine("Ошибка при получении шаблона");
                    return false;
                }
            }
            catch (Exception exception)
            {
                Log.TraceException("ControlCenter::CheckOsmi", exception);
                WriteLine("ERROR: {0}", exception.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Проверка конфигурации.
        /// </summary>
        /// <returns><code>True</code>, если приложение сконфигурировано</returns>
        public static bool CheckConfiguration()
        {
            var config
                = DicardsConfiguration.LoadConfiguration(OsmiUtility.DicardsJson());


            var result = config.Verify(false)
                && CheckIrbis() && CheckOsmi();

            Log.Debug("ControlCenter::CheckConfiguration result=" + result);

            return result;
        }

        /// <summary>
        /// Initialize.
        /// </summary>
        public static void Initialize
            (
                [CanBeNull] AbstractOutput output
            )
        {
            var config
                = DicardsConfiguration.LoadConfiguration(OsmiUtility.DicardsJson());
            config.Verify(true);
            Config = config;

            Output = output;

            ConnectionString = config.ConnectionString;
            var baseUri = config.BaseUri.ThrowIfNull("config.BaseUri");
            var apiId = config.ApiId.ThrowIfNull("config.apiId");
            var apiKey = config.ApiKey.ThrowIfNull("config.apiKey");
            Client = new OsmiCardsClient
                (
                    baseUri,
                    apiId,
                    apiKey
                );

            var culture = CultureInfo.InvariantCulture;
            var styles = NumberStyles.Any;
            ReaderIdTag = int.Parse
                (
                    config.ReaderId.ThrowIfNull("config.ReaderId"),
                    styles,
                    culture
                );
            WriteLine("Reader ID tag={0}", ReaderIdTag);
            TicketTag = int.Parse
                (
                    config.Ticket.ThrowIfNull("config.Ticket"),
                    styles,
                    culture
                );
            WriteLine("Ticket tag={0}", TicketTag);

            TemplateName = config.Template;
            FioField = config.FioField;
            WriteLine("Reading DiCARDS template: {0}", TemplateName);
            Template = Client.GetTemplateInfo
                (
                    config.Template.ThrowIfNull("config.Template")
                );
        }

        public static bool Ping()
        {
            using (IrbisConnection connection = GetIrbisConnection())
            {
                WriteLine("Pinging IRBIS-server");
                connection.NoOp();
            }

            return true;
        }

        /// <summary>
        /// Отправка письма со ссылкой на карту для указанного читателя.
        /// </summary>
        /// <param name="reader">Читатель</param>
        public static void SendEmail
            (
                [NotNull] ReaderInfo reader
            )
        {
            Code.NotNull(reader, "reader");

            Log.Debug("ControlCenter::SendEmail for reader=" + reader);

            var readerId = GetReaderId(reader);
            var ticket = GetTicket(reader);
            var emails = reader
                .Record.ThrowIfNull("reader.Record")
                .FMA(32);

            Log.Trace("ControlCenter::SendEmail: emails="
                + string.Join(" ; ", emails));

            foreach (var email in emails)
            {
                if (!string.IsNullOrEmpty(ticket))
                {
                    Client.SendCardMail(ticket, email);
                }

                if (!string.IsNullOrEmpty(readerId)
                    && !readerId.SameString(ticket))
                {
                    Client.SendCardMail(readerId, email);
                }

                Log.Debug("Sent email to " + email);
                WriteLine("Послано письмо по адресу {0}", email);
            }
        }

        /// <summary>
        /// Посылка SMS указанному читателю.
        /// </summary>
        /// <param name="reader">Читатель</param>
        public static void SendSms
            (
                [NotNull] ReaderInfo reader
            )
        {
            Code.NotNull(reader, "reader");

            Log.Debug("ControlCenter::SendSms for reader=" + reader);

            var readerId = GetReaderId(reader);
            var ticket = GetTicket(reader);
            var phoneNumber = reader.HomePhone.ThrowIfNull();

            if (!string.IsNullOrEmpty(ticket))
            {
                Client.SendCardSms(ticket, phoneNumber);
            }
            if (!string.IsNullOrEmpty(readerId)
                && !readerId.SameString(ticket))
            {
                Client.SendCardSms(readerId, phoneNumber);
            }

            Log.Debug("Sent SMS to " + phoneNumber);
            WriteLine("Послано сообщение на номер {0}", phoneNumber);
        }

        /// <summary>
        /// Shutdown.
        /// </summary>
        public static void Shutdown()
        {
            // Nothing to do here yet
        }

        /// <summary>
        /// Write debug line.
        /// </summary>
        public static void WriteLine
            (
                [NotNull] string format,
                params object[] arguments
            )
        {
            Code.NotNull(format, "format");

            Output?.WriteLine(format, arguments);
        }

        #endregion
    }
}
