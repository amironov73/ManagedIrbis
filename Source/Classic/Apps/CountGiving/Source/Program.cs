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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Direct;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Readers;
using ManagedIrbis.Search;

#endregion

// ReSharper disable LocalizableElement
// ReSharper disable InconsistentNaming
// ReSharper disable UseStringInterpolation
// ReSharper disable InlineOutVariableDeclaration

namespace CountGiving
{
    class Program
    {
        private static DirectAccess64 accessor;
        private static int maxMfn;
        //private static ActionBlock<MarcRecord> processBlock;
        private static PftFormatter formatter;
        private static IrbisStopWords stopwords;
        private static readonly Regex regex = new Regex(@"[A-Za-zА-Яа-я][A-Za-zА-Яа-я-]+[A-Za-zА-Яа-я]");
        private static BinaryWriter writer;
        private static int lastId = 0;
        private static readonly Dictionary<string, int> dictionary
            = new Dictionary<string, int>();
        private static int goodRecords = 0;
        private static int longest = 0;

        static void ReadRecord
            (
                int mfn
            )
        {
            if (mfn % 5000 == 0)
            {
                Console.WriteLine("Read: {0}", mfn);
            }

            MarcRecord record = accessor.ReadRecord(mfn);
            if (ReferenceEquals(record, null))
            {
                return;
            }

            // processBlock.Post(record);
            ProcessRecord(record);
        }

        static void ProcessRecord
            (
                [NotNull] MarcRecord record
            )
        {
            string worklist = record.FM(920);
            if (worklist != "PAZK" && worklist != "SPEC")
            {
                return;
            }

            int count = record.FM(999).SafeToInt32();

            string formatted = formatter.FormatRecord(record);
            List<int> words = new List<int>();
            foreach (Match match in regex.Matches(formatted))
            {
                string word = match.Value.ToUpperInvariant();
                if (word.Length >= 3 && !stopwords.IsStopWord(word))
                {
                    int id;
                    if (!dictionary.TryGetValue(word, out id))
                    {
                        id = ++lastId;
                        dictionary.Add(word, id);
                    }

                    if (!words.Contains(id))
                    {
                        words.Add(id);
                    }
                }
            }

            if (words.Count != 0)
            {
                goodRecords++;
                if (words.Count > longest)
                {
                    longest = words.Count;
                }
                BookData data = new BookData
                {
                    Count = count,
                    Mfn = record.Mfn,
                    Words = words.ToArray()
                };
                data.SaveToStream(writer);
            }
        }

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Need 1 argument");
                return;
            }

            string inputFileName = args[0];

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                stopwords = IrbisStopWords.ParseFile("IBIS.STW");

                string source = File.ReadAllText("words.pft");
                formatter = new PftFormatter()
                {
                    Program = PftUtility.CompileProgram(source)
                };

                //DataflowLinkOptions linkOptions = new DataflowLinkOptions
                //{
                //    PropagateCompletion = true
                //};
                //ExecutionDataflowBlockOptions executionOptions
                //    = new ExecutionDataflowBlockOptions
                //{
                //    MaxDegreeOfParallelism = 4
                //};
                //processBlock = new ActionBlock<MarcRecord>
                //    (
                //        (Action<MarcRecord>)ProcessRecord,
                //        executionOptions
                //    );

                using (FileStream stream = File.Create("words.bin"))
                using (writer = new BinaryWriter(stream))
                using (accessor = new DirectAccess64(inputFileName))
                {
                    //maxMfn = accessor.GetMaxMfn();
                    maxMfn = 150000;
                    Console.WriteLine("Max MFN={0}", maxMfn);

                    // Сначала считываем все записи
                    for (int mfn = 1; mfn < maxMfn; mfn++)
                    {
                        ReadRecord(mfn);
                    }
                }

                using (StreamWriter textWriter = File.CreateText("words.dic"))
                {
                    string[] keys = dictionary.Keys.ToArray();
                    Array.Sort(keys);
                    foreach (string key in keys)
                    {
                        textWriter.WriteLine("{0}\t{1}", key, dictionary[key]);
                    }
                }

                // Дожидаемся завершения
                // processBlock.Complete();
                // processBlock.Completion.Wait();

                Console.WriteLine
                    (
                        "Good records={0}, dictionary size={1}, longest array={2}",
                        goodRecords,
                        dictionary.Count,
                        longest
                    );

                DictionaryCounterInt32<int> counter = new DictionaryCounterInt32<int>();
                using (FileStream stream = File.OpenRead("words.bin"))
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    while (stream.Position < stream.Length)
                    {
                        BookData data = new BookData();
                        data.RestoreFromStream(reader);
                        foreach (int word in data.Words)
                        {
                            counter.Increment(word);
                        }
                    }
                }

                int maxCount = counter.Values.Max();
                int threshold = maxCount / 5 + 1;
                Console.WriteLine
                    (
                        "Max count={0}, threshold={1}",
                        maxCount,
                        threshold
                    );

                using (FileStream stream = File.OpenRead("words.bin"))
                using (BinaryReader reader = new BinaryReader(stream))
                using (StreamWriter textWriter = File.CreateText("words.csv"))
                {
                    while (stream.Position < stream.Length)
                    {
                        BookData data = new BookData();
                        data.RestoreFromStream(reader);

                        int i;
                        for (i = 0; i < data.Words.Length; i++)
                        {
                            textWriter.Write("{0},", data.Words[i]);
                        }
                        for (; i < longest; i++)
                        {
                            textWriter.Write("0,");
                        }
                        textWriter.WriteLine("{0}", data.Count);
                    }
                }

                Console.WriteLine("Complete");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            stopwatch.Stop();
            TimeSpan elapsed = stopwatch.Elapsed;
            Console.WriteLine("Elapsed: {0}", elapsed.ToAutoString());
        }
    }
}
