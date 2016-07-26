/* Program.cs --
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using ManagedIrbis;
using ManagedIrbis.Network;

#endregion

namespace AnyCmd
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("AnyCmd <connection-string> " +
                                  "<command-code> [command-args]");

                return 1;
            }

            try
            {
                string connectionString = args[0];

                using (IrbisConnection connection
                    = new IrbisConnection(connectionString))
                {
                    string commandCode = args[1];
                    object[] commandArguments
                        = args.GetSpan(2, args.Length - 2)
                        .Cast<object>()
                        .ToArray();

                    ServerResponse response
                        = connection.ExecuteArbitraryCommand
                        (
                            commandCode,
                            commandArguments
                        );

                    File.WriteAllBytes
                        (
                            "anycmd.txt",
                            response.RawAnswer
                        );

                    List<string> lines = response.RemainingAnsiStrings();
                    foreach (string line in lines)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }

            return 0;
        }
    }
}
