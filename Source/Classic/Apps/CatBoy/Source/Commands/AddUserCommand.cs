// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AddUserCommand.cs --
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

#endregion

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace CatBoy.Commands
{
    /// <summary>
    /// Добавление пользователя в систему.
    /// </summary>
    class AddUserCommand
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
            var text = GetCommandText(message);

            var navigator = new TextNavigator(text);
            var username = navigator.ReadWord();
            if (string.IsNullOrEmpty(username))
            {
                client.SendTextMessageAsync(chatId, "Не задано имя пользователя");
                return;
            }

            navigator.SkipWhitespace();
            var password = navigator.ReadWord();
            if (string.IsNullOrEmpty(password))
            {
                client.SendTextMessageAsync(chatId, "Не задан пароль");
                return;
            }

            try
            {
                using (var irbis = GetConnection())
                {
                    client.SendTextMessageAsync(chatId, "Получаю список пользователей").Wait();
                    var users = irbis.ListUsers().ToList();
                    if (users.FirstOrDefault(user => user.Name.SameString(username)) != null)
                    {
                        client.SendTextMessageAsync(chatId, $"Пользователь {username} уже существует").Wait();
                        return;
                    }

                    var userInfo = new UserInfo
                    {
                        Name = username,
                        Password = password,
                        Reader = "irbisr.ini",
                        Cataloger = "irbisc.ini",
                        Circulation = "irbisb.ini"
                    };
                    users.Add(userInfo);

                    client.SendTextMessageAsync(chatId, "Сохраняю список пользователей").Wait();
                    irbis.UpdateUserList(users.ToArray());
                    client.SendTextMessageAsync(chatId, $"Успешно создан пользователь {username}").Wait();
                }
            }
            catch (Exception exception)
            {
                client.SendTextMessageAsync(chatId, exception.Message).Wait();
            }
        }
    }
}
