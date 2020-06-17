#region Using directives

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;

using AM;
using AM.Configuration;
using AM.Net;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Readers;

using Newtonsoft.Json.Linq;

using RestfulIrbis.OsmiCards;

using CM = System.Configuration.ConfigurationManager;

#endregion

/*

    Сервис отправляет письмо со ссылкой на скачивание приложения
    OSMI Cards для переданного в аргументах читателя.

    1. Для возможности саморегистрации читатель должен иметь валидный
      (работающий) адрес электронной почты, прописанный в поле v32 базы RDR.

    2. Допускается наличие двух и более повторений поля v32. Письмо со ссылкой
       будет послано на все указанные адреса.

    3. В личном кабинете читателя добавляется ссылка вида
       https://i.irklib.ru/osmi/samoreg.aspx?ticket=TICKET&name=FIO,
       где TICKET - содержимое поля v30 (идентификатор читателя),
       а FIO - фамилия, имя, отчество читателя в формате v10, “ “v11, “ “v12.
       Возможно, вызов samoreg.aspx следует делать по методу POST,
       чтобы не засорять адресную строку.

    4. Сервис саморегистрации проверяет соответствие регистрационных данных
       и в случае успеха инициирует средствами API OsmiCards отправку письма
       с ссылкой на скачивание приложения.

    5. Повторное нажатие читателем на ту же ссылку приводит к повторной отправке
       письма со ссылкой, это является для читателя штатным способом снова
       установить приложение, если он его удалил по каким-либо причинам.

    6. При удачной отправке письма сервис выдаёт страницу со следующим текстом:
       “Уважаемый читатель! На адрес reader@mail.com отправлено письмо со ссылкой
       на скачивание приложения Osmi Cards.”

    7. При возникновении любого сбоя выдаётся страница с текстом “Уважаемый читатель!
       Возникла ошибка _________. Обратитесь в Отдел по работе с читателями.”

 */

// ReSharper disable InvertIf
// ReSharper disable LocalizableElement
// ReSharper disable HeapView.ObjectAllocation.Evident

namespace OsmiSamo
{
    public partial class SamoReg
        : Page
    {
        private string connectionString;
        private OsmiCardsClient client;
        private IrbisConnection irbis;
        private string ticket;
        private string fio;
        private ReaderInfo reader;
        private string errorMessage;
        private string templateName;
        private JObject template;

        private void WriteLog
            (
                [NotNull] string format,
                params object[] args
            )
        {
            var fileName = Server.MapPath("~/SamoReg.log");

            using (var writer = File.AppendText (fileName))
            {
                writer.Write("{0:yyyy-MM-dd HH:mm:ss}: ", DateTime.Now);
                writer.WriteLine(format, args);
            }
        }

        private void Initialize()
        {
            // Этим паролем может быть зашифрованы чувствительные данные
            const string password = "irbis";

            var baseUri = ConfigurationUtility
                .GetString("baseUri", null, password)
                .ThrowIfNull("baseUri");
            var apiID = ConfigurationUtility
                .GetString("apiID", null, password)
                .ThrowIfNull("apiID");
            var apiKey = ConfigurationUtility
                .GetString("apiKey", null, password)
                .ThrowIfNull("apiKey");
            connectionString = IrbisConnectionUtility
                .GetStandardConnectionString()
                .ThrowIfNull("connectionString");

            client = new OsmiCardsClient
                (
                    baseUri,
                    apiID,
                    apiKey
                );

            templateName = CM.AppSettings["template"];
            template = client.GetTemplateInfo(templateName);
        }

        [CanBeNull]
        private ReaderInfo FindReader()
        {
            var manager = new ReaderManager(irbis);
            var result = manager.GetReader(ticket);
            return result;
        }

        private bool CardExists
            (
                [NotNull] string cardNumber
            )
        {
            var card = client.GetCardInfo(cardNumber);
            var result = card != null;
            return result;
        }

        private bool CheckReader()
        {
            if (string.IsNullOrEmpty(reader.FamilyName))
            {
                errorMessage = "Неверно заполнены данные о ФИО";
                return false;
            }

            var email = reader.Email;
            if (string.IsNullOrEmpty(email))
            {
                errorMessage = "Отсутствует email в базе данных";
                return false;
            }

            if (!MailUtility.VerifyEmail(email))
            {
                errorMessage = "Неверный email в базе данных";
                return false;
            }

            return true;
        }

        private JObject BuildCard()
        {
            var result = OsmiUtility.BuildCardForReader
                (
                    template,
                    reader
                );

            return result;
        }

        private void CreateCard
            (
                [NotNull] string cardNumber,
                [NotNull] JObject card
            )
        {
            Code.NotNullNorEmpty(cardNumber, "cardNumber");
            Code.NotNull(card, "card");

            client.CreateCard
                (
                    cardNumber,
                    templateName,
                    card.ToString()
                );

            Console.WriteLine("Card {0} created", cardNumber);
        }

        private void SendEmail()
        {
            var emails = reader
                .Record.ThrowIfNull("reader.Record")
                .FMA(32);

            foreach (var email in emails)
            {
                var cleaned = MailUtility.CleanupEmail(email);
                if (!string.IsNullOrEmpty(cleaned))
                {
                    client.SendCardMail
                        (
                            ticket,
                            cleaned
                        );
                }

                Console.WriteLine("Send letter to {0}", email);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ticket = Request.Params.Get("ticket");
                fio = Request.Params.Get("fio");
                WriteLog("START {0} <{1}>", fio, ticket);

                if (string.IsNullOrEmpty(ticket))
                {
                    ShowErrorMessage("Не задан номер читательского билета");
                    return;
                }

                if (string.IsNullOrEmpty (fio))
                {
                    ShowErrorMessage("Не задано ФИО читателя");
                    return;
                }

                Initialize();

                using (irbis = new IrbisConnection(connectionString))
                {
                    reader = FindReader();
                }

                if (reader == null)
                {
                    ShowErrorMessage("Не удалось найти читателя с указанным билетом");
                    return;
                }

                var rightTicket = reader.PassCard ?? reader.Ticket;
                if (!ticket.SameString (rightTicket))
                {
                    ShowErrorMessage("Номер билета в БД не совпадает с указанным");
                    return;
                }

                if (!reader.FullName.SameString (fio))
                {
                    ShowErrorMessage("Заданы неверные учетные данные");
                    return;
                }

                ticket = rightTicket;

                if (!CheckReader())
                {
                    ShowErrorMessage(errorMessage);
                    return;
                }

                if (!CardExists(ticket))
                {
                    var card = BuildCard();
                    CreateCard(ticket, card);
                }

                SendEmail();

                ShowSuccessMessage(reader.Email);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        private void ShowSuccessMessage(string mail)
        {
            WriteLog("SUCCESS: " + mail);
            var fileName = Server.MapPath("~/Success.html");
            var contents = File.ReadAllText(fileName);
            contents = Regex.Replace(contents, "!!!.*!!!", mail);
            Response.Write(contents);
        }

        private void ShowErrorMessage(string message)
        {
            WriteLog("ERROR: " + message);
            var fileName = Server.MapPath("~/Error.html");
            var contents = File.ReadAllText(fileName);
            contents = Regex.Replace(contents, "!!!.*!!!", message);
            Response.Write(contents);
        }

    }
}
