using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM.Collections;
using ManagedIrbis;
using ManagedIrbis.Batch;

namespace PopularBooks
{
    class Program
    {
        const string ConnectionString = "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;";

        private static IrbisConnection Connection;

        private static List<Pair<int,int>> list = new List<Pair<int, int>>();

        private static string EnhanceText
            (
                string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            string result = text
                .Replace(" (Введено оглавление)", string.Empty)
                .Replace(" [Текст]", string.Empty)
                .Replace("[Текст]", string.Empty)
                .Replace("  ", " ")
                .Trim();

            return result;
        }

        static void ProcessRecord
            (
                MarcRecord record
            )
        {
            string countText = record.FM("999");
            if (!string.IsNullOrEmpty(countText))
            {
                int count;
                if (int.TryParse(countText, out count))
                {
                    list.Add(new Pair<int, int>(record.Mfn,count));
                }
            }
        }

        static void Main(string[] args)
        {
           string connectionString = ConnectionString;
           if (args.Length != 0)
           {
               connectionString = args[0];
           }

            try
            {
                //IrbisEncoding.RelaxUtf8();
                using (Connection = new IrbisConnection())
                {
                    Connection.ParseConnectionString(connectionString);
                    Connection.Connect();

                    BatchRecordReader batch
                        = (BatchRecordReader) BatchRecordReader.Search
                        (
                            Connection,
                            Connection.Database,
                            //@"V=KN * G=201$",
                            @"V=KN",
                            1000
                        );
                    batch.BatchRead += (sender, eventArgs) =>
                    {
                        Console.Write(".");
                    };

                    //BatchRecordReader batch = records as BatchRecordReader;
                    //if (!ReferenceEquals(batch, null))
                    //{
                    //    Console.WriteLine("Found: {0}", batch.TotalRecords);
                    //    batch.BatchRead += (sender, args) 
                    //        => Console.WriteLine(batch.RecordsRead);
                    //}

                    foreach (MarcRecord record in batch)
                    {
                        ProcessRecord(record);
                    }

                    Console.WriteLine();

                    Pair<int, int>[] top = list.OrderByDescending
                        (
                            pair => pair.Second
                        )
                        .Take(100)
                        .ToArray();

                    foreach (Pair<int, int> pair in top)
                    {
                        string description = EnhanceText
                            (
                                Connection.FormatRecord
                                (
                                    "@sbrief", 
                                    pair.First
                                )
                            );

                        Console.WriteLine("{0}\t{1}", description, pair.Second);
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
