// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HelpCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

namespace CatBoy.Commands
{
    /// <summary>
    /// Выдаёт список доступных команд.
    /// </summary>
    class HelpCommand
        : BotCommand
    {
        /// <inheritdoc />
        public override string Name => "help";

        /// <inheritdoc />
        public override string Description => "Выводит список доступных команд";

        /// <inheritdoc />
        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var text = Bot.GetAllCommands();
            client.SendTextMessageAsync(chatId, text).Wait();
        }
    }
}
