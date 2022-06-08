// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global

/*

 Анализ спрашиваемости журналов определенного фонда

 Находятся все выпуски журнала и суммируется количество выдач,
 проставленных АРМ "Книговыдача" в каждом из выпусков.

 */

#region Using directives

using System;
using System.Linq;

using AM;

using ManagedIrbis;
using ManagedIrbis.Magazines;

#endregion

internal class Program
{
    private static readonly string ConnectionString
        = "server=127.0.0.1;user=librarian;password=secret;db=IBIS;";

    public static void Main ()
    {
        try
        {
            using (var connection = new IrbisConnection (ConnectionString))
            {
                // var searchExpression = "(MHR=Ф504 + MHR=Ф505 + MHR=Ф506) * V=NJ "
                //     + "* (G=2021 + G=2020 + G2019 + G=2018 + G=2017)";

                var searchExpression = "MHR=Ф404 * V=NJ "
                    + "* (G=2021 + G=2020 + G2019 + G=2018 + G=2017)";

                var found = connection.Search (searchExpression);
                Console.WriteLine ($"Found={found.Length}"); // количество найденных выпусков журнала

                var manager = new MagazineManager (connection);
                var magazines = manager.GetAllMagazines()
                    .OrderBy (mag => mag.Title)
                    .ToArray();

                // проходимся по всем выпускам
                var records = connection.ReadRecords (connection.Database, found);
                foreach (var record in records)
                {
                    var issue = MagazineIssueInfo.Parse (record);
                    var magazine = magazines.FirstOrDefault (mag => mag.Index.SameString (issue.MagazineCode));
                    if (ReferenceEquals (magazine, null))
                    {
                        // если журнал не найден, это что-то странное
                        continue;
                    }

                    // суммируем количество выдач
                    var count = 0;
                    if (!ReferenceEquals (magazine.UserData, null))
                    {
                        count = (int) magazine.UserData;
                    }

                    count += issue.LoanCount;
                    magazine.UserData = count;

                } // foreach

                // выводим результат в консоль
                foreach (var magazine in magazines)
                {
                    if (ReferenceEquals (magazine.UserData, null))
                    {
                        // если свойство UserData пустое,
                        // значит, ни одного выпуска не было проанализовано
                        // похоже, это не наш журнал
                        continue;
                    }

                    var count = (int) magazine.UserData;

                    Console.Out.WriteLine ($"{magazine.Title};{count}");

                } // foreach

            } // using

        } // try

        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
        }

    } // method Main

} // class Program
