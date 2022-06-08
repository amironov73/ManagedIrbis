// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HoursCommand.cs -- режим работы библиотеки
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Linq;

using HtmlAgilityPack;

using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

namespace IrbisBot.Commands
{
    /// <summary>
    /// Режим работы библиотеки.
    /// </summary>
    class HoursCommand
        : BotCommand
    {
        public override string Name => "hours";
        public override string Alias => "Режим";

        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            const string defaultText = "<b>Режим работы:</b>\n\n"
                + "ВТ-ВС 11.00-20.00 (до 22.00 в режиме читального зала)\n"
                + "ПН - выходной,\n"
                + "последняя пятница месяца - санитарный день";

            var text = defaultText;

            try
            {
                // пробуем взять режим с сайта библиотеки
                const string url = "https://irklib.ru/";
                var web = new HtmlWeb();
                var doc = web.Load(url);
                var root = doc.DocumentNode;
                var regime = root
                    .Descendants("div")
                    .FirstOrDefault(node => node.Id == "mode");
                if (!ReferenceEquals(regime, null))
                {
                    text = regime.InnerHtml;
                    text = text.Replace("<br>", "\n");
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
            }

            var position = text.IndexOf("<div", StringComparison.Ordinal);
            if (position >= 0)
            {
                text = text.Substring(0, position);
            }

            SendMessage(client, chatId, text);
        }
    }
}
