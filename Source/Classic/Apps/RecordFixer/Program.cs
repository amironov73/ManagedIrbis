// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

using System;
using System.Linq;

using AM;

using ManagedIrbis;
using ManagedIrbis.Fields;

namespace RecordFixer
{
    class Program
    {
        // Строка подключения
        private const string ConnectionString = "host=127.0.0.1;port=6666;user=1;password=1;arm=A;";

        // База, в которой происходит восстановление
        private const string MainDatabase = "IBIS";

        // База с резервной копией
        private const string BackupDatabase = "IBIS2";

        // Поле связи
        private const int LinkTag = 1119;

        // Префикс связи
        private const string LinkPrefix = "GUID=";

        // Поля, остающиеся в записи
        private static readonly int[] ResiduaryTags = { 907, 910, 999 };

        // Условие отбора записей, подлежащих восстановлению
        private const string SearchExpression = "TH=FETD=AVD-КТ-201711";

        // Крайняя дата редактирования, после которой запись считается
        // не подлежащей восстановлению
        private static readonly DateTime ThresholdDate = new DateTime(2017, 11, 2);

        // Выполнять восстановление записей?
        private static readonly bool DoRestore = false;

        // Используемое подключение
        private static IrbisConnection connection;

        // Признак прерывания
        private static bool Cancel;

        private static bool CompareRecords
            (
                MarcRecord mainRecord,
                MarcRecord backupRecord
            )
        {
            bool result = true;

            int[] tags = mainRecord.Fields
                .Select(field => field.Tag)
                .Where(tag => !tag.OneOf(ResiduaryTags))
                .Distinct()
                .OrderBy(tag => tag)
                .ToArray();

            foreach (int tag in tags)
            {
                int count = mainRecord.Fields.GetFieldCount(tag);
                for (int occ = 0; occ < count; occ++)
                {
                    RecordField firstField = mainRecord.Fields.GetField(tag, occ)
                        .ThrowIfNull("firstField");
                    RecordField secondField = backupRecord.Fields.GetField(tag, occ);
                    if (ReferenceEquals(secondField, null))
                    {
                        Console.WriteLine("{0}/{1}\t{2}", tag, occ + 1, firstField.ToText());
                        Console.WriteLine("\t--нет--");
                        result = false;
                    }
                    else
                    {
                        secondField.UserData = true;
                        if (firstField.ToString() != secondField.ToString())
                        {
                            Console.WriteLine("{0}/{1}\t{2}", tag, occ + 1, firstField.ToText());
                            Console.WriteLine("\t{0}", secondField.ToText());
                            result = false;
                        }
                    }
                }
            }

            RecordField[] backupOnly = backupRecord.Fields
                .Where(field => !field.Tag.OneOf(ResiduaryTags))
                .Where(field => ReferenceEquals(field.UserData, null))
                .ToArray();
            if (backupOnly.Length != 0)
            {
                Console.WriteLine("Только в BACKUP:");
                foreach (RecordField field in backupOnly)
                {
                    Console.WriteLine("{0}/{1}\t{2}", field.Tag, field.Repeat + 1, field.ToText());
                    result = false;
                }
            }

            return result;
        }

        private static void ProcessRecord
            (
                int mfn
            )
        {
            connection.Database = MainDatabase;
            MarcRecord mainRecord = connection.ReadRecord(mfn);
            string linkValue = mainRecord.FM(LinkTag);

            connection.Database = BackupDatabase;
            MarcRecord backupRecord = connection.SearchReadOneRecord
                (
                    "\"{0}{1}\"",
                    LinkPrefix,
                    linkValue
                );
            if (ReferenceEquals(backupRecord, null))
            {
                Console.WriteLine
                    (
                        "Не нашлось парной записи для MFN={0} ({1}{2})",
                        mfn,
                        LinkPrefix,
                        linkValue
                    );
                Console.WriteLine(new string('=', 70));
                return;
            }

            Console.WriteLine("MFN={0}", mfn);
            RevisionInfo[] revisions = RevisionInfo.Parse(mainRecord)
                .Where(rev => rev.Name != "avd")
                .ToArray();
            if (revisions.Length == 0)
            {
                Console.WriteLine("Запись без ревизий!");
                Console.WriteLine(new string('=', 70));
                return;
            }

            RevisionInfo lastRevision = revisions.Last();
            DateTime lastEdit = IrbisDate.ConvertStringToDate(lastRevision.Date);
            Console.WriteLine
                (
                    "Последнее редактирование: {0} {1:d}",
                    lastRevision.Name,
                    lastEdit
                );
            if (lastEdit > ThresholdDate)
            {
                Console.WriteLine("Запись редактировалась, надо восстанавливать вручную");
                Console.WriteLine(new string('=', 70));
                return;
            }

            if (CompareRecords(mainRecord, backupRecord))
            {
                Console.WriteLine("Нет различий между MAIN и BACKUP");
                goto DONE;
            }

            if (DoRestore)
            {
                RecordField[] residuaryFields = mainRecord.Fields
                    .Where(field => field.Tag.OneOf(ResiduaryTags))
                    .Select(field => field.Clone())
                    .ToArray();
                RecordField[] backupFields = backupRecord.Fields
                    .Where(field => !field.Tag.OneOf(ResiduaryTags))
                    .Select(field => field.Clone())
                    .ToArray();
                mainRecord.Fields.Clear();
                mainRecord.Fields.AddRange(backupFields);
                mainRecord.Fields.AddRange(residuaryFields);
                Console.WriteLine
                    (
                        "Сформирована новая версия записи: {0} новых и {1} старых полей",
                        residuaryFields.Length,
                        backupFields.Length
                    );
                connection.WriteRecord(mainRecord);
            }

            DONE:
            Console.WriteLine(new string('=', 70));
            Console.WriteLine();
        }

        public static void Main()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            try
            {
                using (connection = new IrbisConnection())
                {
                    connection.ParseConnectionString(ConnectionString);
                    connection.Connect();
                    Console.WriteLine("Подключились");

                    connection.Database = MainDatabase;
                    int[] found = connection.Search(SearchExpression);
                    Console.WriteLine("Отобрано записей: {0}", found.Length);


                    foreach (int mfn in found)
                    {
                        if (Cancel)
                        {
                            break;
                        }
                        try
                        {
                            ProcessRecord(mfn);
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine
                                (
                                    "MFN={0}, {1}: {2}",
                                    mfn,
                                    exception.GetType(),
                                    exception.Message
                                );
                        }
                    }
                }
                Console.WriteLine("Отключились");
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
