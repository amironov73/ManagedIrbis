// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BotCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

using ManagedIrbis;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
//using Telegram.Bot.Types.ReplyMarkups;

using CM=System.Configuration.ConfigurationManager;

#endregion

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace CatBoy.Commands
{
    /// <summary>
    /// Абстрактная команда бота.
    /// </summary>
    abstract class BotCommand
    {
        /// <summary>
        /// Официальное имя команды (англоязычное).
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Описание команды.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Русскоязычный алиас
        /// </summary>
        public virtual string Alias => string.Empty;

        public abstract void Execute(Message message, TelegramBotClient client);

        public static bool IsAllowed(Message message, TelegramBotClient client)
        {
            var username = message.From.Username;
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            var allowed = CM.AppSettings["allowed"].Split(',');
            foreach (var one in allowed)
            {
                if (string.CompareOrdinal(one, username) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        protected bool Filter(Message message, TelegramBotClient client)
        {
            if (!IsAllowed(message, client))
            {
                var chatId = message.Chat.Id;
                client.SendTextMessageAsync(chatId, "[[SUCCESS]]");
                return true;
            }

            return false;
        }

        public IrbisConnection GetConnection()
        {
            var connectionString = CM.AppSettings["irbis"];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ApplicationException("Не задана строка подключения");
            }

            return new IrbisConnection(connectionString);
        }

        public string GetCommandText(Message message)
        {
            char[] separators = {' '};
            var splitted = message.Text.Split(separators, 2);
            if (splitted.Length > 1)
            {
                return splitted[1].Trim();
            }

            return string.Empty;
        }

        ///// <summary>
        ///// Экранная клавиатура с командами.
        ///// </summary>
        //public virtual ReplyKeyboardMarkup GetKeyboard()
        //{
        //    List<List<KeyboardButton>> buttons = new List<List<KeyboardButton>>()
        //    {
        //        new List<KeyboardButton>
        //        {
        //            new KeyboardButton() { Text = "Анонсы" },
        //            new KeyboardButton() { Text = "Контакты" }
        //        },
        //        new List<KeyboardButton>
        //        {
        //            new KeyboardButton() { Text = "Режим работы" },
        //            new KeyboardButton() { Text = "Помощь" }
        //        }

        //    };
        //    ReplyKeyboardMarkup result
        //        = new ReplyKeyboardMarkup(buttons, true);

        //    return result;
        //}

        public virtual void SendMessage
            (
                TelegramBotClient client,
                ChatId chatId,
                string text
            )
        {
            client.SendTextMessageAsync
                (
                    chatId,
                    text,
                    ParseMode.Html
                    //replyMarkup: GetKeyboard()
                )
                .Wait();
        }

        public virtual bool Contains(string command)
        {
            return command.StartsWith("/" + Name)
                || (!string.IsNullOrEmpty(Alias)
                    && Alias.SameString(command));
        }
    }
}
