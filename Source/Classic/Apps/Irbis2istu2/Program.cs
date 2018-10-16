// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using AM;
using AM.Collections;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

#endregion

// ReSharper disable StringLiteralTypo
// ReSharper disable LocalizableElement

namespace Irbis2istu2
{
    static class Program
    {
        private static DbManager _database;
        private static Stopwatch _stopwatch;
        private static StreamReader _reader;
        private static CaseInsensitiveDictionary<bool> _indexes;
        private static string _sourceFile, _sqlConnectionString;
        private static PftProgram _titleProgram, _headingProgram,
            _authorsProgram, _exemplarsProgram, _linkProgram, _typeProgram,
            _briefProgram;

        private static string Limit
            (
                [NotNull] this string text,
                int length
            )
        {
            return text.Length < length
                ? text
                : text.Substring(0, length);
        }

        [NotNull]
        private static PftFormatter _GetFormatter
            (
                [NotNull] PftProgram program
            )
        {
            PftFormatter result = new PftFormatter
            {
                Program = program
            };
            LocalProvider provider = (LocalProvider) result.Context.Provider;
            provider.FallForwardPath = ".";

            return result;
        }

        [NotNull]
        private static string _GetTitle
            (
                [NotNull] MarcRecord record
            )
        {
            if (ReferenceEquals(_titleProgram, null))
            {
                string source = File.ReadAllText("title.pft", IrbisEncoding.Ansi);
                _titleProgram = PftUtility.CompileProgram(source);
            }

            PftFormatter formatter = _GetFormatter(_titleProgram);
            string result = formatter.FormatRecord(record);

            return result.Limit(250);
        }

        [NotNull]
        private static string _GetDescription
            (
                [NotNull] MarcRecord record
            )
        {
            if (ReferenceEquals(_briefProgram, null))
            {
                string source = File.ReadAllText("sbrief_istu.pft", IrbisEncoding.Ansi);
                _briefProgram = PftUtility.CompileProgram(source);
            }

            PftFormatter formatter = _GetFormatter(_briefProgram);
            string result = formatter.FormatRecord(record);

            return result.Limit(500);
        }

        [CanBeNull]
        private static string _GetHeading
            (
                [NotNull] MarcRecord record
            )
        {
            if (ReferenceEquals(_headingProgram, null))
            {
                string source = File.ReadAllText("heading.pft", IrbisEncoding.Ansi);
                _headingProgram = PftUtility.CompileProgram(source);
            }

            PftFormatter formatter = _GetFormatter(_headingProgram);
            string result = formatter.FormatRecord(record).Limit(128);

            return result.EmptyToNull();
        }

        [CanBeNull]
        private static string _GetAuthors
            (
                [NotNull] MarcRecord record
            )
        {
            if (ReferenceEquals(_authorsProgram, null))
            {
                string source = File.ReadAllText("authors.pft", IrbisEncoding.Ansi);
                _authorsProgram = PftUtility.CompileProgram(source);
            }

            PftFormatter formatter = _GetFormatter(_authorsProgram);
            string merged = formatter.FormatRecord(record);
            string[] lines = merged.SplitLines().NonEmptyLines().ToArray();
            string result = string.Join("; ", lines).Limit(200);

            return result.EmptyToNull();
        }

        private static int _GetYear
            (
                [NotNull] MarcRecord record
            )
        {
            string result = record.FM(210, 'd');
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

            Match match = Regex.Match(result, @"\d{4}");
            if (match.Success)
            {
                result = match.Value;
            }
            return result.SafeToInt32();
        }

        private static int _GetExemplars
            (
                [NotNull] MarcRecord record
            )
        {
            if (ReferenceEquals(_exemplarsProgram, null))
            {
                string source = File.ReadAllText("exemplars.pft", IrbisEncoding.Ansi);
                _exemplarsProgram = PftUtility.CompileProgram(source);
            }

            PftFormatter formatter = _GetFormatter(_exemplarsProgram);
            string result = formatter.FormatRecord(record);

            return result.SafeToInt32();
        }

        [CanBeNull]
        private static string _GetLink
            (
                [NotNull] MarcRecord record
            )
        {
            if (ReferenceEquals(_linkProgram, null))
            {
                string source = File.ReadAllText("link.pft", IrbisEncoding.Ansi);
                _linkProgram = PftUtility.CompileProgram(source);
            }

            PftFormatter formatter = _GetFormatter(_linkProgram);
            string result = formatter.FormatRecord(record).Limit(200);

            return result.EmptyToNull();
        }

        [NotNull]
        private static string _GetType
            (
                [NotNull] MarcRecord record
            )
        {
            if (ReferenceEquals(_typeProgram, null))
            {
                string source = File.ReadAllText("type.pft", IrbisEncoding.Ansi);
                _typeProgram = PftUtility.CompileProgram(source);
            }

            PftFormatter formatter = _GetFormatter(_typeProgram);
            string formatted = formatter.FormatRecord(record);
            string result = formatted.Contains("1")
                ? "электронный"
                : "традиционный";

            return result;
        }

        [CanBeNull]
        public static MarcRecord ReadRecord
            (
                int mfn
            )
        {
            const string recordSeparator = "*****";

            MarcRecord result = new MarcRecord
            {
                Mfn = mfn,
                Database = "ISTU"
            };
            while (true)
            {
                string line = _reader.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    return null;
                }

                if (line == recordSeparator)
                {
                    break;
                }

                if (line[0] != '#')
                {
                    return null;
                }

                int pos = line.IndexOf(':') + 1;
                if (pos <= 1 || line[pos] != ' ')
                {
                    return null;
                }

                int tag = FastNumber.ParseInt32(line, 1, pos - 2);
                RecordField field = new RecordField(tag);
                result.Fields.Add(field);
                int start = ++pos, length = line.Length;
                while (pos < length)
                {
                    if (line[pos] == '^')
                    {
                        break;
                    }
                    pos++;
                }

                if (pos != start)
                {
                    field.Value = line.Substring(start, pos - start);
                    start = pos;
                }

                while (start < length - 1)
                {
                    char code = line[++start];
                    pos = ++start;
                    while (pos < length)
                    {
                        if (line[pos] == '^')
                        {
                            break;
                        }
                        pos++;
                    }

                    SubField sub = new SubField
                        (
                            code,
                            line.Substring(start, pos - start)
                        );
                    field.SubFields.Add(sub);
                    start = pos;
                }
            }

            result.Modified = false;
            return result;
        }

        static void ProcessRecord
            (
                [NotNull] MarcRecord record
            )
        {
            string index = record.FM(903);
            if (string.IsNullOrEmpty(index))
            {
                return;
            }

            if (_indexes.ContainsKey(index))
            {
                Console.WriteLine("Repeating index: {0}", index);
                return;
            }

            _indexes.Add(index, false);

            string description = _GetDescription(record);

            if (string.IsNullOrEmpty(description))
            {
                return;
            }

            description = description.Limit(500);

            IrbisData data = new IrbisData
            {
                Index = index.Limit(32),
                Description = description,
                Heading = _GetHeading(record),
                Title = _GetTitle(record),
                Author = _GetAuthors(record),
                Count = _GetExemplars(record),
                Year = _GetYear(record),
                Link = _GetLink(record),
                Type = _GetType(record)
            };

            _database.Insert(data);
            Console.WriteLine("[{0}] {1}", record.Mfn, data.Description);
        }

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Need 2 arguments");
                return;
            }

            _indexes = new CaseInsensitiveDictionary<bool>();
            _sourceFile = args[0];
            _sqlConnectionString = args[1];

            try
            {
                _stopwatch = new Stopwatch();
                _stopwatch.Start();

                Sql2000DataProvider dataProvider = new Sql2000DataProvider();
                using (_reader = new StreamReader(_sourceFile))
                using (_database = new DbManager(dataProvider, _sqlConnectionString))
                {
                    Console.WriteLine
                    (
                        "Started at: {0}",
                        DateTime.Now.ToLongUniformString()
                    );

                    _database
                        .SetCommand("delete from [dbo].[irbisdata]")
                        .ExecuteNonQuery();
                    Console.WriteLine("table truncated");

                    int mfn = 1;
                    MarcRecord record;
                    while ((record = ReadRecord(mfn)) != null)
                    {
                        try
                        {
                            ProcessRecord(record);
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine("[{0}] {1}", mfn, exception.Message);
                        }

                        mfn++;
                    }

                    _database
                        .SetCommand("EXECUTE [upload_done]")
                        .ExecuteNonQuery();
                    Console.WriteLine("[upload_done]");

                    _database
                        .SetCommand("insert into [FlagTable] default values")
                        .ExecuteNonQuery();
                    Console.WriteLine("[FlagTable]");
                }

                _stopwatch.Stop();
                TimeSpan elapsed = _stopwatch.Elapsed;
                Console.WriteLine
                    (
                        "Elapsed: {0}",
                        elapsed.ToAutoString()
                    );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
