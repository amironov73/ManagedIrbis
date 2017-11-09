// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

using System;
using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Collections;
using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Fields;
using ManagedIrbis.Readers;

namespace RdrStat
{
    class Program
    {
        // Строка подключения
        private const string ConnectionString = "host=127.0.0.1;port=6666;user=1;password=1;arm=A;";

        // База, по которой считается статистика
        private const string DatabaseName = "RDR";

        // Признак прерывания
        private static bool Cancel;

        // Используемое подключение
        private static IrbisConnection connection;

        // ==========================================================

        // Всего М и Ж
        private static int MaleCount, FemaleCount;

        // М по возрастам
        private static readonly DictionaryCounterInt32<int> MaleAge
            = new DictionaryCounterInt32<int>();

        // Ж по возрастам
        private static readonly DictionaryCounterInt32<int> FemaleAge
            = new DictionaryCounterInt32<int>();

        // События М по возрастам
        private static readonly DictionaryCounterInt32<int> MaleVisits
            = new DictionaryCounterInt32<int>();

        // События Ж по возрастам
        private static readonly DictionaryCounterInt32<int> FemaleVisits
            = new DictionaryCounterInt32<int>();

        static void Main()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            try
            {
                using (connection = new IrbisConnection())
                {
                    connection.ParseConnectionString(ConnectionString);
                    connection.Connect();
                    connection.Database = DatabaseName;
                    Console.WriteLine("Подключились");

                    IEnumerable<MarcRecord> records
                        = BatchRecordReader.WholeDatabase
                        (
                            connection,
                            DatabaseName,
                            500
                            //, rdr =>
                            //{
                            //    Console.WriteLine
                            //        (
                            //            "{0} из {1}",
                            //            rdr.RecordsRead,
                            //            rdr.TotalRecords
                            //        );
                            //}
                        );

                    foreach (MarcRecord record in records)
                    {
                        if (Cancel)
                        {
                            break;
                        }

                        if (record.Deleted)
                        {
                            continue;
                        }

                        ReaderInfo reader = ReaderInfo.Parse(record);
                        if (reader.WorkPlace.SafeContains("ИОГУНБ"))
                        {
                            continue;
                        }

                        int age = reader.Age;
                        int visits = reader.Visits.Count(v => v.IsVisit);

                        if (reader.Gender.SameString("ж"))
                        {
                            FemaleCount++;
                            FemaleAge.Increment(age);
                            FemaleVisits.Augment(age, visits);
                        }
                        else
                        {
                            MaleCount++;
                            MaleAge.Increment(age);
                            MaleVisits.Augment(age, visits);
                        }
                    }
                }
                Console.WriteLine("Отключились");

                Console.WriteLine(";Муж;Жен;М пос;Ж пос");
                Console.WriteLine
                    (
                        "Всего;{0};{1};{2};{3}",
                        MaleCount,
                        FemaleCount,
                        MaleVisits.Total,
                        FemaleVisits.Total
                    );
                for (int age = 12; age < 100; age++)
                {
                    Console.WriteLine
                        (
                            "{0};{1};{2};{3};{4}",
                            age,
                            MaleAge.GetValue(age),
                            FemaleAge.GetValue(age),
                            MaleVisits.GetValue(age),
                            FemaleVisits.GetValue(age)
                        );
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private static void Console_CancelKeyPress
            (
                object sender,
                ConsoleCancelEventArgs e
            )
        {
            Console.WriteLine("Прерываем обработку");
            Cancel = true;
            e.Cancel = true;
        }
    }
}
