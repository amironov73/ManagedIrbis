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

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

#endregion

namespace IrbisBot.Commands
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
        /// Русскоязычный алиас
        /// </summary>
        public virtual string Alias => string.Empty;

        public abstract void Execute(Message message, TelegramBotClient client);

        /// <summary>
        /// Экранная клавиатура с командами.
        /// </summary>
        public virtual ReplyKeyboardMarkup GetKeyboard()
        {
            List<List<KeyboardButton>> buttons = new List<List<KeyboardButton>>()
            {
                new List<KeyboardButton>
                {
                    new KeyboardButton() { Text = "Анонсы" },
                    new KeyboardButton() { Text = "Контакты" }
                },
                new List<KeyboardButton>
                {
                    new KeyboardButton() { Text = "Режим работы" },
                    new KeyboardButton() { Text = "Помощь" }
                }

            };
            ReplyKeyboardMarkup result
                = new ReplyKeyboardMarkup(buttons, true);

            return result;
        }

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
                    ParseMode.Html,
                    replyMarkup: GetKeyboard()
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
