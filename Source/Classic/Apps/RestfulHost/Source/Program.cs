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

#endregion

namespace RestfulHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri uri = new Uri("http://localhost:1234");

            using (NancyHost host = new NancyHost(uri))
            {
                host.Start();
                Console.WriteLine("Running on {0}", uri);
                Console.ReadLine();
            }
        }
    }
}
