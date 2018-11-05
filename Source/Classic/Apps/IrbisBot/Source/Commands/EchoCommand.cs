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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

namespace IrbisBot.Commands
{
    class EchoCommand
        : BotCommand
    {
        public override string Name => "echo";

        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            char[] separators = {' '};
            string[] splitted = message.Text.Split(separators, 2);
            if (splitted.Length > 1)
            {
                string text = splitted[1];
                client.SendTextMessageAsync(chatId, text).Wait();
            }
        }
    }
}
