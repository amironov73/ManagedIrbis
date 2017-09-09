// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- program entry point
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Linq;

using ManagedIrbis;
using ManagedIrbis.Readers;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace CutTail
{
    class Program
    {
        private static DateTime Threshold;
        private static string TailDb;

        private static string FormatSpan(TimeSpan span)
        {
            return string.Format
                (
                    "{0:00}:{1:00}:{2:00}",
                    span.Hours,
                    span.Minutes,
                    span.Seconds
                );
        }

        static void Main()
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Threshold = new DateTime(DateTime.Today.Year, 1, 1);

                string thresholdString = CM.AppSettings["threshold"];

                if (!string.IsNullOrEmpty(thresholdString))
                {
                    Threshold = DateTime.Parse(thresholdString);
                }

                TailDb = CM.AppSettings["tail-db"];

                int total = 0;
                int cropped = 0;

                string connectionString = CM.AppSettings["connection-string"];
                using (IrbisConnection connection
                    = new IrbisConnection(connectionString))
                {
                    int[] mfns = connection.Search
                        (
                            "RI=$"
                        );

                    const int delta = 1000;

                    for (int i = 0; i < mfns.Length; i += delta)
                    {
                        int[] sub = mfns
                            .Skip(i)
                            .Take(delta)
                            .ToArray();

                        MarcRecord[] records = connection.ReadRecords
                            (
                                connection.Database,
                                sub
                            );

                        foreach (MarcRecord record in records)
                        {
                            if (record.Deleted)
                            {
                                continue;
                            }

                            total++;

                            Console.WriteLine("ELAPSED: {0}", FormatSpan(stopwatch.Elapsed));
                            ReaderInfo reader = ReaderInfo.Parse(record);
                            Console.WriteLine(reader);

                            VisitInfo[] visits = record
                                .Fields
                                .GetField(40)
                                .Select(VisitInfo.Parse)
                                .Where(loan => loan.IsVisit
                                        || loan.IsReturned)
                                .ToArray();

                            VisitInfo[] old = visits
                                .Where(visit => visit.DateGiven < Threshold)
                                .ToArray();

                            Console.WriteLine("VISITS TOTAL:     {0}", visits.Length);
                            Console.WriteLine("VISITS TO DELETE: {0}", old.Length);

                            if (old.Length != 0)
                            {
                                if (!string.IsNullOrEmpty(TailDb))
                                {
                                    connection.PushDatabase(TailDb);
                                    int[] tailMfns = new int[0];
                                    try
                                    {
                                        tailMfns = connection.Search("\"RI={0}\"", reader.Ticket);
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine(ex);
                                    }
                                    MarcRecord copy;
                                    if (tailMfns.Length != 0)
                                    {
                                        copy = connection.ReadRecord(tailMfns[0]);
                                        Console.WriteLine("USING OLD RECORD {0}", tailMfns[0]);
                                    }
                                    else
                                    {
                                        copy = new MarcRecord();
                                        RecordField[] non40 = record
                                            .Fields
                                            .Where(field => field.Tag != 40)
                                            .ToArray();
                                        copy.Fields.AddRange(non40);
                                        Console.WriteLine("COPY CREATED");
                                    }

                                    RecordField[] old40 = old.Select(loan => loan.Field).ToArray();
                                    copy.Fields.AddRange(old40);

                                    try
                                    {
                                        connection.WriteRecord(copy, false, true);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("CAN'T WRITE COPY, SKIP");
                                        Debug.WriteLine(ex);
                                        continue;
                                    }
                                    finally
                                    {
                                        connection.PopDatabase();
                                    }

                                    Console.WriteLine("COPY WRITTEN");

                                }

                                MarcRecord r2;
                                try
                                {
                                    r2 = connection.ReadRecord(record.Mfn);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("CAN'T REREAD RECORD, SKIP");
                                    Debug.WriteLine(ex);
                                    continue;
                                }

                                RecordField[] toDelete = r2
                                    .Fields
                                    .GetField(40)
                                    .Select(VisitInfo.Parse)
                                    .Where(loan => loan.IsVisit || loan.IsReturned)
                                    .Where(visit => visit.DateGiven < Threshold)
                                    .Select(visit => visit.Field)
                                    .ToArray();

                                foreach (RecordField field in toDelete)
                                {
                                    r2.Fields.Remove(field);
                                }

                                try
                                {
                                    connection.WriteRecord(r2, false, true);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("CAN'T WRITE MODIFIED RECORD, SKIP");
                                    Debug.WriteLine(ex);
                                    continue;
                                }

                                Console.WriteLine("RECORD WRITTEN");

                                cropped++;
                            }

                            Console.WriteLine(new string('=', 60));
                        }
                    }
                }

                stopwatch.Stop();

                Console.WriteLine("TOTAL={0}", total);
                Console.WriteLine("CROPPED={0}", cropped);
                Console.WriteLine("ELAPSED={0}", FormatSpan(stopwatch.Elapsed));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
