using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Collections;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Direct;
using ManagedIrbis.Fields;

namespace CountPublishers
{
    class Program
    {
        private static readonly char[] charsToTrim = { '"', '[', ']' };

        static readonly IrbisProvider provider = new NullProvider();

        static readonly DictionaryCounterInt32<string> counter1
            = new DictionaryCounterInt32<string>(StringComparer.InvariantCultureIgnoreCase);
        static readonly DictionaryCounterInt32<string> counter2
            = new DictionaryCounterInt32<string>(StringComparer.InvariantCultureIgnoreCase);

        static void ProcessRecord
            (
                MarcRecord record
            )
        {
            BookInfo info = new BookInfo(provider, record);
            string worksheet = info.Worksheet;
            if (worksheet != "SPEC" && worksheet != "PAZK")
            {
                return;
            }

            string[] publishers = info.Publishers;
            int count = info.UsageCount;
            int exemplars = info.Exemplars.Length;

            if (count != 0)
            {
                foreach (string publisher in publishers)
                {
                    string trimmed = publisher.Trim(charsToTrim);
                    counter1.Augment(trimmed, count);
                    counter2.Augment(trimmed, exemplars);
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }

            try
            {
                DirectAccess64 irbis = new DirectAccess64(args[0]);
                int maxMfn = irbis.GetMaxMfn();
                Console.WriteLine("Max MFN={0}", maxMfn);

                Parallel.For(1, maxMfn, mfn =>
                {
                    if (mfn % 1000 == 1)
                    {
                        Console.Write(".");
                    }

                    try
                    {
                        MarcRecord record = irbis.ReadRecord(mfn);
                        if (!ReferenceEquals(record, null))
                        {
                            ProcessRecord(record);
                        }
                    }
                    catch
                    {
                        // Do nothing
                    }
                });
                irbis.Dispose();

                using (StreamWriter writer = File.CreateText("publishers.txt"))
                {
                    string[] keys = counter1.Keys.OrderBy(_ => _).ToArray();
                    foreach (string key in keys)
                    {
                        writer.WriteLine("{0}\t{1}\t{2}", key, counter1[key],
                            counter2[key]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
