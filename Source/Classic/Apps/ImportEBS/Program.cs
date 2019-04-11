using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using DevExpress.Spreadsheet;

using ManagedIrbis;

// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement

namespace ImportEBS
{
    class Program
    {
        private static IrbisConnection connection;
        private static Workbook workbook;
        private static Worksheet worksheet;
        private static readonly char[] separators = {' ', ','};
        private static int writeCounter = 0;

        static MarcRecord FindRecord(string[] numbers)
        {
            string[] databases = {"ISTU", "NTD", "PERIO"};

            foreach (string database in databases)
            {
                connection.Database = database;
                foreach (string number in numbers)
                {
                    MarcRecord record = connection.SearchReadOneRecord("\"IN={0}\"", number);
                    if (record != null)
                    {
                        return record;
                    }

                    if (number.StartsWith("dsk"))
                    {
                        string number2 = "ДСК" + number.Substring(3);
                        record = connection.SearchReadOneRecord("\"IN={0}\"", number2);
                        if (record != null)
                        {
                            Console.WriteLine("=> {0}", number2);
                            return record;
                        }
                    }

                    if (number.StartsWith("er"))
                    {
                        string number2 = "ЭР" + number.Substring(4);
                        record = connection.SearchReadOneRecord("\"IN={0}\"", number2);
                        if (record != null)
                        {
                            Console.WriteLine("=> {0}", number2);
                            return record;
                        }
                    }
                }
            }

            return null;
        }

        static void ProcessRecord(MarcRecord record, string download, string others)
        {
            record.AddNonEmptyField(3001, download);
            record.AddNonEmptyField(3002, others);
            record.Database = "EBS";
            record.Mfn = 0;
            record.Version = 0;
            record.Status = 0;
            connection.WriteRecord(record);
            writeCounter++;
        }

        static void ProcessCell(int rowNumber)
        {
            string rawText = worksheet.Cells[rowNumber, 6].Value.ToString();
            if (string.IsNullOrEmpty(rawText))
            {
                return;
            }

            string[] parts = rawText.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            MarcRecord record = FindRecord(parts);
            if (record == null)
            {
                Console.WriteLine("NOT FOUND: {0}", rawText);
                return;
            }

            string download = worksheet.Cells[rowNumber, 7].Value.ToString();
            string others = worksheet.Cells[rowNumber, 8].Value.ToString();

            //ProcessRecord(record, download, others);

            //Console.WriteLine(rawText);
        }

        static void Main()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                using (connection = new IrbisConnection())
                {
                    connection.Database = "ISTU";
                    connection.Username = "librarian";
                    connection.Password = "secret";
                    connection.Workstation = IrbisWorkstation.Administrator;
                    connection.Connect();


                    workbook = new Workbook();
                    workbook.LoadDocument("EBS.xlsx");
                    worksheet = workbook.Worksheets[1];

                    //connection.TruncateDatabase("EBS");

                    for (int rowNumber = 2; rowNumber < 22500; rowNumber++)
                    {
                        ProcessCell(rowNumber);
                    }

                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            Console.WriteLine("DONE: written={0}", writeCounter);
            stopwatch.Stop();
            TimeSpan elapsed = stopwatch.Elapsed;
            Console.WriteLine("Elapsed: {0} min", elapsed.ToMinuteString());
        }
    }
}
