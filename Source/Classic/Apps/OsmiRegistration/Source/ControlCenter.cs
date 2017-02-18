// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ControlCenter.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AM;
using AM.Text.Output;
using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Readers;
using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RestfulIrbis;
using RestfulIrbis.OsmiCards;
using CM = System.Configuration.ConfigurationManager;

#endregion

namespace OsmiRegistration
{
    /// <summary>
    /// 
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
        public static JObject Configuration { get; set; }

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
            WriteLine("Читатель {0}:", reader.Ticket);

            if (string.IsNullOrEmpty(reader.FamilyName))
            {
                WriteLine("Отсутствует фамилия!");
                return false;
            }

            if (string.IsNullOrEmpty(reader.Ticket))
            {
                WriteLine("Отсутствует идентификатор!");
                return false;
            }

            string email = reader.Email;

            if (string.IsNullOrEmpty(email))
            {
                WriteLine("Отсутствует email!");
                return false;
            }

            if (!VerifyEmail(email))
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

            string ticket = reader.Ticket.ThrowIfNull();

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

        /// <summary>
        /// Initialize.
        /// </summary>
        public static void Initialize()
        {
            Configuration = JObject.Parse
                (
                    File.ReadAllText("osmi.json")
                );

            ConnectionString = CM.AppSettings["connectionString"];

            string baseUri = CM.AppSettings["baseUri"];
            string apiId = CM.AppSettings["apiID"];
            string apiKey = CM.AppSettings["apiKey"];
            Client = new OsmiCardsClient
                (
                    baseUri,
                    apiId,
                    apiKey
                );

            WriteLine("Reading OSMI template");
            TemplateName = CM.AppSettings["template"];
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

            string ticket = reader.Ticket.ThrowIfNull();
            string email = reader.Email.ThrowIfNull();

            Client.SendCardMail
                (
                    ticket,
                    email
                );
            
            WriteLine("Послано письмо по адресу {0}", email);
        }

        public static void SendSms
            (
                [NotNull] ReaderInfo reader
            )
        {
            Code.NotNull(reader, "reader");

            string ticket = reader.Ticket.ThrowIfNull();
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

        }

        public static bool VerifyEmail
            (
                [NotNull] string email
            )
        {
            Code.NotNullNorEmpty(email, "email");

            bool result = Regex.IsMatch
                (
                    email, 
                    @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", 
                    RegexOptions.IgnoreCase
                );

            return result;
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

            if (!ReferenceEquals(Output, null))
            {
                Output.WriteLine(format, arguments);
            }
        }

        #endregion
    }
}
