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

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

#endregion

// ReSharper disable LocalizableElement

namespace Irbis2istu
{
    static class Program
    {
        private static string _irbisConnectionString, _sqlConnectionString;
        private static IrbisConnection _irbisConnection;
        private static DbManager _database;
        private static Stopwatch _stopwatch;
        private static PftProgram _titleProgram, _headingProgram,
            _authorsProgram, _exemplarsProgram, _linkProgram, _typeProgram;
        // private static PftProgram _briefProgram;

        [NotNull]
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
        private static string _GetTitle
            (
                [NotNull] MarcRecord record
            )
        {
            if (ReferenceEquals(_titleProgram, null))
            {
                string source = File.ReadAllText("title.pft");
                _titleProgram = PftUtility.CompileProgram(source);
            }

            PftFormatter formatter = new PftFormatter
            {
                Program = _titleProgram
            };
            string result = formatter.FormatRecord(record);

            return result.Limit(250);
        }

        //[NotNull]
        //private static string _GetDescription
        //    (
        //        [NotNull] MarcRecord record
        //    )
        //{
        //    if (ReferenceEquals(_briefProgram, null))
        //    {
        //        string source = File.ReadAllText("sbrief.pft");
        //        _briefProgram = PftUtility.CompileProgram(source);
        //    }

        //    PftFormatter formatter = new PftFormatter
        //    {
        //        Program = _briefProgram
        //    };
        //    string result = formatter.FormatRecord(record);

        //    return result.Limit(500);
        //}

        [CanBeNull]
        private static string _GetHeading
            (
                [NotNull] MarcRecord record
            )
        {
            if (ReferenceEquals(_headingProgram, null))
            {
                string source = File.ReadAllText("heading.pft");
                _headingProgram = PftUtility.CompileProgram(source);
            }

            PftFormatter formatter = new PftFormatter
            {
                Program = _headingProgram
            };
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
                string source = File.ReadAllText("authors.pft");
                _authorsProgram = PftUtility.CompileProgram(source);
            }

            PftFormatter formatter = new PftFormatter
            {
                Program = _authorsProgram
            };
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
                string source = File.ReadAllText("exemplars.pft");
                _exemplarsProgram = PftUtility.CompileProgram(source);
            }

            PftFormatter formatter = new PftFormatter
            {
                Program = _exemplarsProgram
            };
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
                string source = File.ReadAllText("link.pft");
                _linkProgram = PftUtility.CompileProgram(source);
            }

            PftFormatter formatter = new PftFormatter
            {
                Program = _linkProgram
            };
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
                string source = File.ReadAllText("type.pft");
                _typeProgram = PftUtility.CompileProgram(source);
            }

            PftFormatter formatter = new PftFormatter
            {
                Program = _typeProgram
            };
            string formatted = formatter.FormatRecord(record);
            string result = formatted.Contains("1")
                ? "электронный"
                : "традиционный";

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

            string description = _irbisConnection.FormatRecord("@sbrief", record.Mfn);
            //string description = _GetDescription(record);

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
                return;
            }

            _irbisConnectionString = args[0];
            _sqlConnectionString = args[1];

            try
            {
                _stopwatch = new Stopwatch();
                _stopwatch.Start();

                Sql2000DataProvider dataProvider = new Sql2000DataProvider();
                using (_irbisConnection = new IrbisConnection(_irbisConnectionString))
                using (_database = new DbManager(dataProvider, _sqlConnectionString))
                {
                    Console.WriteLine
                        (
                            "Started at: {0}",
                            DateTime.Now.ToLongUniformString()
                        );

                    int maxMfn = _irbisConnection.GetMaxMfn();
                    Console.WriteLine("Max MFN={0}", maxMfn);

                    _database
                        .SetCommand("delete from [dbo].[irbisdata]")
                        .ExecuteNonQuery();
                    Console.WriteLine("table truncated");

                    BatchRecordReader batch = (BatchRecordReader)BatchRecordReader.WholeDatabase
                        (
                            _irbisConnection,
                            _irbisConnection.Database,
                            500
                        );

                    foreach (MarcRecord record in batch)
                    {
                        ProcessRecord(record);
                    }

                    _database
                        .SetCommand("insert into [FlagTable] default values")
                        .ExecuteNonQuery();

                    _stopwatch.Stop();
                    TimeSpan elapsed = _stopwatch.Elapsed;
                    Console.WriteLine
                        (
                            "Elapsed: {0}",
                            elapsed.ToAutoString()
                        );
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
