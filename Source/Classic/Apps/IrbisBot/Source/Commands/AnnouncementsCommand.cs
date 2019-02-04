// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EchoCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

#endregion

namespace IrbisBot.Commands
{
    class AnnouncementsCommand
        : BotCommand
    {
        public override string Name => "announcements";

        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            try
            {
                HttpClient http = new HttpClient();
                string text = http.GetStringAsync("https://www.irklib.ru/api/").Result.Trim();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    DateParseHandling = DateParseHandling.DateTime,
                    DateFormatString = "dd.MM.yyyy HH:mm:ss"
                };

                Announcement[] announces = JsonConvert.DeserializeObject<Announcement[]>(text, settings);
                announces = announces.Where(a => a.Date.HasValue).OrderBy(a => a.Date.Value).ToArray();
                foreach (Announcement announce in announces)
                {
                    string all = // announce.Date.Value.ToString("f") + "\n" +
                        "<b>" + announce.Name + "</b>\n";

                    all += announce.Text.Trim().Replace("&nbsp;", "") + "\n";

                    if (!string.IsNullOrEmpty(announce.Url))
                    {
                        all = all + "<a href=\"https://www.irklib.ru" + announce.Url + "\">Подробнее</a>";
                    }

                    //all = all + "<b>" + announce.Name + "</b></br><br/>"
                    //      + announce.Text.Trim();

                    client.SendTextMessageAsync(chatId, all, ParseMode.Html).Wait();
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
            }
        }
    }
}
