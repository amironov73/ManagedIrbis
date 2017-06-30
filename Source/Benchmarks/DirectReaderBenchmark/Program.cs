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

namespace DirectReaderBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: DirectReaderBenchmark <path-to-MST>");

                return;
            }

            string masterPath = args[0];

            try
            {
                IrbisEncoding.RelaxUtf8();

                Console.WriteLine("Open: {0}", masterPath);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                using (DirectReader64 reader
                    = new DirectReader64(masterPath, true))
                {
                    int maxMfn = reader.GetMaxMfn();
                    Console.WriteLine("Max MFN={0}", maxMfn);

                    Parallel.For
                        (
                            1,
                            maxMfn,
                            mfn =>
                            {
                                try
                                {
                                    MarcRecord record = reader.ReadRecord(mfn);
                                    //Console.Write('.');
                                    if (!ReferenceEquals(record, null))
                                    {
                                        Debug.Assert(record.Mfn == mfn);
                                    }
                                }
                                catch (Exception exception)
                                {
                                    Console.WriteLine
                                        (
                                            "MFN={0}: exception: {1}",
                                            mfn,
                                            exception
                                        );
                                }
                            }
                        );
                }

                stopwatch.Stop();

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Close: {0}", masterPath);
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
