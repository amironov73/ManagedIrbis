// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StatusCommand.cs --
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
    /// Статус сервера.
    /// </summary>
    class StatusCommand
        : BotCommand
    {
        /// <inheritdoc />
        public override string Name => "status";

        /// <inheritdoc />
        public override string Description => "Статус сервера ИРБИС64";

        /// <inheritdoc />
        public override void Execute(Message message, TelegramBotClient client)
        {
            if (Filter(message, client))
            {
                return;
            }

            var chatId = message.Chat.Id;
            string text = "OK";
            client.SendTextMessageAsync(chatId, text).Wait();
        }
    }
}
