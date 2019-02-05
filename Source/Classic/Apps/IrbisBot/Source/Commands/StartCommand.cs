// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StartCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

namespace IrbisBot.Commands
{
    /// <summary>
    /// Команда вызывается в самом начале диалога с ботом.
    /// </summary>
    class StartCommand
        : BotCommand
    {
        public override string Name => "start";

        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            string text = "Я сижу в подвале среди миллионов книг.\n"
                        + "Могу найти книжку, могу не найти "
                        + char.ConvertFromUtf32(0x1F601); // Улыбающийся смайлик
            SendMessage(client, chatId, text);
        }
    }
}
