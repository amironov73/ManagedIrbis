// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PingCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using AM;

using ManagedIrbis;

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
    /// Проверка связи с сервером.
    /// </summary>
    class PingCommand
        : BotCommand
    {
        /// <inheritdoc />
        public override string Name => "ping";

        /// <inheritdoc />
        public override string Description => "Проверка связи с сервером";

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
                    var stopwatch = new Stopwatch();
                    irbis.NoOp();
                    var text = "Ping " + stopwatch.Elapsed.TotalMilliseconds + " ms";
                    client.SendTextMessageAsync(chatId, text).Wait();
                }
            }
            catch (Exception exception)
            {
                client.SendTextMessageAsync(chatId, exception.ToString());
            }
        }
    }
}
