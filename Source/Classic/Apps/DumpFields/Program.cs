using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ManagedIrbis;
using ManagedIrbis.Direct;

// ReSharper disable LocalizableElement
// ReSharper disable InconsistentNaming

using Counter= AM.Collections.DictionaryCounterInt32<string>;

namespace DumpFields
{
    class Program
    {
        private static readonly Counter _authors
            = new Counter(StringComparer.InvariantCultureIgnoreCase);
        private static readonly Counter _titles
            = new Counter(StringComparer.InvariantCultureIgnoreCase);
        private static readonly Counter _series
            = new Counter(StringComparer.InvariantCultureIgnoreCase);
        private static readonly Counter _publishers
            = new Counter(StringComparer.InvariantCultureIgnoreCase);

        private static DirectAccess64 _access;

        static void ProcessData(Counter counter, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                counter.Increment(value);
            }
        }

        static void ProcessData(Counter counter, string[] values)
        {
            foreach (string value in values)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    counter.Increment(value);
                }
            }
        }

        static void ProcessRecord(int mfn)
        {
            if ((mfn % 50) == 1)
                Console.Write($"\n{mfn-1:00000000}>");
            if ((mfn % 10) == 1)
                Console.Write(" ");

            MarcRecord record = _access.ReadRecord(mfn);
            if (ReferenceEquals(record, null))
            {
                Console.Write('-');
                return;
            }

            if (record.Deleted)
            {
                Console.Write('x');
                return;
            }

            string worksheet = record.FM(920);
            if (string.IsNullOrEmpty(worksheet))
            {
                Console.Write('~');
                return;
            }

            worksheet = worksheet.ToUpperInvariant();
            if (worksheet != "PAZK" && worksheet != "SPEC")
            {
                Console.Write('=');
                return;
            }

            ProcessData(_authors, record.FM(700, 'a'));
            ProcessData(_authors, record.FMA(701, 'a'));
            ProcessData(_titles, record.FM(200, 'a'));
            ProcessData(_titles, record.FMA(461, 'c'));
            ProcessData(_series, record.FM(225, 'a'));
            ProcessData(_series, record.FMA(46, 'a'));
            ProcessData(_publishers, record.FM(210, 'c'));
            ProcessData(_publishers, record.FMA(461, 'g'));

            Console.Write('*');
        }

        static void DumpCounter(Counter counter, string fileName)
        {
            Console.WriteLine(fileName);
            using (StreamWriter writer = File.CreateText(fileName))
            {
                string[] keys = counter.Keys.ToArray();
                Array.Sort(keys);
                foreach (string key in keys)
                    writer.WriteLine($"{key}\t{counter[key]}");
            }
        }

        static void Main(string[] args)
        {
            if (args.Length != 1)
                return;

            string masterName = args[0];
            DirectAccessMode mode = DirectAccessMode.ReadOnly;
            using (_access = new DirectAccess64(masterName, mode))
            {
                int maxMfn = _access.GetMaxMfn();
                Console.WriteLine(maxMfn);

                for (int mfn = 1; mfn < maxMfn; mfn++)
                {
                    try
                    {
                        ProcessRecord(mfn);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine($"MFN={mfn}: exception: {exception.Message}");
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            DumpCounter(_authors, "authors.txt");
            DumpCounter(_titles, "titles.txt");
            DumpCounter(_series, "series.txt");
            DumpCounter(_publishers, "publishers.txt");
        }
    }
}
