// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchCommand.cs --
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

using Telegram.Bot;
using Telegram.Bot.Types;

using AM.Configuration;

using ManagedIrbis;

#endregion

namespace IrbisBot.Commands
{
    class SearchCommand
        : BotCommand
    {
        public override string Name => "time";

        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            string query = message.Text.Trim();
            string answer = $"Ищу книги и статьи по теме '{query}'";
            client.SendTextMessageAsync(chatId, answer).Wait();

            try
            {
                string connectionString = ConfigurationUtility.GetString("irbis");
                using (IrbisConnection connection = new IrbisConnection())
                {
                    connection.ParseConnectionString(connectionString);
                    connection.Connect();
                    int[] found = connection.Search("\"K={0}\"", query);
                    client.SendTextMessageAsync(chatId, $"Найдено: {found.Length}").Wait();
                    if (found.Length > 5)
                    {
                        client.SendTextMessageAsync(chatId, "Покажу только первые пять").Wait();
                    }
                    found = found.Reverse().Take(5).ToArray();
                    foreach (int mfn in found)
                    {
                        string description = connection.FormatRecord("@brief", mfn);
                        client.SendTextMessageAsync(chatId, description).Wait();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public override bool Contains(string command)
        {
            return true;
        }
    }
}
