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
using AM.Collections;
using AM.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Readers;
using ManagedIrbis.Search;

#endregion

// ReSharper disable LocalizableElement
// ReSharper disable InconsistentNaming
// ReSharper disable UseStringInterpolation
// ReSharper disable InlineOutVariableDeclaration

namespace CountReaders
{
    class Program
    {
        private const int MaxAttendance = 50;

        private static IrbisConnection connection;

        class ByDate : IEqualityComparer<VisitInfo>
        {
            public bool Equals(VisitInfo x, VisitInfo y)
            {
                return string.Equals
                    (
                        x.DateGivenString,
                        y.DateGivenString,
                        StringComparison.Ordinal
                    );
            }

            public int GetHashCode(VisitInfo obj)
            {
                return (obj.DateGivenString ?? string.Empty).GetHashCode();
            }
        }

        static void ProcessReader
            (
                ReaderInfo reader
            )
        {
            DateTime registration = reader.RegistrationDate;
            DateTime threshold = registration.AddYears(1);
            List<VisitInfo> visits = reader.Visits
                .Where(v => v.IsVisit && v.DateGiven < threshold)
                .Distinct(new ByDate())
                //.Where(v => !v.Department.SameString("АБ"))
                .ToList();
            reader.Visits = visits.ToArray();
        }

        private static string[] knownCategories =
        {
            "1",  // школьник
            "2",  // учащийся
            "3",  // студент
            "4",  // преподаватель ВУЗа
            "5",  // преподаватель ССУЗ
            "6",  // учитель школы, воспитатель
            "7",  // медицинский работник
            "8",  // экономист, бухгалтер
            "9",  // юрист
            "10", // административный работник
            "11", // ИТР и спец-т с/х
            "12", // работник культуры и искусства
            "13", // предприниматель
            "14", // специалист прочий
            "15", // рабочий
            "16", // безработный, пенсионер, домохозяйка, инвалид
            "17", // прочие
            "18"  // научный сотрудник
        };

        static void AnalyzeCategories
            (
                int year,
                int month
            )
        {
            string expression = string.Format
                (
                    CultureInfo.InvariantCulture,
                    "RD={0:0000}{1:00}$",
                    year,
                    month
                );
            int[] found = connection.Search(expression);
            List<MarcRecord> records = new BatchRecordReader
                (
                    connection,
                    connection.Database,
                    500,
                    true,
                    found
                )
                .ReadAll(true);
            ReaderInfo[] readers = records.Select(ReaderInfo.Parse).ToArray();
            DictionaryCounterInt32<string> counter =new DictionaryCounterInt32<string>();
            foreach (ReaderInfo reader in readers)
            {
                string category = reader.Category;
                if (!string.IsNullOrEmpty(category))
                {
                    counter.Increment(category);
                }
            }

            CultureInfo culture = CultureInfo.CurrentCulture;
            DateTimeFormatInfo format = culture.DateTimeFormat;
            Console.Write("{0} {1}", format.GetAbbreviatedMonthName(month), year);
            foreach (string category in knownCategories)
            {
                Console.Write("\t{0}", counter.GetValue(category));
            }
            int others = 0;
            foreach (string key in counter.Keys)
            {
                if (!knownCategories.Contains(key))
                {
                    int value = counter[key];
                    others += value;
                }
            }

            Console.WriteLine("\t{0}", others);
        }

        [NotNull]
        static List<int> AnalyzeAttendance
            (
                int year
            )
        {
            string expression = string.Format("RD={0}$", year.ToInvariantString());
            int[] found = connection.Search(expression);
            List<MarcRecord> records = new BatchRecordReader
                (
                    connection,
                    connection.Database,
                    500,
                    true,
                    found
                )
                .ReadAll(true);
            ReaderInfo[] readers = records.Select(ReaderInfo.Parse).ToArray();
            foreach (ReaderInfo reader in readers)
            {
                ProcessReader(reader);
            }

            List<int> result = new List<int> { readers.Length };
            for (int attendance = 1; attendance < MaxAttendance; attendance++)
            {
                int count = readers.Count(r => r.Visits.Length >= attendance);
                result.Add(count);
            }

            return result;
        }

        static void AnalyzeYear
            (
                int year
            )
        {
            string expression = string.Format("RD={0}$", year.ToInvariantString());
            int[] found = connection.Search(expression);
            List<MarcRecord> records = new BatchRecordReader
                    (
                        connection,
                        connection.Database,
                        500,
                        true,
                        found
                    )
                    .ReadAll(true);

            foreach (MarcRecord record in records)
            {
                ReaderInfo reader = ReaderInfo.Parse(record);

                DateTime registrationDate = reader.RegistrationDate;
                DateTime lastVisitDate = reader.LastVisitDate;
                if (lastVisitDate == DateTime.MinValue)
                {
                    lastVisitDate = reader.LastRegistrationDate;
                }
                if (lastVisitDate == DateTime.MinValue)
                {
                    if (!ReferenceEquals(reader.Enrollment, null)
                        && reader.Enrollment.Length != 0)
                    {
                        lastVisitDate = reader.Enrollment.Max(e => e.Date);
                    }
                }
                if (lastVisitDate == DateTime.MinValue)
                {
                    lastVisitDate = registrationDate;
                }
                int visitDays = (int)(lastVisitDate - registrationDate).TotalDays;
                if (visitDays == 0)
                {
                    visitDays = 1;
                }
                int visits = reader.Visits.Length;
                double rate = (double)visits / visitDays;
                Console.WriteLine
                    (
                        "{0}\t{1}\t{2}\t{3}\t{4}\t{5:F3}",
                        reader.Ticket,
                        registrationDate.ToShortDateString(),
                        visitDays,
                        reader.LastVisitDate.ToShortDateString(),
                        visits,
                        rate
                    );
            }
        }

        static void AnalyzeTerms()
        {
            TermParameters parameters = new TermParameters
            {
                Database = connection.Database,
                StartTerm = "RD=",
                NumberOfTerms = 10000
            };
            TermInfo[] terms = connection.ReadTerms(parameters);
            terms = terms.Where(ti => ti.Text.SafeStarts("RD=201"))
                .ToArray();
            terms = TermInfo.TrimPrefix(terms, "RD=");
            foreach (TermInfo term in terms)
            {
                DateTime date = IrbisDate.ConvertStringToDate(term.Text);
                Console.WriteLine("{0:yyyy-MM-dd}\t{1}", date, term.Count);
            }

        }

        static void AnalyzeAttendances()
        {
            Console.Write("Посещ.");
            Dictionary<int, List<int>> dictionary
                = new Dictionary<int, List<int>>();
            for (int year = 2010; year < 2018; year++)
            {
                List<int> analyzed = AnalyzeAttendance(year);
                dictionary.Add(year, analyzed);
                Console.Write("\t{0}", year);
            }

            Console.WriteLine();

            for (int i = 0; i < MaxAttendance; i++)
            {
                Console.Write("{0}", i);
                for (int year = 2010; year < 2018; year++)
                {
                    List<int> list = dictionary[year];
                    Console.Write("\t{0}", list[i]);
                }
                Console.WriteLine();
            }
        }

        static void AnalyzeCategories()
        {
            for (int year = 2010; year <= 2018; year++)
            {
                for (int month = 1; month <= 12; month++)
                {
                    AnalyzeCategories(year, month);
                }
            }
        }

        static void Main()
        {
            try
            {
                using (connection = IrbisConnectionUtility.GetClientFromConfig())
                {
                    AnalyzeCategories();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
