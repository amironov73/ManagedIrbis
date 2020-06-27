// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AddDatabaseCommand.cs --
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
    /// Создание базы данных ЭК.
    /// </summary>
    class AddDatabaseCommand
        : BotCommand
    {
        /// <inheritdoc />
        public override string Name => "createdb";

        /// <inheritdoc />
        public override string Description => "Создание базы данных";

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
                    navigator.SkipWhitespace();
                    var description = navigator.GetRemainingText();
                    client.SendTextMessageAsync(chatId, $"Создаю базу данных {dbname}").Wait();
                    irbis.CreateDatabase(dbname, description, true, null);
                    client.SendTextMessageAsync(chatId, $"Успешно создана база {dbname}").Wait();
                }
            }
            catch (Exception exception)
            {
                client.SendTextMessageAsync(chatId, exception.Message).Wait();
            }
        }
    }
}
