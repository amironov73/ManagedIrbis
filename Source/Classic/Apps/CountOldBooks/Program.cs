using System;
using System.Linq;
using System.Text.RegularExpressions;

using AM;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Fields;

// ReSharper disable LocalizableElement

namespace CountOldBooks
{
    internal class Program
    {
        private static string Fond = "Ф201";

        private static string ConnectionString
            = "host=192.168.3.2;port=6666;user=miron;password=miron;db=IBIS;";

        private static readonly int[] Years = new int[2200];
        private static int AbsentBooks = 0, BadYear = 0;

        private static int _GetYear
            (
                MarcRecord record
            )
        {
            var result = record.FM(210, 'd');
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM(461, 'h');
            }
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM(461, 'z');
            }
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM(934);
            }
            if (string.IsNullOrEmpty(result))
            {
                return 0;
            }

            var match = Regex.Match(result, @"\d{4}");
            if (match.Success)
            {
                result = match.Value;
            }
            return result.SafeToInt32();
        }

        static void ProcessRecord
            (
                MarcRecord record
            )
        {
            var exemplars = ExemplarInfo.Parse(record)
                .Where(ex => ex.Place.SameString(Fond))
                //.Where(ex => ex.Barcode.SafeStarts("E0"))
                .ToArray();
            if (exemplars.Length == 0)
            {
                //Console.Write('-');
                AbsentBooks++;
                return;
            }

            var year = _GetYear(record);
            if (year < 1700 || year > 2020)
            {
                //Console.Write('?');
                BadYear += exemplars.Length;
                return;
            }

            Years[year] += exemplars.Length;
            //Console.Write(exemplars.Length.ToString().LastChar());
        }

        public static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                Fond = args[0];
            }

            try
            {
                using (var connection = new IrbisConnection(ConnectionString))
                {
                    var batch = BatchRecordReader.Search
                        (
                            connection,
                            connection.Database,
                            "MHR=" + Fond,
                            500
                        );

                    var current = 1;
                    foreach (var record in batch)
                    {
                        //if (current % 50 == 1)
                        //{
                        //    Console.WriteLine();
                        //    Console.Write("{0:0000000}: ", current-1);
                        //}

                        //if (current % 5 == 1)
                        //{
                        //    Console.Write(' ');
                        //}

                        current++;

                        ProcessRecord(record);
                    }
                }

                //Console.WriteLine("Absent: {0}", AbsentBooks);
                //Console.WriteLine("Bad year: {0}", BadYear);
                for (var year = 1800; year < 2020; year++)
                {
                    //Console.WriteLine("{0};{1}", year, Years[year]);
                    Console.WriteLine("{0}", Years[year]);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}