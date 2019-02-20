using System;
using System.Linq;
using System.Text.RegularExpressions;

using AM;

using BLToolkit.Data;

using DevExpress.Spreadsheet;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Fields;

// ReSharper disable StringLiteralTypo
// ReSharper disable LocalizableElement

namespace IstuPubl
{
    class Program
    {
        private static string connectionString = "host=127.0.0.1;port=6666;user=1;password=1;db=ISTU;";
        private static IrbisConnection connection;
        private static DbManager db;

        private static string searchExpression
            = "\"O=ИРНИТУ$\" * (G=2015 + G=2016 + G=2017 + G=2018)";

        private static string sciQuery = @"select count(*) from attendance as a
            where a.number = @number";
        private static string uchQuery1 = @"select count (a.id) from attendance as a
            inner join uchtrans as u on a.number = u.barcode
            where u.cardnum = @card";
        private static string uchQuery2 = @"select count(*) from uchtrans
            where cardnum=@card";

        private static MarcRecord[] records;

        private static Workbook workbook;
        private static Worksheet worksheet;
        private static int currentRow;

        static Cell WriteCell(int column, string text)
        {
            Cell result = worksheet.Cells[currentRow, column];
            result.Value = text;

            return result;
        }

        static Cell WriteCell(int column, int value)
        {
            Cell result = worksheet.Cells[currentRow, column];
            result.Value = value;

            return result;
        }

        private static int _GetYear
            (
                MarcRecord record
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


        static void Main(string[] args)
        {
            try
            {
                using (db = new DbManager())
                using (connection = new IrbisConnection(connectionString))
                {
                    records = BatchRecordReader.Search
                        (
                            connection,
                            connection.Database,
                            searchExpression,
                            1000
                        ).ToArray();
                    Console.WriteLine();
                    Console.WriteLine($"Found: {records.Length}");

                    workbook = new Workbook();
                    worksheet = workbook.Worksheets[0];
                    currentRow = 0;

                    WriteCell(0, "Биб. описание");
                    WriteCell(1, "Год издания");
                    WriteCell(2, "Кол-во научного");
                    WriteCell(3, "Выдача научного");
                    WriteCell(4, "Кол-во учебного");
                    WriteCell(5, "Выдача учебного");
                    currentRow++;

                    int count = 1;
                    foreach (MarcRecord record in records)
                    {
                        string description = connection.FormatRecord("@sbrief2", record);
                        string card = record.FM(2008);
                        string[] numbers = ExemplarInfo.Parse(record).Select(e => e.Number).ToArray();

                        int year = _GetYear(record);

                        int uchGiving = 0, uchCount = 0;
                        if (!string.IsNullOrEmpty(card))
                        {
                            uchGiving = db.SetCommand(uchQuery1, db.Parameter("card", card))
                                .ExecuteScalar<int>();
                            uchCount = db.SetCommand(uchQuery2, db.Parameter("card", card))
                                .ExecuteScalar<int>();
                        }

                        int sciCount = numbers.Length;
                        int sciGiving = 0;
                        foreach (string number in numbers)
                        {
                            int n = db.SetCommand(sciQuery, db.Parameter("number", number))
                                .ExecuteScalar<int>();
                            sciGiving += n;
                        }

                        Console.WriteLine($"{count++}\t{description}\t{uchGiving}\t{sciGiving}");

                        WriteCell(0, description);
                        WriteCell(1, year);
                        WriteCell(2, sciCount);
                        WriteCell(3, sciGiving);
                        WriteCell(4, uchCount);
                        WriteCell(5, uchGiving);
                        currentRow++;
                    }

                    Console.Write("Saving... ");
                    workbook.SaveDocument("Table.xlsx");
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
