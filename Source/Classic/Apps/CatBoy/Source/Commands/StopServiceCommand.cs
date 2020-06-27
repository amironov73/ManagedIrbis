// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StopServiceCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ServiceProcess;

using AM;

using Telegram.Bot;
using Telegram.Bot.Types;

using CM=System.Configuration.ConfigurationManager;

#endregion

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace CatBoy.Commands
{
    /// <summary>
    /// Остановка сервиса ИРБИС64.
    /// </summary>
    class StopServiceCommand
        : BotCommand
    {
        /// <inheritdoc />
        public override string Name => "stop";

        /// <inheritdoc />
        public override string Description => "Остановка сервиса ИРБИС64";

        /// <inheritdoc />
        public override void Execute(Message message, TelegramBotClient client)
        {
            if (Filter(message, client))
            {
                return;
            }

            var chatId = message.Chat.Id;
            try
            {
                var serviceName = CM.AppSettings["service"].ThrowIfNull("serviceName");
                var machineName = CM.AppSettings["machine"].ThrowIfNull("machineName");
                client.SendTextMessageAsync(chatId, $"Пытаюсь остановить сервис {serviceName} на {machineName}").Wait();
                ServiceController controller
                    = new ServiceController(serviceName, machineName);
                controller.Stop();
                client.SendTextMessageAsync(chatId, $"Сервис {serviceName} на {machineName} успешно остановлен").Wait();
            }
            catch (Exception exception)
            {
                client.SendTextMessageAsync(chatId, exception.Message).Wait();
            }
        }
    }
}
