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
// ReSharper disable InlineOutVariableDeclaration

namespace Depersonizer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Need two arguments");
                return;
            }

            string inputFileName = args[0];
            string outputFileName = args[1];

            Dictionary<string,DepersonalizedReader> readers
                = new Dictionary<string, DepersonalizedReader>();

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
                using (DirectAccess64 accessor
                    = new DirectAccess64(inputFileName, DirectAccessMode.ReadOnly))
                {
                    int maxMfn = accessor.GetMaxMfn();
                    Console.WriteLine("Max MFN: {0}", maxMfn);

                    for (int mfn = 1; mfn < maxMfn; mfn++)
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
                }

                // Сохраняем результат
                using (StreamWriter writer = File.CreateText(outputFileName))
                {
                    var allReaders = readers.Values;
                    foreach (DepersonalizedReader reader in allReaders)
                    {
                        MarcRecord record = reader.ToMarcRecord();
                        PlainText.WriteRecord(writer, record);
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
