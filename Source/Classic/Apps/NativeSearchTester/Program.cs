using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.Text;

using IrbisInterop;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Search;
using ManagedIrbis.Server;

namespace NativeSearchTester
{
    class Program
    {
        private static Irbis64Dll irbis;
        private static string searchExpression;

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                return;
            }

            string databasePath = args[0];
            searchExpression = args[1];

            try
            {
                ServerConfiguration configuration
                    = ServerConfiguration.FromIniFile(databasePath);
                using (irbis = new Irbis64Dll(configuration))
                {
                    irbis.Layout = SpaceLayout.Version2014();
                    string systemPath = irbis.Configuration.SystemPath;
                    string mainIni = Path.GetFullPath
                        (
                            Path.Combine
                                (
                                    systemPath,
                                    "irbisc.ini"
                                )
                        );
                    Irbis65Dll.IrbisMainIniInit(mainIni);
                    irbis.UseDatabase("ibis");
                    Console.WriteLine("Max MFN={0}", irbis.GetMaxMfn());
                    string briefPft = irbis.GetPftPath("sbrief");
                    irbis.SetFormat("@" + briefPft);

                    int[] found = irbis.Search(searchExpression);
                    Console.WriteLine("Found: {0}", found.Length);
                    Console.WriteLine();
                    foreach (int mfn in found)
                    {
                        string formatted = irbis.FormatRecord(mfn);
                        Console.WriteLine
                            (
                                "{0}: {1}",
                                mfn,
                                formatted
                            );
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
