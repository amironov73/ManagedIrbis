// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HelpCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

namespace IrbisBot.Commands
{
    /// <summary>
    /// Помощь по работе с ботом.
    /// </summary>
    class HelpCommand
        : BotCommand
    {
        public override string Name => "help";
        public override string Alias => "Помощь";

        public override void Execute(Message message, TelegramBotClient client)
        {
            ChatId chatId = message.Chat.Id;
            string text = "Бот показывает анонсы мероприятий библиотеки, её контакты и режим работы.\n"
                + "Кроме того, бот ищет книги или статьи в электронном каталоге. "
                + "Для поиска введите ключевое слово (например, <i>черемша</i>), "
                + "заглавие книги (например, <i>Голодные игры</i>) "
                + "или фамилию автора (например, <i>Акунин</i>)\n"
                + "Для расширения поиска используйте усечение окончаний слов "
                + "(<i>черемша</i> \x2192 <i>черемш</i>).";
            SendMessage(client, chatId, text);
        }
    }
}
