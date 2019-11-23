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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DevExpress.Spreadsheet;

using AM;
using AM.Collections;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Fields;
using ManagedIrbis.Menus;
using ManagedIrbis.Search;

#endregion

// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

namespace CountForeign
{
    class Program
    {
        private static IrbisConnection _connection;
        private static IrbisProvider _provider;
        private static MarcRecord[] _records;
        private static Workbook _workbook;
        private static Worksheet _worksheet;
        private static int _currentRow;
        private static MenuFile _menu;

        private static readonly string[] _wrongLanguages =
        {
            "chi", "eng", "fre",
            "ger", "jpn", "kor", "mon", "rus", "spa", "scc"
        };

        private static Row CurrentLine()
        {
            return _worksheet.Rows[_currentRow];
        }

        private static Cell WriteCell(int column, string text)
        {
            Cell result = _worksheet.Cells[_currentRow, column];
            result.Value = text;

            return result;
        }

        private static Cell WriteCell(int column, int value)
        {
            Cell result = _worksheet.Cells[_currentRow, column];
            result.Value = value;

            return result;
        }

        private static void NewLine()
        {
            _currentRow++;
        }

        private static string GetAuthor
            (
                BookInfo book
            )
        {
            var result = "";

            foreach (var author in book.Authors)
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result = result + "; ";
                }

                result += author.ToFullName();
            }

            return result;
        }

        private static string GetLanguage(BookInfo book)
        {
            var languages = book.Languages;
            if (languages.IsNullOrEmpty())
            {
                return "rus";
            }

            foreach (var language in languages)
            {
                if (!language.SameString("rus"))
                {
                    return language;
                }
            }

            return "rus";
        }

        static bool FilterLanguage
            (
                BookInfo book
            )
        {
            return GetLanguage(book).ToLowerInvariant().OneOf(_wrongLanguages);
        }

        static void ProcessRecord
            (
                MarcRecord record
            )
        {
            var book = new BookInfo(_provider, record);
            if (FilterLanguage(book))
            {
                return;
            }

            WriteCell(0, _currentRow);
            WriteCell(1, GetAuthor(book));
            WriteCell(2, book.TitleText);
            WriteCell(3, record.FM(210, 'c'));
            WriteCell(4, record.FM(210, 'a'));
            WriteCell(5, book.Year);
            WriteCell(6, book.Pages);
            WriteCell(7, record.FM(225, 'a'));
            WriteCell(8, record.FM(606, 'a'));
            WriteCell(9, record.FM(621));
            WriteCell(10, GetLanguage(book));
            WriteCell(11, record.FM(215, 'x'));
            WriteCell(12, _menu.GetString
                (
                    record.FM(900, 'x') ?? "?",
                    "неизвестно")
                );
            NewLine();

            Console.WriteLine(book.Description);
        }

        static void Main()
        {
            var cities = File
                .ReadAllLines("Cities.txt")
                .NonEmptyLines()
                .ToArray();

            var expression = string.Concat
                (
                    "V=KN",
                    " * (",
                    SearchUtility.ConcatTerms
                        (
                            "G=",
                            "+",
                            new []{"2017$", "2018$", "2019$"}
                        ),
                    ") * (",
                    SearchUtility.ConcatTerms
                        (
                            "MI=",
                            "+",
                            cities
                        ),
                    ")"
                );

            try
            {
                _workbook = new Workbook();
                _worksheet = _workbook.Worksheets[0];

                WriteCell(0, "№ п/п");
                WriteCell(1, "Автор");
                WriteCell(2, "Заглавие");
                WriteCell(3, "Издательство");
                WriteCell(4, "Место публикации");
                WriteCell(5, "Год публикации");
                WriteCell(6, "Объем (стр)");
                WriteCell(7, "Серия");
                WriteCell(8, "Рубрика");
                WriteCell(9, "ББК");
                WriteCell(10, "Язык");
                WriteCell(11, "Тираж");
                WriteCell(12, "Целевое назначение");
                CurrentLine().Bold();
                NewLine();

                var connectionString = IrbisConnectionUtility.GetStandardConnectionString();
                using (_connection = new IrbisConnection(connectionString))
                {
                    _menu = _connection.ReadMenu("cn.mnu");
                    _provider = new ConnectedClient(_connection);
                    _records = _connection.SearchRead(expression);
                    Console.WriteLine("Records found: {0}", _records.Length);
                    foreach (var record in _records)
                    {
                        ProcessRecord(record);
                    }
                }

                _workbook.SaveDocument("foreign.xlsx");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
