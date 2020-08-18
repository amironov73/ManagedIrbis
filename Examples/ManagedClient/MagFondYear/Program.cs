// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement

/* Program.cs -- таблица "фонды-годы"
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Linq;

using AM;

using ManagedIrbis;
using ManagedIrbis.Fields;
using ManagedIrbis.Magazines;

#endregion

/*
 * Программа формирует простую таблицу для журналов:
 * по горизонтали - фонды
 * по вертикали - годы издания
 * в клетках - количество экземпляров.
 */

namespace MagFondYear
{
    /// <summary>
    /// Информация о фонде: код плюс массив по годам издания.
    /// </summary>
    internal class FondInfo
    {
        /// <summary>
        /// Код места хранения.
        /// </summary>
        public readonly string Place;

        /// <summary>
        /// Массив с количеством экземпляров.
        /// Индекс в массиве соответствует году издания.
        /// </summary>
        public readonly int[] Years;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FondInfo(string place)
        {
            Place = place;
            Years = new int[2200];
        }

        public override string ToString()
        {
            return Place;
        }
    }

    internal static class Program
    {
        private static IrbisConnection Connection;
        private static MagazineManager Manager;
        private static MagazineInfo[] Magazines;
        private const int LowYear = 1700;
        private const int HighYear = 2020;

        private static readonly FondInfo[] Fonds =
        {
            new FondInfo("*"),
            new FondInfo("ФП"),
            new FondInfo("Ф104"),
            new FondInfo("Ф201"),
            new FondInfo("Ф202"),
            new FondInfo("Ф301"),
            new FondInfo("Ф302"),
            new FondInfo("Ф303"),
            new FondInfo("Ф304"),
            new FondInfo("Ф401"),
            new FondInfo("Ф403"),
            new FondInfo("Ф404"),
            new FondInfo("Ф501"),
            new FondInfo("Ф502"),
            new FondInfo("Ф503"),
            new FondInfo("Ф504"),
            new FondInfo("Ф505"),
            new FondInfo("Ф506"),
            new FondInfo("Ф507"),
            new FondInfo("Ф601"),
            new FondInfo("Ф602"),
            new FondInfo("Ф603"),
            new FondInfo("Ф604")
        };

        /// <summary>
        /// Отработка конкретного выпуска журнала.
        /// </summary>
        private static void ProcessIssue(MagazineIssueInfo issue)
        {
            var record = issue.Record;
            if (!ReferenceEquals(record, null))
            {
                var exemplars = ExemplarInfo.Parse(record);
                if (!ReferenceEquals(exemplars, null))
                {
                    var year = issue.Year.SafeToInt32();
                    if (year >= 1800 && year <= 2020)
                    {
                        foreach (var exemplar in exemplars)
                        {
                            var place = exemplar.Place;
                            var found = false;
                            if (!string.IsNullOrEmpty(place))
                            {
                                foreach (var fond in Fonds)
                                {
                                    if (place.SameString(fond.Place))
                                    {
                                        fond.Years[year]++;
                                        found = true;
                                    }
                                }

                                if (!found)
                                {
                                    Fonds[0].Years[year]++;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Отработка названия журнала.
        /// </summary>
        private static void ProcessMagazine(MagazineInfo magazine)
        {
            Console.WriteLine(magazine.Title);
            var issues = Manager.GetIssues(magazine);
            Console.WriteLine($"\t{issues.Length}");
            foreach (var issue in issues)
            {
                ProcessIssue(issue);
            }
        }

        /// <summary>
        /// Сохранение результата в текстовый файл.
        /// </summary>
        private static void SaveResults()
        {
            var others = Fonds.First(); // "прочие"
            // фонды, подлежащие подсчету
            var mainFonds = Fonds.Skip(1).ToArray();

            using (var writer = File.CreateText("results.txt"))
            {
                writer.Write("Фонд");
                foreach (var fond in mainFonds)
                {
                    writer.Write($"\t{fond.Place}");
                }

                writer.WriteLine($"\t{others.Place}");

                for (var year = LowYear; year <= HighYear; year++)
                {
                    writer.Write($"{year}");
                    foreach (var fond in mainFonds)
                    {
                        writer.Write($"\t{fond.Years[year]}");
                    }
                    writer.WriteLine($"\t{others.Years[year]}");
                }
            }
        }

        /// <summary>
        /// Точка входа в программу.
        /// </summary>
        public static void Main()
        {
            try
            {
                using (Connection = IrbisConnectionUtility.GetClientFromConfig())
                {
                    Console.WriteLine("Загрузка названий журналов...");
                    Manager = new MagazineManager(Connection);
                    Magazines = Manager.GetAllMagazines();
                    Magazines = Magazines
                        .Where(m => !m.IsNewspaper)
                        .OrderBy(m => m.Title)
                        .ToArray();
                    Console.WriteLine($"Всего журналов: {Magazines.Length}");

                    foreach (var magazine in Magazines)
                    {
                        ProcessMagazine(magazine);
                    }
                }

                SaveResults();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
