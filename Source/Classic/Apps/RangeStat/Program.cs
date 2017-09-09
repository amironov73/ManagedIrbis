using System;
using System.IO;

using AM;
using AM.Text;

using IrbisInterop;

using ManagedIrbis;
using ManagedIrbis.Server;

namespace RangeStat
{
    class Program
    {
        private static Irbis64Dll irbis;
        private static NumberText fromNumber;
        private static NumberText toNumber;

        static void ProcessNumber
            (
                string number
            )
        {
            int[] found = irbis.ExactSearch(string.Format
                (
                    "IN={0}",
                    number
                ));
            if (found.Length == 0)
            {
                return;
            }
            int mfn = found[0];
            irbis.ReadRecord(mfn);
            NativeRecord native = irbis.GetRecord();
            MarcRecord record = native.ToMarcRecord();
            string description = irbis.FormatRecord(mfn);
            int count = record.FM(999).SafeToInt32();
            Console.WriteLine
                (
                    "{0}\t{1}\t{2}",
                    number,
                    count,
                    description
                );
        }

        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                return;
            }

            string databasePath = args[0];
            fromNumber = args[1];
            toNumber = args[2];

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

                    string savedDirectory = Directory.GetCurrentDirectory();
                    Directory.SetCurrentDirectory(systemPath);
                    try
                    {
                        irbis.UseDatabase("ibis");

                        string briefPft = irbis.GetPftPath("sbrief");
                        irbis.SetFormat("@" + briefPft);

                        NumberText currentNumber = fromNumber;
                        while (currentNumber < toNumber)
                        {
                            ProcessNumber(currentNumber.ToString());
                            currentNumber = currentNumber.Increment();
                        }
                    }
                    finally
                    {
                        Directory.SetCurrentDirectory(savedDirectory);
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
