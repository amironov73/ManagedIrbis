// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using System;
using System.IO;
using System.Linq;

using AM;

using ManagedIrbis;

#endregion

// ReSharper disable StringLiteralTypo
// ReSharper disable LocalizableElement

namespace SetRealType
{
    class Program
    {
        private static string connnectionString
            = "host=192.168.1.1;port=6666;user=librarian;password=secret;db=IBIS;";

        private static string fileName = "numbers.txt";

        private static string realType = "журнал";

        private static void Main()
        {
            try
            {
                string[] lines = File.ReadAllLines(fileName);
                using (IrbisConnection connection = new IrbisConnection(connnectionString))
                {
                    foreach (string line in lines)
                    {
                        string number = line.Trim();
                        if (string.IsNullOrEmpty(number))
                        {
                            continue;
                        }

                        MarcRecord record = connection.SearchReadOneRecord("\"IN={0}\"", number);
                        if (ReferenceEquals(record, null))
                        {
                            Console.WriteLine("NOT FOUND: {0}", number);
                            continue;
                        }

                        RecordField field = record.Fields
                            .GetField(910)
                            .GetField('b', number)
                            .FirstOrDefault();
                        if (ReferenceEquals(field, null))
                        {
                            Console.WriteLine("CAN'T FIND FIELD: {0}", number);
                            continue;
                        }

                        //string value = field.GetFirstSubFieldValue('5');
                        //if (!value.SameString(realType))
                        {
                            field.SetSubField('5', realType);
                            field.SetSubField('s', IrbisDate.TodayText);
                            field.SetSubField('!', "Ф602");
                            connection.WriteRecord(record, false);
                        }

                        string description = connection.FormatRecord("@brief", record.Mfn);
                        Console.WriteLine("{0}\t{1}", number, description);
                    }
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
