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
using System.Globalization;
using System.IO;
using System.Linq;

using AM;
using AM.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Direct;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Readers;

#endregion

// ReSharper disable LocalizableElement
// ReSharper disable InconsistentNaming
// ReSharper disable InlineOutVariableDeclaration

namespace Depersonizer
{
    class Program
    {
        private static readonly Dictionary<string, DepersonalizedReader> readers
            = new Dictionary<string, DepersonalizedReader>();

        private static void ProcessOneDatabase
            (
                [NotNull] string inputFileName
            )
        {
            using (DirectAccess64 accessor
                = new DirectAccess64(inputFileName, DirectAccessMode.ReadOnly))
            {
                int maxMfn = accessor.GetMaxMfn();
                Console.WriteLine("{0}: Max MFN={1}", inputFileName, maxMfn);

                for (int mfn = 1; mfn < maxMfn; mfn++)
                {
                    if (mfn % 100 == 1)
                    {
                        Console.Write(" {0} ", mfn - 1);
                    }

                    try
                    {
                        MarcRecord record = accessor.ReadRecord(mfn);
                        if (!ReferenceEquals(record, null))
                        {
                            ReaderInfo readerInfo = ReaderInfo.Parse(record);
                            string ticket = readerInfo.Ticket;
                            if (string.IsNullOrEmpty(ticket))
                            {
                                continue;
                            }

                            DepersonalizedReader depersonalized;
                            if (readers.TryGetValue(ticket, out depersonalized))
                            {
                                depersonalized.AddVisits(readerInfo.Visits);
                            }
                            else
                            {
                                depersonalized = DepersonalizedReader
                                    .FromReaderInfo(readerInfo);
                                readers.Add(ticket, depersonalized);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine();
                        Console.WriteLine("MFN {0}: {1}", mfn, exception.Message);
                    }
                }
            }

            Console.WriteLine("Total: {0}", readers.Count);
            Console.WriteLine("Memory: {0}", Process.GetCurrentProcess().WorkingSet64);
        }

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Need two arguments");
                return;
            }

            string inputFileName = args[0];
            string outputFileName = args[1];

            try
            {
                // Сначала прочитаем уже имеющиеся записи
                if (File.Exists(outputFileName))
                {
                    using (StreamReader stringReader = File.OpenText(outputFileName))
                    {
                        MarcRecord marcRecord;
                        while ((marcRecord = PlainText.ReadRecord(stringReader)) != null)
                        {
                            ReaderInfo readerInfo = ReaderInfo.Parse(marcRecord);
                            DepersonalizedReader depersonalized
                                = DepersonalizedReader.FromReaderInfo(readerInfo);
                            string ticket = depersonalized.Ticket.ThrowIfNull();
                            readers.Add(ticket, depersonalized);
                        }
                    }
                }

                // Теперь будем вычитывать данные и объединять их с уже имеющимися
                string ext = Path.GetExtension(inputFileName);
                if (ext.SameString(".mst"))
                {
                    ProcessOneDatabase(inputFileName);
                }
                else
                {
                    string[] inputFiles = File.ReadAllLines(inputFileName);
                    foreach (string inputFile in inputFiles)
                    {
                        ProcessOneDatabase(inputFile);
                    }
                }

                // Сохраняем результат
                Console.WriteLine("Writing ");
                using (StreamWriter writer = File.CreateText(outputFileName))
                {
                    var allReaders = readers.Values;
                    foreach (DepersonalizedReader reader in allReaders)
                    {
                        MarcRecord record = reader.ToMarcRecord();
                        PlainText.WriteRecord(writer, record);
                    }
                }

                Console.WriteLine("DONE");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
