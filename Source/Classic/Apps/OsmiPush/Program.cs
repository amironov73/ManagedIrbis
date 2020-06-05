// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- notifies debtors about book prolongation.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Configuration;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Readers;

using Newtonsoft.Json.Linq;

using RestfulIrbis.OsmiCards;

using CM = System.Configuration.ConfigurationManager;

#endregion

/*

    Утилита отправляет задолжникам уведомление о необходимости
    продлить/сдать книги с помощью системы OsmiCards.

 */

// ReSharper disable LocalizableElement
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable HeapView.ObjectAllocation
// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable SuggestVarOrType_BuiltInTypes
// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_Elsewhere

namespace OsmiPush
{
    class Program
    {
        private static OsmiCardsClient client;
        private static IrbisConnection connection;
        private static DateTime deadline;

        static void Main()
        {
            Console.WriteLine(new string('=', 80));
            Console.WriteLine("Start: {0}", DateTime.Now);
            deadline = DateTime.Today.AddDays(-1.0);

            try
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
                string connectionString = IrbisConnectionUtility
                    .GetStandardConnectionString()
                    .ThrowIfNull("connectionString");
                string messageText = CM.AppSettings["message"];

                client = new OsmiCardsClient
                    (
                        baseUri,
                        apiID,
                        apiKey
                    );

                Console.WriteLine("PING:");
                JObject ping = client.Ping();
                Console.WriteLine(ping);

                string[] cards = client.GetCardList();
                Console.Write("CARDS: {0}", cards.Length);

                List<string> recipients = new List<string>();
                using (connection = new IrbisConnection(connectionString))
                {
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
                        ReaderInfo reader = ReaderInfo.Parse(record);
                        string ticket = (reader.PassCard ?? reader.Ticket)
                            .ThrowIfNull("ticket");
                        if (!ticket.OneOf(cards))
                        {
                            continue;
                        }

                        // Массив просроченных книг.
                        VisitInfo[] outdated = reader.Visits
                            .Where(v => !v.IsVisit)
                            .Where(v => !v.IsReturned)
                            .Where(v => v.DateExpected < deadline)
                            .ToArray();
                        if (outdated.Length != 0)
                        {
                            Console.WriteLine
                                (
                                    "[{0}] {1}: {2} book(s)",
                                    reader.Ticket,
                                    reader.FullName,
                                    outdated.Length
                                );
                            recipients.Add(reader.Ticket);
                        }
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Total recipient(s): {0}", recipients.Count);

                if (recipients.Count != 0)
                {
                    client.SendPushMessage
                      (
                          recipients.ToArray(),
                          messageText
                      );
                    Console.WriteLine("Send OK");
                }

                Console.WriteLine("Finish: {0}", DateTime.Now);
                Console.WriteLine(new string('=', 80));
                Console.WriteLine();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
