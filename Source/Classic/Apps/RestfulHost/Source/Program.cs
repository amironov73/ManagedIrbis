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

using Nancy;
using Nancy.Hosting.Self;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace RestfulHost
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string publishUri = CM.AppSettings["publishUri"];
                Uri uri = new Uri(publishUri);

                using (NancyHost host = new NancyHost(uri))
                {
                    host.Start();
                    Console.WriteLine("Running on {0}", uri);
                    Console.WriteLine("Press ENTER to stop");
                    Console.ReadLine();
                    host.Stop();
                    Console.WriteLine("Stopped");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
