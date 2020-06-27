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

// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

namespace CatBoy.Commands
{
    /// <summary>
    /// Команда вызывается в самом начале диалога с ботом.
    /// </summary>
    class StartCommand
        : BotCommand
    {
        /// <inheritdoc />
        public override string Name => "start";

        /// <inheritdoc />
        public override string Description => "Команда вызывается в самом начале диалога с ботом";

        /// <inheritdoc />
        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            string text = "Я повелеваю сервером ИРБИС.\n"
                          + "Могу запустить, могу удалить. "
                          + char.ConvertFromUtf32(0x1F601) // Улыбающийся смайлик
                          + "\n\n"
                          + Bot.GetAllCommands();
            client.SendTextMessageAsync(chatId, text).Wait();
        }
    }
}
