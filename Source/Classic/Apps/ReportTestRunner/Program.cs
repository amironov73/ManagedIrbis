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

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Client;
using ManagedIrbis.Reports;

using MoonSharp.Interpreter;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace ReportTestRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("ReportTestRunner <folder>");
                return;
            }

            try
            {
                string rootPath = CM.AppSettings["rootPath"];
                AbstractClient environment
                    = new LocalClient(rootPath);

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
