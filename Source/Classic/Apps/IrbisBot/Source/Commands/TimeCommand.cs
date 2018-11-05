using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace IrbisBot.Commands
{
    class TimeCommand
        : BotCommand
    {
        public override string Name => "time";

        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var russian = CultureInfo.GetCultureInfo("ru-RU");
            string text = "В нашем подземелье " + DateTime.Now.ToString("t", russian)
                + ". Все ушли домой";
            client.SendTextMessageAsync(chatId, text).Wait();
        }
    }
}
