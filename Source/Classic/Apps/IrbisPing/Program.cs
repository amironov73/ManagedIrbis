// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisPing.cs -- 
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

using ManagedIrbis;

#endregion

namespace IrbisPing
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("IrbisPing version 0.0.1");
                Console.WriteLine("USAGE: IrbisPing <connection-string>");
                Console.WriteLine();

                return 1;
            }

            try
            {
                using (IrbisConnection connection = new IrbisConnection())
                {
                    string connectionString = args[0];
                    connection.ParseConnectionString(connectionString);

                    connection.Connect();

                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return 1;
            }
        }
    }
}
