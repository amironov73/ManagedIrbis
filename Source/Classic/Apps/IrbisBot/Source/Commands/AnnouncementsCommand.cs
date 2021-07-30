// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AnnouncementsCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;

using Newtonsoft.Json;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

#endregion

namespace IrbisBot.Commands
{
    /// <summary>
    /// Анонсы мероприятий библиотеки.
    /// </summary>
    class AnnouncementsCommand
        : BotCommand
    {
        public override string Name => "announcements";
        public override string Alias => "Анонсы";

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
                if (ReferenceEquals(announces, null))
                {
                    SendMessage(client, chatId, "К сожалению, анонсов пока нет");
                }

                announces = announces.Where(a => a.Date.HasValue).OrderBy(a => a.Date.Value).ToArray();
                if (announces.Length == 0)
                {
                    SendMessage(client, chatId, "К сожалению, анонсов пока нет");
                }

                foreach (Announcement announce in announces)
                {
                    string msg = "<b>" + announce.Name + "</b>\n";

                    msg += announce.Text.Trim().Replace("&nbsp;", "") + "\n";

                    if (!string.IsNullOrEmpty(announce.Url))
                    {
                        msg = msg + "<a href=\"https://www.irklib.ru" + announce.Url + "\">Подробнее</a>";
                    }

                    if (!string.IsNullOrEmpty(announce.Picture))
                    {
                        InputOnlineFile photo = new InputOnlineFile("https://www.irklib.ru" + announce.Picture);
                        client.SendPhotoAsync
                            (
                                chatId,
                                photo,
                                msg,
                                ParseMode.Html,
                                replyMarkup:GetKeyboard()
                            )
                            .Wait();
                    }
                    else
                    {
                        SendMessage(client, chatId, msg);
                    }
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
            }
        }
    }
}
