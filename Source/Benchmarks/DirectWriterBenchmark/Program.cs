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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using ManagedIrbis;
using ManagedIrbis.Direct;

#endregion

namespace DirectWriterBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: DirectWriterBenchmark <path-to-MST>");

                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string masterPath = args[0];
            try
            {
                DirectUtility.CreateDatabase64(masterPath);

                using (DirectAccess64 accessor = new DirectAccess64(masterPath))
                {
                    for (int i = 0; i < 10000; i++)
                    {
                        if (i % 1000 == 0)
                        {
                            Console.Write(".");
                        }

                        MarcRecord record = new MarcRecord();
                        record.BeginUpdate(100);
                        for (int tag = 200; tag < 300; tag++)
                        {
                            record.Fields.Add
                                (
                                    new RecordField
                                        (
                                            tag,
                                            "Это поле номер " + tag
                                        )
                                );
                        }
                        record.EndUpdate();
                        accessor.WriteRecord(record);
                    }

                    Console.WriteLine();

                    for (int approach = 0; approach < 10; approach++)
                    {
                        Console.WriteLine("Approach {0}", approach + 1);
                        for (int mfn = 1; mfn <= 1000; mfn++)
                        {
                            MarcRecord record = accessor.ReadRecord(mfn)
                                .ThrowIfNull("accessor.ReadRecord(mfn)");
                            record.Fields.Add
                                (
                                    new RecordField
                                    (
                                        300,
                                        "Запись отредактирована " + DateTime.Now
                                    )
                                );
                            accessor.WriteRecord(record);
                        }
                    }
                }

                stopwatch.Stop();
                Console.WriteLine
                    (
                        "Elapsed: {0}",
                        stopwatch.Elapsed
                    );
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
