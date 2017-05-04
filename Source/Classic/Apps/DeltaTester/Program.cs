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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM.IO;
using AM.Runtime;
using ManagedIrbis;
using ManagedIrbis.Client;

#endregion

namespace DeltaTester
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }

            try
            {
                string connectionString = args[0];
                using (IrbisConnection connection
                    = new IrbisConnection(connectionString))
                {
                    ConnectedClient provider
                        = new ConnectedClient(connection);

                    CatalogState first;
                    if (File.Exists("first.state"))
                    {
                        first = SerializationUtility
                            .RestoreObjectFromFile<CatalogState>
                            (
                                "first.state"
                            );
                        Console.WriteLine("Previous state restored from first.state");
                    }
                    else
                    {
                        first = provider.GetCatalogState("ISTU");
                        first.SaveToFile("first.state");
                        Console.WriteLine("Initial state saved to first.state");

                        return;
                    }

                    CatalogState second = provider.GetCatalogState("ISTU");
                    second.SaveToFile("second.state");
                    Console.WriteLine("New state saved to second.state");

                    CatalogDelta delta = CatalogDelta.Create
                        (
                            first,
                            second
                        );
                    delta.SaveToFile("state.delta");
                    Console.WriteLine("Difference saved to state.delta");
                    Console.WriteLine(delta);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
