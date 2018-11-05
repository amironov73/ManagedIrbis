using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace IrbisBot.Commands
{
    class StartCommand
        : BotCommand
    {
        public override string Name => "start";

        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            client.SendTextMessageAsync(chatId, "Я сижу в подвале среди миллионов книг.\r\n"
                + "Могу найти книжку, могу не найти :)").Wait();
        }
    }
}
