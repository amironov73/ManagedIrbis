// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- программа удаляет у указанных экземпляров отметку об инвентаризации.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using ManagedIrbis;

#endregion

// ReSharper disable UseStringInterpolation
// ReSharper disable StringLiteralTypo
// ReSharper disable LocalizableElement

namespace RemoveInventarization
{
    class Program
    {
        private static int _index;
        private static Stopwatch _stopwatch;
        private static IrbisConnection _connection;

        /// <summary>
        /// Форматируем отрезок времени
        /// в виде ЧЧ:ММ:СС.
        /// </summary>
        private static string _FormatTimeSpan
            (
                TimeSpan timeSpan
            )
        {
            string result = string.Format
            (
                "{0:00}:{1:00}:{2:00}",
                timeSpan.Hours,
                timeSpan.Minutes,
                timeSpan.Seconds
            );

            return result;
        }

        static bool ProcessRecord
            (
                MarcRecord record,
                string  number
            )
        {
            RecordField field = record.Fields
                .GetField(910)
                .GetField('b', number)
                .FirstOrDefault();
            if (ReferenceEquals(field, null))
            {
                Console.WriteLine("{0}: no 910", number);
                return false;
            }

            bool processed = field.HaveSubField('!')
                             || field.HaveSubField('s');
            if (processed)
            {
                field.RemoveSubField('!');
                field.RemoveSubField('s');
                Console.Write("[{0}]", record.Mfn);
            }
            else
            {
                Console.Write("{0} ", record.Mfn);
            }

            return processed;
        }

        static void ProcessNumber
            (
                string number
            )
        {
            number = number.Trim();
            if (string.IsNullOrEmpty(number))
            {
                return;
            }

            Console.Write
                (
                    "{0,6}) {1} ",
                    _index++,
                    _FormatTimeSpan(_stopwatch.Elapsed)
                );

            int[] mfns = _connection.Search("\"IN=" + number + "\"");
            if (mfns.Length == 0)
            {
                Console.WriteLine("{0}: not found", number);

                return;
            }

            Console.Write("{0}: ", number);

            foreach (int mfn in mfns)
            {
                MarcRecord record = _connection.ReadRecord(mfn);
                if (ProcessRecord(record, number))
                {
                    _connection.WriteRecord(record);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Аргументы: 1) имя файла в формате "одна строчка - один номер",
        /// 2) строка подключения к серверу.
        /// </summary>
        /// <param name="args"></param>
        static void Main
            (
                string[] args
            )
        {
            if (args.Length != 2)
            {
                Console.WriteLine
                    ("RemoveInventarization <sigla.txt> <connectionString>");
                return;
            }

            string fileName = args[0];
            string connectionString = args[1];

            _stopwatch = new Stopwatch();

            try
            {
                using (var reader = new StreamReader(fileName, Encoding.Default))
                using (_connection = new IrbisConnection(connectionString))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        ProcessNumber(line);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            _stopwatch.Stop();

            Console.WriteLine();
            Console.WriteLine("Elapsed: {0}", _FormatTimeSpan(_stopwatch.Elapsed));
        }
    }
}
