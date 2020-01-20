using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AM;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Client;
using ManagedIrbis.Direct;
using ManagedIrbis.Pft;

// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

namespace BookPopularity
{
    class Info
    {
        public int Mfn;
        public int Year;
        public string Index;
        public DateTime Date;
        public int Reduced;
        //public string Title;
        public int Count;
    }

    class Program
    {
        private const string mstPath = @"D:\IRBIS64_2015\Datai\IBIS\ibis.MST";
        private static IrbisConnection _connection;
        private static DirectAccess64 _direct;
        private static LocalProvider _provider;
        private static List<Info> _infos = new List<Info>();

        static void _OnReceive(BatchRecordReader reader)
        {
            Console.Write ("{0} ", reader.RecordsRead);
        }

        private static DateTime _GetYear(MarcRecord record)
        {
            var years = record.FMA(907, 'a').ToArray();
            Array.Sort(years);
            if (years.Length == 0)
            {
                return DateTime.MinValue;
            }

            return IrbisDate.SafeParse(years[0])?.Date
                   ?? DateTime.MinValue;

            //Match match = Regex.Match(years[0], @"\d{4}");
            //if (match.Success)
            //{
            //    return FastNumber.ParseInt32(match.Value);
            //}

            //return 0;

            //string result = record.FM(210, 'd');
            //if (string.IsNullOrEmpty(result))
            //{
            //    result = record.FM(461, 'h');
            //}
            //if (string.IsNullOrEmpty(result))
            //{
            //    result = record.FM(461, 'z');
            //}
            //if (string.IsNullOrEmpty(result))
            //{
            //    result = record.FM(934);
            //}
            //if (string.IsNullOrEmpty(result))
            //{
            //    return 0;
            //}

            //Match match = Regex.Match(result, @"\d{4}");
            //if (match.Success)
            //{
            //    result = match.Value;
            //}
            //return result.SafeToInt32();
        }

        static void ProcessRecord(MarcRecord record)
        {
            var worksheet = record.FM(920);
            if (!worksheet.SameString("PAZK")
                && !worksheet.SameString("SPEC"))
            {
                return;
            }

            var index = record.FM(903);
            if (string.IsNullOrEmpty(index))
            {
                return;
            }

            var count = FastNumber.ParseInt32(record.FM(999) ?? string.Empty);
            if (count == 0)
            {
                return;
            }

            var year = _GetYear(record);
            var years = (DateTime.Today - year).Days / 365.0;
            if (years > 30.0)
            {
                return;
            }
            var reduced = (int)Math.Truncate(count/years);

            Info info = new Info
            {
                Mfn = record.Mfn,
                Index = index,
                Date = year,
                Reduced = reduced,
                Count = count
            };
            _infos.Add(info);
        }

        static void GatherFromConnection()
        {
            const string connectionString = "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;";

            using (_connection = new IrbisConnection(connectionString))
            {
                _infos.Capacity = _connection.GetMaxMfn(_connection.Database);

                var records = BatchRecordReader.WholeDatabase
                (
                    _connection,
                    _connection.Database,
                    5000,
                    true,
                    _OnReceive
                );
                Parallel.ForEach(records, ProcessRecord);
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        static void GatherFromDirect()
        {
            using (_direct = new DirectAccess64(mstPath, DirectAccessMode.ReadOnly))
            {
                var maxMfn = _direct.GetMaxMfn();
                for (var mfn = 1; mfn < maxMfn; mfn++)
                {
                    if ((mfn % 1000) == 1)
                    {
                        Console.Write("{0} ", mfn-1);
                    }

                    try
                    {
                        var record = _direct.ReadRecord(mfn);
                        ProcessRecord(record);
                    }
                    catch (Exception)
                    {
                        Console.Write('!');
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        static void SummarizeFromConnection()
        {
            _infos = _infos.OrderByDescending(_ => _.Count).ToList();
            foreach (var info in _infos)
            {
                Console.WriteLine("{0}\t{1}", info.Mfn, info.Count);
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        static void SummarizeFromDirect()
        {
            _infos = _infos.OrderByDescending(_ => _.Reduced).Take(500).ToList();
            var formatter = new PftFormatter();
            var brief = File.ReadAllText("sbrief.pft", IrbisEncoding.Ansi);
            var rootPath = @"D:\IRBIS64_2015";
            using (_provider = new LocalProvider(rootPath))
            {
                _provider.Database = "IBIS";
                formatter.SetProvider(_provider);
                formatter.ParseProgram(brief);
                foreach (var info in _infos)
                {
                    try
                    {
                        var title = formatter.FormatRecord(info.Mfn);
                        Console.WriteLine("{0}\t{1}\t{2}\t{3}", info.Mfn, info.Count, info.Reduced, title);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("!!! {0}", exception.Message);
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                //GatherFromConnection();
                GatherFromDirect();
                SummarizeFromDirect();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed;
            Console.WriteLine("Elapsed: {0}", elapsed);
        }
    }
}
