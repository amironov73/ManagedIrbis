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
using ManagedIrbis.Server;

namespace MagazineRating
{
    class Program
    {
        private static Dictionary<string, string> magazines;
        private static DictionaryCounterInt32<string> counter;
        private static Irbis64Dll irbis;
        private static string filter;

        static void ProcessRecord
            (
                int mfn
            )
        {
            irbis.ReadRecord(mfn);
            NativeRecord record = irbis.GetRecord();
            NativeField v933 = record.GetFirstField(933);
            if (ReferenceEquals(v933, null))
            {
                return;
            }

            string index = v933.Value;
            string description;
            if (!magazines.ContainsKey(index))
            {
                int[] found = irbis.ExactSearch("I=" + index);
                if (found.Length == 0)
                {
                    return;
                }
                description = irbis.FormatRecord(found[0]);
                magazines.Add(index, description);
            }
            description = magazines[index];

            NativeField v999 = record.GetFirstField(999);
            int count = ReferenceEquals(v999, null)
                ? 0
                : v999.Value.SafeToInt32();

            Console.WriteLine
                (
                    "{0}: {1}",
                    irbis.FormatRecord(mfn),
                    count
                );

            counter.Augment(description, count);
        }

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                return;
            }

            magazines = new CaseInsensitiveDictionary<string>();
            counter = new DictionaryCounterInt32<string>();
            string databasePath = args[0];
            filter = args[1];

            try
            {
                ServerConfiguration configuration
                    = ServerConfiguration.FromIniFile(databasePath);
                using (irbis = new Irbis64Dll(configuration))
                {
                    irbis.Layout = SpaceLayout.Version2014();
                    irbis.SetStandardIniFile("irbisc.ini");
                    int[] found = null;
                    irbis.UseDatabase("ibis");
                    string briefPft = irbis.GetPftPath("sbrief");
                    irbis.SetFormat("@" + briefPft);

                    found = irbis.Search(filter);
                    if (found.Length == 0)
                    {
                        Console.WriteLine("Noting found");
                        return;
                    }
                    Console.WriteLine("Found: {0}", found.Length);

                    foreach (int mfn in found)
                    {
                        ProcessRecord(mfn);
                    }

                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Всего выдач: {0}", counter.Total);
                    Console.WriteLine();

                    KeyValuePair<string, int>[] pairs = counter
                        .OrderByDescending(pair => pair.Value)
                        .ToArray();

                    foreach (KeyValuePair<string, int> pair in pairs)
                    {
                        Console.WriteLine
                            (
                                "{0}\t{1}",
                                pair.Key,
                                pair.Value
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
