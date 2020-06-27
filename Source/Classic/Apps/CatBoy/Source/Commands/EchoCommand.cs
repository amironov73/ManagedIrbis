// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EchoCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace CatBoy.Commands
{
    /// <summary>
    /// Эхо-команда (для тестирования бота).
    /// </summary>
    class EchoCommand
        : BotCommand
    {
        /// <inheritdoc />
        public override string Name => "echo";

        /// <inheritdoc />
        public override string Description => "Эхо-команда (для тестирования бота)";

        /// <inheritdoc />
        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var text = GetCommandText(message);
            if (!string.IsNullOrEmpty(text))
            {
                client.SendTextMessageAsync(chatId, text).Wait();
            }
        }
    }
}
