// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchWebCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Net.Http;
using System.Text.RegularExpressions;

using Telegram.Bot;
using Telegram.Bot.Types;

using Telegram.Bot.Types.Enums;

#endregion

// ReSharper disable StringLiteralTypo

namespace IrbisBot.Commands
{
    /// <summary>
    /// Поиск с обращением к Web-ИРБИС.
    /// </summary>
    class SearchWebCommand
        : BotCommand
    {
        public override string Name => "search_web";

        private static string _template1
            = "https://i.irklib.ru/cgi/irbis64r_61/cgiirbis_64.exe?"
            + "C21COM=S"
            + "&I21DBN=FORMAT"
            + "&P21DBN=FORMAT"
            + "&S21ALL=(<.>K={0}$<.>+%2B+<.>A={0}$<.>+%2B+<.>T={0}$<.>)*G=$"
            + "&S21FMT=BRIEFWEBRF"
            + "&S21SRW=GOD"
            + "&S21SRD=DOWN"
            + "&S21CNR=5";

        private static string _template2
            = "https://i.irklib.ru/cgi/irbis64r_61/cgiirbis_64.exe?"
            + "C21COM=S"
            + "&I21DBN=IBIS"
            + "&P21DBN=IBIS"
            + "&S21ALL=(%22K={0}$%22+%2B+%22A={0}$%22+%2B+%22T={0}$%22)*G=$"
            + "&S21FMT=FULLWEBR"
            + "&S21SRW=GOD"
            + "&S21SRD=DOWN";

        private static readonly string[] _goodTags = { "b", "/b" };
        private static readonly string[] _splits = { "<br>", "<br/>" };

        private static string _Evaluator(Match match)
        {
            string tag = match.Groups[1].Value.ToLowerInvariant();
            if (Array.IndexOf(_goodTags, tag) < 0)
            {
                return string.Empty;
            }

            return match.Value;
        }

        private static string _CleanText(string text)
        {
            Regex regex1 = new Regex("<([A-Za-z/]+).*?>");
            Regex regex2 = new Regex("<[Aa][^>]*?>[^<]*?</[Aa]>");

            string result = regex2.Replace(text, string.Empty);
            result = regex1.Replace(result, _Evaluator);
            result = result.Replace("()", string.Empty);

            return result;
        }

        public override void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            string query = message.Text.Trim();
            string answer = $"Ищу книги и статьи по запросу '{query}'";
            client.SendTextMessageAsync(chatId, answer).Wait();

            try
            {
                string request = string.Format(_template1, query);
                bool foundAnything = false;
                HttpClient http = new HttpClient();
                string text = http.GetStringAsync(request).Result.Trim();
                string[] lines = text.Split(_splits, StringSplitOptions.None);
                foreach (string line in lines)
                {
                    string clean = _CleanText(line).Trim();
                    if (!string.IsNullOrEmpty(clean))
                    {
                        foundAnything = true;
                        client.SendTextMessageAsync(chatId, clean, ParseMode.Html).Wait();
                    }
                }

                if (!foundAnything)
                {
                    string sorry = "К сожалению, ничего найти не удалось";
                    client.SendTextMessageAsync(chatId, sorry).Wait();
                }
                else
                {
                    string encoded = Uri.EscapeUriString(query);
                    string seeAll = string.Format(_template2, encoded);
                    seeAll = string.Format("См. <a href=\"{0}\">все найденные</a>", seeAll);
                    client.SendTextMessageAsync(chatId, seeAll, ParseMode.Html).Wait();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public override bool Contains(string command)
        {
            return true;
        }
    }
}
