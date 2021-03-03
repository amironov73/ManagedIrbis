using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Text;

using ManagedIrbis;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Mishina
{
    class FoundBook
    {
        public string Description { get; set; }

        public string Price { get; set; }

        public string Exemplars { get; set; }
    }

    class Program
    {
        private static string ConnectionString
            = "host=127.0.0.1;port=6666;user=librarian;password=secret;db=IBIS;";

        private static string GetPrice
            (
                MarcRecord record
            )
        {
            var price = record.FM(10, 'd');
            if (!string.IsNullOrEmpty(price))
            {
                return price;
            }

            return string.Empty;
        }

        static void Main(string[] args)
        {
            try
            {
                var connection = new IrbisConnection();
                connection.ParseConnectionString(ConnectionString);

                if (args.Length > 0)
                {
                    connection.Host = args[0];
                }
                if (args.Length > 1)
                {
                    connection.Username = args[1];
                }
                if (args.Length > 2)
                {
                    connection.Password = args[2];
                }
                if (args.Length > 3)
                {
                    connection.Database = args[3];
                }

                connection.Connect();

                var found = connection.Search("V=KN * KNL=2020");
                Console.WriteLine($"Найдено: {found.Length}");

                var list = new List<FoundBook>();

                foreach (var mfn in found)
                {
                    var record = connection.ReadRecord(mfn);
                    var description = connection.FormatRecord("@brief", mfn)
                        .Replace('"', ' ')
                        .Replace('\n', ' ')
                        .Replace('\r', ' ')
                        .Replace("  ", " ")
                        .Replace("  ", " ")
                        .Replace("  ", " ")
                        .Trim();
                    var price = GetPrice(record);
                    if (string.IsNullOrEmpty(price))
                    {
                        price = "нет";
                    }
                    var exemplars = connection.FormatRecord("'всего ', f(rsum((if p(v910^a) then '1;' fi)),0,0), ': ', &uf('O')", mfn);
                    var book = new FoundBook
                    {
                        Description = description,
                        Price = price,
                        Exemplars = exemplars
                    };
                    list.Add(book);
                    Console.Write('.');
                }

                Console.WriteLine();
                list = list.OrderBy(item => item.Description).ToList();
                Console.WriteLine();

                foreach (var book in list)
                {
                    Console.WriteLine($"{book.Description}\t{book.Price}\t{book.Exemplars}");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
