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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;

#endregion

namespace IrbisBot.Commands
{
    abstract class BotCommand
    {
        public abstract string Name { get; }

        public abstract void Execute(Message message, TelegramBotClient client);

        public virtual bool Contains(string command)
        {
            return command.StartsWith("/" + Name);
        }
    }
}
