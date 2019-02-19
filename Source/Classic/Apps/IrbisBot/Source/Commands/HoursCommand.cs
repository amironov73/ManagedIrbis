// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HoursCommand.cs --
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
    /// Режим работы библиотеки.
    /// </summary>
    class HoursCommand
        : BotCommand
    {
        public override string Name => "hours";
        public override string Alias => "Режим работы";

        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            string text = "<b>Режим работы:</b>\n\n"
                + "ВТ-ВС 11.00-20.00 (до 22.00 в режиме читального зала)\n"
                + "ПН - выходной,\n"
                + "последняя пятница месяца - санитарный день";


            SendMessage(client, chatId, text);
        }
    }
}
