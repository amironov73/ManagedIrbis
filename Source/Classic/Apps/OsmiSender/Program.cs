// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- registers the reader in OsmiCards service
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

using AM;
using AM.Configuration;
using AM.Net;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Readers;

using Newtonsoft.Json.Linq;

using RestfulIrbis.OsmiCards;

using CM = System.Configuration.ConfigurationManager;

#endregion

// ReSharper disable InvertIf
// ReSharper disable LocalizableElement
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable HeapView.ObjectAllocation
// ReSharper disable HeapView.ObjectAllocation.Evident

namespace OsmiSender
{
    /*

     Утилита осуществляет привязку читателей, зарегистрированных
     через Госуслуги, в системе OsmiCards.

     1. Web-ИРБИС как-то там авторизует читателя через Госуслуги
        и создает запись.

     2. Вся ответственность за правильность заполнения лежит
        на Госуслугах.

     3. В записи есть поля 10,11,12, 30 и 32.

     4. Также есть поле 3340, которое содержит хеш СНИЛС.
        Это признак регистрации читателя через Госуслуги.

     5. Отдельный сценарий AutoIn.GBL для Web-ИРБИС посредством
        &unifor(‘+2...’) формирует команду вида
        OsmiSender.exe TICKET
        где TICKET - номер читательского билета.

     6. Программа OsmiSender ожидает 5 секунд, после чего производит
        поиск записи согласно переданному ей аргументу. Если запись
        не найдена, программа ждёт ещё 5 секунд, и снова производит
        поиск. При необходимости, попытки поиска повторяются не менее
        10 раз с интервалом ожидания 5 секунд. Если запись всё равно
        не находится, значит, на сервере сбой или перегрузка,
        программа формирует запись об ошибке в файле
        OsmiSender.log и аварийно завершается.

     7. Если запись найдена, то производится вызов API OsmiCards,
        формирующий новую запись в БД OsmiCards. Сервер OsmiCards
        должен отправить читателю письмо с ссылкой на скачивание приложение.

     8. Об успешном либо неуспешном вызове API OsmiCards формируется
        запись в файле OsmiSender.log.

     9. Чтобы быть успешно зарегистрированным в системе, читатель должен
        иметь валидный e-mail.

     10. Дважды зарегистрировать в OsmiCards одного и того же читателя нельзя.

     */

    internal static class Program
    {
        private static string connectionString;
        private static OsmiCardsClient client;
        private static IrbisConnection irbis;
        private static string ticket;
        private static ReaderInfo reader;
        private static string templateName;
        private static JObject template;

        private static void WriteLog
            (
                [NotNull] string format,
                params object[] args
            )
        {
            var fileName = Path.ChangeExtension
                (
                    RuntimeUtility.ExecutableFileName,
                    ".log"
                );

            using (var writer
                = new StreamWriter(fileName, true, Encoding.UTF8))
            {
                writer.Write("{0:yyyy-MM-dd HH:mm:ss}: ", DateTime.Now);
                writer.WriteLine(format, args);
            }
        }

        private static void Initialize()
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
        private static ReaderInfo FindReader()
        {
            var manager = new ReaderManager(irbis);
            var result = manager.GetReader(ticket);
            return result;
        }

        private static bool CardExists
            (
                [NotNull] string cardNumber
            )
        {
            var card = client.GetCardInfo(cardNumber);
            var result = card != null;

            Console.WriteLine
                (
                    "Card exist: {0}",
                    result.ToString(CultureInfo.InvariantCulture)
                );

            return result;
        }

        private static bool CheckReader()
        {
            if (string.IsNullOrEmpty(reader.FamilyName))
            {
                WriteLog("ERROR: ticket {0}: no family name", ticket);
                return false;
            }

            var email = reader.Email;

            if (string.IsNullOrEmpty(email))
            {
                WriteLog("ERROR: ticket {0}: no email", ticket);
                return false;
            }

            if (!MailUtility.VerifyEmail(email))
            {
                WriteLog("ERROR: ticket {0}: bad email {1}", ticket, email);
                return false;
            }

            return true;
        }

        private static JObject BuildCard()
        {
            var result = OsmiUtility.BuildCardForReader
                (
                    template,
                    reader
                );

            return result;
        }

        private static void CreateCard
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

        private static string CleanupEmail
            (
                [NotNull] string email
            )
        {
            return email.Replace(" ", string.Empty);
        }

        private static void SendEmail()
        {
            var emails = reader
                .Record.ThrowIfNull("reader.Record")
                .FMA(32);

            foreach (var email in emails)
            {
                var cleaned = CleanupEmail(email);
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

        private static int Main(string[] args)
        {
            var version = Assembly.GetEntryAssembly().GetName().Version;
            Console.WriteLine("OsmiSender from ArsMagna project, http://arsmagna.ru");
            Console.WriteLine("Written by Alexey Mironov in 2020");
            Console.WriteLine("Version {0}", version);
            Console.WriteLine();

            if (args.Length != 1)
            {
                Console.WriteLine("USAGE: OsmiSender <TICKET>");
                return 1;
            }

            try
            {
                ticket = args[0];
                WriteLog("START: {0}", ticket);
                Initialize();

                using (irbis = new IrbisConnection(connectionString))
                {
                    var delay = ConfigurationUtility.GetInt32("delay", 5000);
                    var retry = ConfigurationUtility.GetInt32("retry", 10);
                    for (var i = 0; i < retry; i++)
                    {
                        Thread.Sleep(delay);
                        reader = FindReader();
                        if (reader != null)
                        {
                            break;
                        }
                        Console.WriteLine("Ticket not found, try {0} of {1}", i + 1, retry);
                    }
                }

                if (reader == null)
                {
                    Console.WriteLine("Failed to find ticket {0}", ticket);
                    WriteLog("ERROR: Failed to find ticket {0}", ticket);
                    return 2;
                }

                if (CardExists(ticket))
                {
                    WriteLog("ERROR: Card {0} already exist", ticket);
                    return 3;
                }

                if (!CheckReader())
                {
                    return 4;
                }

                var card = BuildCard();
                CreateCard(ticket, card);
                SendEmail();

                Console.WriteLine("Ticket {0} sucessfully registered", ticket);
                WriteLog("SUCCESS: Registered ticket {0}", ticket);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex);
                WriteLog("EXCEPTION: {0}", ex);
                return 1;
            }

            return 0;
        }
    }
}
