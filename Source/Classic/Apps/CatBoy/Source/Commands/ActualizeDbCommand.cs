// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ActualizeDbCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM.Text;

using ManagedIrbis;

using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace CatBoy.Commands
{
    /// <summary>
    /// Актуализация базы данных.
    /// </summary>
    class ActualizeDbCommand
        : BotCommand
    {
        /// <inheritdoc />
        public override string Name => "actualizedb";

        /// <inheritdoc />
        public override string Description => "Актуализация базы данных";

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
                    client.SendTextMessageAsync(chatId, $"Начинаю актуализацию {dbname}").Wait();
                    irbis.ActualizeDatabase(dbname);
                    client.SendTextMessageAsync(chatId, $"Актуализация база {dbname} успешно завершена").Wait();
                }
            }
            catch (Exception exception)
            {
                client.SendTextMessageAsync(chatId, exception.Message).Wait();
            }
        }
    }
}
