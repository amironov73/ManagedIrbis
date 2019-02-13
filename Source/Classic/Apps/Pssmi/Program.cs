using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DevExpress.Spreadsheet;

using AM;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Search;

namespace Pssmi
{
    class Program
    {
        private const string connectionString = "host=127.0.0.1;port=6666;user=1;password=1;db=ISTU;";
        private static IrbisConnection Connection;
        private static Workbook workbook;
        private static Worksheet worksheet;
        private static int currentRow;

        static Cell WriteCell(int column, string text)
        {
            Cell result = worksheet.Cells[currentRow, column];
            if (int.TryParse(text, out int value))
            {
                result.Value = value;
            }
            else
            {
                result.Value = text;
            }

            return result;
        }

        static void Main()
        {
            try
            {
                workbook= new Workbook();
                worksheet = workbook.Worksheets[0];
                WriteCell(0, "Книга");
                WriteCell(1, "Всего");
                WriteCell(2, "Не");
                WriteCell(3, "910");
                WriteCell(4, "ПССМИ");
                currentRow++;

                using (Connection = new IrbisConnection(connectionString))
                {
                    Console.Write("Searching... ");
                    SearchParameters parameters = new SearchParameters
                    {
                        Database = Connection.Database,
                        SequentialSpecification = "v2010^a:'ПССМИ'"
                    };
                    int[] found = Connection.SequentialSearch(parameters);
                    Console.WriteLine($"found: {found.Length}");

                    string format = "&uf('6sbrief'), "
                    + "' ||| ', f(rsum((if p(v2010) then v2010^c, ',', fi,)),0,0)"
                    + "' ||| ', f(rsum((if v2010^a:'ПССМИ' then else v2010^c, ',', fi)),0,0)"
                    + "' ||| ', f(rsum((if v910^a:'U' then v910^1, ',', fi)),0,0)"
                    + "' ||| ', f(rsum((if v2010^a:'ПССМИ' then v2010^c, ',', fi)),0,0)";
                    BatchRecordFormatter formatter = new BatchRecordFormatter(Connection,
                        Connection.Database, format, 500, found);
                    List<string> all = formatter.FormatAll();
                    all = all
                        .NonEmptyLines()
                        .Select(item => item.Trim())
                        .NonEmptyLines()
                        .OrderBy(item => item)
                        .ToList();
                    string[] separators = {" ||| "};
                    foreach (string line in all)
                    {
                        string[] parts = line.Split(separators, StringSplitOptions.None);
                        WriteCell(0, parts[0]);
                        WriteCell(1, parts[1]);
                        WriteCell(2, parts[2]);
                        WriteCell(3, parts[3]);
                        WriteCell(4, parts[4]);

                        Console.WriteLine(line);
                        currentRow++;
                    }

                    Console.Write("Saving... ");
                    workbook.SaveDocument("PSSMI.xlsx");
                    Console.WriteLine("saved");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
