// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Bot.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using AM;
using AM.Configuration;
using AM.Runtime;
using AM.Text;

using MihaZupan;

using Telegram.Bot;
using Telegram.Bot.Types.Enums;

using IrbisBot.Commands;

using MessageEventArgs = Telegram.Bot.Args.MessageEventArgs;

#endregion

// ReSharper disable LocalizableElement

namespace IrbisBot
{
    static class Bot
    {
        public static bool Paused { get; set; }
        public static string Token { get; set; }

        public static List<BotCommand> Commands;

        private static TelegramBotClient _client;

        private static TextWriter GetLogWriter()
        {
            string exeName = RuntimeUtility.ExecutableFileName;
            string logName = Path.ChangeExtension(exeName, ".log");
            StreamWriter result = new StreamWriter(logName, true);

            return result;
        }

        public static void WriteLog(string line)
        {
            if (ReferenceEquals(_client, null))
            {
                return;
            }

            lock (_client)
            {
                try
                {
                    using (TextWriter writer = GetLogWriter())
                    {
                        writer.Write("{0:yyyy-MM-dd HH:mm:ss} ", DateTime.Now);
                        writer.WriteLine(line);
                    }
                }
                catch (Exception exception)
                {
                    Trace.WriteLine(exception.Message);
                }
            }
        }

        public static TelegramBotClient GetClient()
        {
            if (!ReferenceEquals(_client, null))
            {
                return _client;
            }

            Commands = new List<BotCommand>
            {
                new StartCommand(),
                new HelpCommand(),
                new ContactsCommand(),
                new HoursCommand(),
                new AnnouncementsCommand(),
                new EchoCommand(),
                new SearchWebCommand()
            };

            Token = ConfigurationUtility.GetString("apiToken");
            string socksConfig = ConfigurationUtility.GetString("socksProxy");
            HttpToSocks5Proxy proxy = null;
            if (!string.IsNullOrEmpty(socksConfig))
            {
                string[] parts = socksConfig.Split(CommonSeparators.Colon);
                proxy = new HttpToSocks5Proxy
                    (
                        parts[0],
                        NumericUtility.ParseInt32(parts[1])
                    );
            }

            // TODO use httpProxy configuration string

            _client = proxy == null
                ? new TelegramBotClient(Token)
                : new TelegramBotClient(Token, proxy);

            _client.OnMessage += OnMessage;

            return _client;
        }

        public static void MessageLoop()
        {
            var me = _client.GetMeAsync().Result;
            Console.WriteLine("ME: {0}", me.Username);
            _client.StartReceiving(new UpdateType[0]);
            WriteLog("MESSAGE LOOP STARTED");
        }

        private static void OnMessage
            (
                object sender,
                MessageEventArgs e
            )
        {
            if (Paused)
            {
                return;
            }

            var message = e.Message;
            if (ReferenceEquals(message, null))
            {
                return;
            }

            if (message.Type != MessageType.Text)
            {
                return;
            }

            var text = message.Text;
            Console.WriteLine("Got command: {0}", text);
            WriteLog($"[{message.From.Username}] {message.Text}");
            foreach (var command in Commands)
            {
                if (command.Contains(text))
                {
                    command.Execute(message, GetClient());
                    break;
                }
            }
        }

    }
}
