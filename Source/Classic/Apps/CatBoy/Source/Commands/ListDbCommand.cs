// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ListDbCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
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
    /// Список баз данных.
    /// </summary>
    class ListDbCommand
        : BotCommand
    {
        /// <inheritdoc />
        public override string Name => "databases";

        /// <inheritdoc />
        public override string Description => "Список баз данных";

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
                    var databases = irbis.ListDatabases();
                    var list = $"Всего: {databases.Length}\n"
                        + StringUtility.Join("\n", databases);
                    client.SendTextMessageAsync(chatId, list).Wait();
                }
            }
            catch (Exception exception)
            {
                client.SendTextMessageAsync(chatId, exception.ToString());
            }
        }
    }
}
