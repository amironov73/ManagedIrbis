// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RestartServerCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
//using Telegram.Bot.Types.ReplyMarkups;

#endregion

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace CatBoy.Commands
{
    /// <summary>
    /// Перезапуск сервера ИРБИС.
    /// </summary>
    class RestartServerCommand
        : BotCommand
    {
        /// <inheritdoc />
        public override string Name => "adduser";

        /// <inheritdoc />
        public override string Description => "Добавление пользователя в систему";

        /// <inheritdoc />
        public override void Execute(Message message, TelegramBotClient client)
        {
            if (Filter(message, client))
            {
                return;
            }

            var chatId = message.Chat.Id;

            try
            {
                using (var irbis = GetConnection())
                {
                    client.SendTextMessageAsync(chatId, "Перезапускаю сервер").Wait();
                    irbis.RestartServer();
                    client.SendTextMessageAsync(chatId, $"Сервер успешно перезапущен").Wait();
                }
            }
            catch (Exception exception)
            {
                client.SendTextMessageAsync(chatId, exception.Message).Wait();
            }
        }
    }
}
