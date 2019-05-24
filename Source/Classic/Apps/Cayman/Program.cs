using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using ManagedIrbis;

// ReSharper disable StringLiteralTypo
// ReSharper disable LocalizableElement

namespace Cayman
{
    class Program
    {
        private static string connectionString
            = "host=192.168.3.2;port=6666;user=miron;password=miron;db=IBIS;arm=C;";
        private static IrbisConnection _connection;

        static void ProcessBook(int mfn)
        {
            string description = _connection.FormatRecord("@sbrief", mfn);
            int count = _connection.FormatRecord("v999", mfn).SafeToInt32(0);
            Console.WriteLine($"{description}\t{count}");
        }

        static void ProcessBook(string title, string author)
        {
            if (!string.IsNullOrEmpty(author))
            {
                author = author.Split(' ')[0];
            }
            title = title
                .Replace("\"", string.Empty)
                .Replace("?", string.Empty)
                .Replace("(", string.Empty)
                .Replace(")", string.Empty)
                ;
            title = title.Trim();
        AGAIN: string expression = $"(\"T={title}\")";
            if (!string.IsNullOrEmpty(author))
            {
                expression += $" * (\"A={author}$\")";
            }

            int[] found = _connection.Search(expression);
            if (found.Length != 0)
            {
                foreach (int mfn in found)
                {
                    ProcessBook(mfn);
                }
                return;
            }

            int pos = title.LastIndexOfAny(new[] { '.', ':' });
            if (pos > 0)
            {
                title = title.Substring(0, pos);
                goto AGAIN;
            }

            // Console.WriteLine($"{title} {author}: НЕ НАЙДЕНО");
        }

        private static void ProcessBook(string line)
        {
            string[] parts = line.Split('\t');
            string title = parts[0];
            string author = null;
            if (parts.Length != 1)
            {
                author = parts[1];
            }
            ProcessBook(title, author);
        }

        static void Main()
        {
            try
            {
                string[] lines = File.ReadAllLines("booklist.txt");
                using (_connection = new IrbisConnection(connectionString))
                {
                    foreach (string line in lines)
                    {
                        ProcessBook(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
