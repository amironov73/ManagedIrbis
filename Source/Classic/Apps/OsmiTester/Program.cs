// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- 
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
using AM;
using Newtonsoft.Json.Linq;

using RestfulIrbis;
using RestfulIrbis.OsmiCards;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace OsmiTester
{
    class Program
    {
        static void Main()
        {
            try
            {
                string baseUri = CM.AppSettings["baseUri"];
                string apiID = CM.AppSettings["apiID"];
                string apiKey = CM.AppSettings["apiKey"];
                OsmiCardsClient client = new OsmiCardsClient
                    (
                        baseUri,
                        apiID,
                        apiKey
                    );

                Console.WriteLine("PING:");
                JObject ping = client.Ping();
                Console.WriteLine(ping);

                Console.WriteLine();
                Console.WriteLine("DEFAULTS:");
                JObject defaults = client.GetDefaults();
                Console.WriteLine(defaults);

                Console.WriteLine();
                Console.WriteLine("TEMPLATE LIST:");
                string[] templates = client.GetTemplateList();
                Console.WriteLine
                    (
                        StringUtility.Join
                        (
                            ", ",
                            templates
                        )
                    );

                Console.WriteLine();
                Console.WriteLine("TEMPLATE INFO:");
                JObject chb = client.GetTemplateInfo("chb");
                Console.WriteLine(chb);

                Console.WriteLine();
                Console.WriteLine("CARD LIST:");
                string[] cards = client.GetCardList();
                Console.WriteLine
                    (
                        StringUtility.Join
                        (
                            ", ",
                            cards
                        )
                    );

                Console.WriteLine();
                Console.WriteLine("CARD INFO:");
                OsmiCard card = client.GetCardInfo("4433AD69");
                Console.WriteLine(card);

                Console.WriteLine();
                Console.WriteLine("GET CARD LINK:");
                string link = client.GetCardLink("4433AD69");
                Console.WriteLine(link);

                Console.WriteLine();
                Console.WriteLine("GET IMAGES");
                OsmiImage[] images = client.GetImages();
                Console.WriteLine
                    (
                        StringUtility.Join
                            (
                                ", ",
                                images
                            )
                    );
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
