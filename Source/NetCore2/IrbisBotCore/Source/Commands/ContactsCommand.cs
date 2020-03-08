// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ContactsCommand.cs --
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
    /// Контакты библиотеки.
    /// </summary>
    class ContactsCommand
        : BotCommand
    {
        public override string Name => "contacts";
        public override string Alias => "Контакты";

        public override void Execute(Message message, TelegramBotClient client)
        {
            ChatId chatId = message.Chat.Id;
            string text = "Почтовый адрес: 664033, г. Иркутск, ул. Лермонтова, 253\n"
                + "Электронная почта: library@irklib.ru\n"
                + "Многоканальный телефон: (3952) 48-66-80\n"
                + "Добавочный номер приемной: 705\n\n"
                + "<a href=\"https://2gis.ru/irkutsk/firm/1548640653569394\">На карте</a>";


            SendMessage(client, chatId, text);
        }
    }
}
