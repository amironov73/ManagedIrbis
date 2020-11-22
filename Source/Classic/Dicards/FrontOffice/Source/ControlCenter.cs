﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
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

using AM;
using AM.IO;
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

        // /// <summary>
        // /// Configuration.
        // /// </summary>
        //public static JObject Configuration { get; set; }

        /// <summary>
        /// Connection string for IRBIS-server.
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// Debug output.
        /// </summary>
        public static AbstractOutput Output { get; set; }

        /// <summary>
        /// Template itself.
        /// </summary>
        public static JObject Template { get; set; }

        public static string TemplateName { get; set; }

        #endregion

        #region Private members

        private static string DicardsJson()
        {
            var result = PathUtility.MapPath("dicards.json");

            return result;
        }

        #endregion

        #region Public methods

        public static JObject BuildCard
            (
                [NotNull] ReaderInfo reader
            )
        {
            Code.NotNull(reader, "reader");

            JObject result = OsmiUtility.BuildCardForReader
                (
                    Template,
                    reader
                );

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

                WriteLine
                    (
                        "Карта: {0}",
                        result ? "существует" : "не существует"
                    );

                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CheckReader
            (
                [NotNull] ReaderInfo reader
            )
        {
            Code.NotNull(reader, "reader");

            WriteLine(string.Empty);

            var ticket = reader.PassCard ?? reader.Ticket;
            if (string.IsNullOrEmpty(ticket))
            {
                WriteLine("Отсутствует идентификатор!");
                return false;
            }

            WriteLine("Читатель {0}:", ticket);
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

            WriteLine("Читатель удовлетворяет требованиям OSMICARDS");
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

            WriteLine("Карта создана");
        }

        public static void DeleteCard
            (
                [NotNull] ReaderInfo reader
            )
        {
            Code.NotNull(reader, "reader");

            string ticket = (reader.PassCard ?? reader.Ticket)
                .ThrowIfNull("ticket");
            Client.DeleteCard
                (
                    ticket,
                    true
                );
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

            using (IrbisConnection connection = GetIrbisConnection())
            {
                connection.WriteRecord(record);
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
            try
            {
                using (var connection = GetIrbisConnection())
                {
                    connection.Connect();
                    var serverVersion = connection.GetServerVersion();
                    WriteLine("ИРБИС64: {0}\r\n", serverVersion);
                    int maxMfn = connection.GetMaxMfn(connection.Database);
                    WriteLine("Max MFN={0}\r\n", maxMfn);
                }
            }
            catch (Exception exception)
            {
                WriteLine("ERROR: {0}", exception.Message);
                return false;
            }

            return true;
        }

        private static bool CheckOsmi()
        {
            try
            {
                WriteLine("Подключаемся к API. Считываем шаблон\r\n");
                var template = Client.GetTemplateInfo(TemplateName).ToString();
                if (string.IsNullOrEmpty(template))
                {
                    WriteLine("Ошибка при получении шаблона");
                    return false;
                }
            }
            catch (Exception exception)
            {
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
                = DicardsConfiguration.LoadConfiguration(DicardsJson());

            return config.Verify(false)
                && CheckIrbis() && CheckOsmi();
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
                = DicardsConfiguration.LoadConfiguration(DicardsJson());
            config.Verify(true);

            Output = output;

            ConnectionString = config.ConnectionString;
            string baseUri = config.BaseUri;
            string apiId = config.ApiId;
            string apiKey = config.ApiKey;
            Client = new OsmiCardsClient
                (
                    baseUri,
                    apiId,
                    apiKey
                );

            TemplateName = config.Template;
            WriteLine("Reading OSMI template: {0}", TemplateName);
            Template = Client.GetTemplateInfo(TemplateName);
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

        public static void SendEmail
            (
                [NotNull] ReaderInfo reader
            )
        {
            Code.NotNull(reader, "reader");

            string ticket = (reader.PassCard ?? reader.Ticket)
                .ThrowIfNull("ticket");
            string[] emails = reader
                .Record.ThrowIfNull("reader.Record")
                .FMA(32);

            foreach (string email in emails)
            {
                Client.SendCardMail
                    (
                        ticket,
                        email
                    );

                WriteLine("Послано письмо по адресу {0}", email);
            }
        }

        public static void SendSms
            (
                [NotNull] ReaderInfo reader
            )
        {
            Code.NotNull(reader, "reader");

            string ticket = (reader.PassCard ?? reader.Ticket)
                .ThrowIfNull("ticket");
            string phoneNumber = reader.HomePhone.ThrowIfNull();

            Client.SendCardSms
                (
                    ticket,
                    phoneNumber
                );

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
