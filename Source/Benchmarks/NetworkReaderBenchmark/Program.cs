// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* .cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using ManagedIrbis;
using ManagedIrbis.Batch;

#endregion

namespace NetworkReaderBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: NetworkReaderBenchmark <path-to-MST>");

                return;
            }

            string connectionString = args[0];

            try
            {
                IrbisEncoding.RelaxUtf8();

                Console.WriteLine("Open");

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                using (IrbisConnection connection
                    = new IrbisConnection(connectionString))
                {
                    int maxMfn = connection.GetMaxMfn();
                    Console.WriteLine("Max MFN={0}", maxMfn);

                    IEnumerable<MarcRecord> batch
                        = BatchRecordReader.WholeDatabase
                        (
                            connection,
                            connection.Database,
                            1000,
                            reader => { Console.Write('.'); }
                        );
                    foreach (MarcRecord record in batch)
                    {
                        if (record.Modified)
                        {
                            Console.WriteLine("Very strange!");
                        }
                    }
                }

                stopwatch.Stop();

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Close");
                Console.WriteLine
                    (
                        "Elapsed: {0} sec",
                        stopwatch.Elapsed.ToSecondString()
                    );
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
