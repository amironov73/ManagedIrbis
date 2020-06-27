// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DeleteUserCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Linq;

using AM;
using AM.Text;

using ManagedIrbis;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
//using Telegram.Bot.Types.ReplyMarkups;

#endregion

// ReSharper disable CommentTypo

namespace CatBoy.Commands
{
    /// <summary>
    /// Удаление пользователя из системы.
    /// </summary>
    class DeleteUserCommand
        : BotCommand
    {
        /// <inheritdoc />
        public override string Name => "deleteuser";

        /// <inheritdoc />
        public override string Description => "Удаление пользователя из системы";

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
            var username = navigator.ReadWord();
            if (string.IsNullOrEmpty(username))
            {
                client.SendTextMessageAsync(chatId, "Не задано имя пользователя");
                return;
            }

            try
            {
                using (var irbis = GetConnection())
                {
                    client.SendTextMessageAsync(chatId, "Получаю список пользователей").Wait();
                    var users = irbis.ListUsers().ToList();
                    if (users.FirstOrDefault(user => user.Name.SameString(username)) == null)
                    {
                        client.SendTextMessageAsync(chatId, $"Пользователь {username} не существует").Wait();
                        return;
                    }

                    users = users.Where(user => !user.Name.SameString(username)).ToList();

                    client.SendTextMessageAsync(chatId, "Сохраняю список пользователей").Wait();
                    irbis.UpdateUserList(users.ToArray());
                    client.SendTextMessageAsync(chatId, $"Пользователь {username} удалён").Wait();
                }
            }
            catch (Exception exception)
            {
                client.SendTextMessageAsync(chatId, exception.Message).Wait();
            }
        }
    }
}
