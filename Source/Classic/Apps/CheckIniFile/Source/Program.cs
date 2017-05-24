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

using AM.IO;

using ManagedIrbis;
using ManagedIrbis.Client;

#endregion

namespace CheckIniFile
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: CheckIniFile <fileName>");
                return;
            }

            try
            {
                string fileName = args[0];
                Encoding encoding = IrbisEncoding.Ansi;

                using (IniFile iniFile
                    = new IniFile(fileName, encoding, false))
                {
                    ClientLM lm = new ClientLM();
                    bool result = lm.CheckHash(iniFile);

                    Console.WriteLine("{0}: {1}", fileName, result);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
