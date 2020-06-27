// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EmptyDbCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM.Text;

using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace CatBoy.Commands
{
    /// <summary>
    /// Опустошение базы данных.
    /// </summary>
    class EmptyDbCommand
        : BotCommand
    {
        /// <inheritdoc />
        public override string Name => "emptydb";

        /// <inheritdoc />
        public override string Description => "Опустошение базы данных";

        /// <inheritdoc />
        public override void Execute(Message message, TelegramBotClient client)
        {
            if (Filter(message, client))
            {
                return;
            }

            var chatId = message.Chat.Id;
            var text = GetCommandText(message);

            var navigator = new TextNavigator(text);
            var dbname = navigator.ReadWord();
            if (string.IsNullOrEmpty(dbname))
            {
                client.SendTextMessageAsync(chatId, "Не задано имя базы данных").Wait();
                return;
            }

            try
            {
                using (var irbis = GetConnection())
                {
                    client.SendTextMessageAsync(chatId, $"Опустошаю базу данных {dbname}").Wait();
                    irbis.DeleteDatabase(dbname);
                    client.SendTextMessageAsync(chatId, $"База данных {dbname} опустошена").Wait();
                }
            }
            catch (Exception exception)
            {
                client.SendTextMessageAsync(chatId, exception.Message).Wait();
            }
        }
    }
}
