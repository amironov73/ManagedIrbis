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

using AM.Logging;

using ManagedIrbis.Isis;

#endregion

namespace Isis32Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Log.SetLogger(new ConsoleLogger());

                Console.WriteLine
                (
                    "ISIS32.DLL version: {0}",
                    Isis32Dll.Version
                );

                using (Isis32Dll isis = new Isis32Dll())
                {
                    
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
