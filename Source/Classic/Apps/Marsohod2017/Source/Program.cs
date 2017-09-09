/* Program.cs
 */

/*
 * МАРСОХОД -- загрузчик ISO-файлов из проекта МАРС в ИРБИС64
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AM;
using ManagedIrbis;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace Marsohod2017
{
    /*========================================================*/

    /// <summary>
    /// Информация о журнале в целом.
    /// </summary>
    class MagazineInfo
    {
        #region Properties

        public string Title { get; set; }

        public string Index { get; set; }

        public string MarsCode { get; set; }

        public string Flag { get; set; }

        public int Mfn { get; set; }

        #endregion

        #region Construciton

        #endregion

        #region Private members

        #endregion

        #region Public methods


        public static MagazineInfo FromRecord
            (
                MarcRecord record
            )
        {
            int marsCode = NumericUtility.ParseInt32(CM.AppSettings["mars-code"]);
            int marsFlag = NumericUtility.ParseInt32(CM.AppSettings["mars-flag"]);

            MagazineInfo result = new MagazineInfo
            {
                Title = record.FM(200, 'a'),
                Index = record.FM(903),
                MarsCode = record.FM(marsCode),
                Flag = record.FM(marsFlag),
                Mfn = record.Mfn
            };

            return result;
        }

        #endregion
    }

    /*========================================================*/

    /// <summary>
    /// Строчка из FST-файла
    /// </summary>
    class FstLine
    {
        #region Properties

        /// <summary>
        /// Метка поля
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Метод
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Код преобразования
        /// </summary>
        public string Code { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private static readonly char[] _separator = { ' ' };

        #endregion

        #region Public methods

        public static FstLine ParseLine
            (
                string textLine
            )
        {
            textLine = textLine.Trim();
            if (string.IsNullOrEmpty(textLine))
            {
                return null;
            }

            string[] parts = textLine.Split(_separator, 3);
            if (parts.Length == 3)
            {
                FstLine fstLine = new FstLine
                {
                    Tag = parts[0],
                    Method = parts[1],
                    Code = parts[2]
                };
                return fstLine;
            }
            return null;
        }

        #endregion
    }

    /*========================================================*/

    class FstFile
    {
        #region Properties

        public List<FstLine> Lines { get { return _lines; } }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private readonly List<FstLine> _lines = new List<FstLine>();

        #endregion

        #region Public methods

        /*========================================================*/

        public MarcRecord Execute
            (
                IrbisConnection client,
                MarcRecord record
            )
        {
            MarcRecord result = new MarcRecord();

            foreach (FstLine fstLine in Lines)
            {
                string formatted = client.FormatRecord
                    (
                        fstLine.Code,
                        record
                    );
                if (!string.IsNullOrEmpty(formatted))
                {
                    formatted = formatted.Trim();
                    if (!string.IsNullOrEmpty(formatted))
                    {
                        formatted = formatted.Replace("\r", string.Empty);
                        string[] parts = formatted.Split
                            (
                                new[] { '\n' },
                                StringSplitOptions.RemoveEmptyEntries
                            );
                        foreach (string part in parts)
                        {
                            string trimmed = part.Trim();
                            if (!string.IsNullOrEmpty(trimmed))
                            {
                                RecordField field = RecordFieldUtility.Parse
                                    (
                                        fstLine.Tag,
                                        trimmed
                                    );
                                result.Fields.Add(field);
                            }
                        }
                    }
                }
            }

            return result;
        }

        /*========================================================*/

        public static FstFile ParseLocalFile
            (
                string fileName
            )
        {
            FstFile result = new FstFile();

            using (StreamReader reader = new StreamReader
                (
                    fileName,
                    Encoding.Default
                ))
            {
                string textLine;
                while ((textLine = reader.ReadLine()) != null)
                {
                    FstLine fstLine = FstLine.ParseLine(textLine);
                    if (fstLine != null)
                    {
                        result.Lines.Add(fstLine);
                    }
                }
            }

            return result;
        }

        /*========================================================*/

        public static FstFile ParseServerFile
            (
                IrbisConnection client,
                string fileName
            )
        {
            FstFile result = new FstFile();

            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    fileName
                );
            string content = client.ReadTextFile(specification);
            content = content.Replace("\r", string.Empty);
            foreach (string textLine in content.Split
                (
                    new[] { '\n' },
                    StringSplitOptions.RemoveEmptyEntries
                ))
            {
                FstLine fstLine = FstLine.ParseLine(textLine);
                if (fstLine != null)
                {
                    result.Lines.Add(fstLine);
                }
            }

            return result;
        }

        #endregion
    }

    /*========================================================*/

    class RecordTask
    {
        #region Properties

        public MarcRecord Record { get; set; }

        public string CurrentIssue { get; set; }

        public MagazineInfo Magazine { get; set; }

        #endregion
    }


    /*========================================================*/

    class Program
    {
        /// <summary>
        /// Строка подключения к серверу
        /// </summary>
        static string ConnectionString { get; set; }

        /// <summary>
        /// Папка с запакованными файлами
        /// </summary>
        static string InputPath { get; set; }

        /// <summary>
        /// Спецификация файлов
        /// </summary>
        static string FileSpec { get; set; }

        /// <summary>
        /// Имя файла с FST
        /// </summary>
        static string FstFileName { get; set; }

        static bool DeleteProcessedFiles { get; set; }

        static string LogFileName { get; set; }

        static string MinYear { get; set; }

        static string MaxYear { get; set; }

        static string WhenAlreadyExists { get; set; }

        static int QueueLength { get; set; }


        /*========================================================*/

        static FstFile Fst { get; set; }

        static IrbisConnection Client { get; set; }

        //static RecordBuffer Buffer { get; set; }

        static string CurrentIssue { get; set; }

        static Dictionary<string, MagazineInfo> Magazines { get; set; }

        static readonly Regex NameRegex = new Regex
            (
                @"^(?<code>\w{4})(?<year>\d\d)(?<volume>_to\d+)?(?<number>_(?:no|vy)\d+)"
              + @"(?<chapter>_ch\d+)?$"
            );

        static int FileCount;

        static int RecordCount;

        static DateTime StartTime;

        static volatile bool GoOn = true;

        static QueueEngine<RecordTask> Queue;

        static StreamWriter Log;

        private static readonly object SyncRoot = new object();

        /*========================================================*/

        static void WriteLogLine
            (
                string format,
                params object[] args
            )
        {
            lock (SyncRoot)
            {
                if (Log != null)
                {
                    Log.WriteLine(format, args);
                }
                Console.WriteLine(format, args);
            }
        }

        static void WriteLog
            (
                string format,
                params object[] args
            )
        {
            lock (SyncRoot)
            {
                if (Log != null)
                {
                    Log.Write(format, args);
                }
                Console.Write(format, args);
            }
        }

        static void ReadConfiguration()
        {
            ConnectionString = CM.AppSettings["connection-string"];
            InputPath = CM.AppSettings["input-path"];
            FileSpec = CM.AppSettings["file-spec"];
            FstFileName = CM.AppSettings["fst"];
            DeleteProcessedFiles = bool.Parse(CM.AppSettings["delete"]);
            LogFileName = CM.AppSettings["log-file-name"];
            MinYear = CM.AppSettings["min-year"];
            MaxYear = CM.AppSettings["max-year"];
            WhenAlreadyExists = CM.AppSettings["already-exists"];
            QueueLength = int.Parse(CM.AppSettings["queue-length"]);
        }

        static string[] DiscoverFiles()
        {
            string[] result = Directory.GetFiles
                (
                    InputPath,
                    FileSpec
                );

            return result;
        }

        static Dictionary<string, MagazineInfo> LoadMagazines()
        {
            Dictionary<string, MagazineInfo> result
                = new Dictionary<string, MagazineInfo>();
            MarcRecord[] records = Client.SearchRead("MARS=$");
            foreach (MarcRecord record in records)
            {
                MagazineInfo magazine = MagazineInfo.FromRecord(record);
                if ((magazine != null) && (magazine.MarsCode != null))
                {
                    result.Add
                        (
                            magazine.MarsCode,
                            magazine
                        );
                }
            }

            return result;
        }

        static string GetCode
            (
                MarcRecord record,
                string name
            )
        {
            return record.Fields
                .GetField(903)
                .GetField('a', name)
                .GetSubField('b')
                .GetSubFieldValue()
                .FirstOrDefault();
        }

        static bool ProcessRecord
            (
                MarcRecord record
            )
        {
            string article = record.FM(1);

            if (string.IsNullOrEmpty(article))
            {
                WriteLogLine("Запись без идентификатора, пропускаем ее");
                return false;
            }

            WriteLog("[{0}] СТАТЬЯ {1}: ", RecordCount, article);

            string code = GetCode(record, "code");
            string year = GetCode(record, "year");
            string volume = GetCode(record, "to");
            string number = GetCode(record, "no") ?? GetCode(record, "vy");
            string chapter = GetCode(record, "ch");

            if (string.IsNullOrEmpty(code))
            {
                WriteLog("Нет поля code, пропускаем запись ");
                return false;
            }
            if (string.IsNullOrEmpty(year))
            {
                WriteLog("Нет поля year, пропускаем запись ");
                return false;
            }
            if (string.IsNullOrEmpty(number))
            {
                WriteLog("Нет поля number, пропускаем запись ");
                return false;
            }

            MagazineInfo magazine;
            if (!Magazines.TryGetValue(code, out magazine))
            {
                WriteLog("Журнал с неизвестным кодом {0}, пропускаем ", code);
                return true;
            }

            if (magazine.Flag != "1")
            {
                WriteLog("Журнал не помечен как импортируемый, пропускаем ");
                return true;
            }

            if ((string.CompareOrdinal(year, MinYear) < 0)
              || (string.CompareOrdinal(year, MaxYear) > 0))
            {
                WriteLog
                    (
                        "Год {0} не входит в диапазон {1}-{2}, пропускаем ",
                        year,
                        MinYear,
                        MaxYear
                    );
                return true;
            } // if

            string issueIndex = magazine.Index + "/" + year;
            if (!string.IsNullOrEmpty(volume))
            {
                issueIndex += ("/" + volume);
            }
            issueIndex += ("/" + number);
            if (!string.IsNullOrEmpty(chapter))
            {
                issueIndex += ("/" + chapter);
            }

            if (CurrentIssue != issueIndex)
            {
                WriteLog
                    (
                        "ЖУРНАЛ {0} {1}/{2} ",
                        magazine.Title,
                        year,
                        number
                    );

                int[] previousArticles = new int[0];

                lock (SyncRoot)
                {
                    try
                    {
                        previousArticles = Client.Search
                            (
                                "\"II={0}\"",
                                issueIndex
                            );
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
                if (previousArticles.Length != 0)
                {
                    if (WhenAlreadyExists == "skip")
                    {
                        WriteLog
                            (
                                "Найдено {0} предыдущих версий статей, пропускаем{1}" ,
                                previousArticles.Length,
                                Environment.NewLine
                            );
                        return false;
                    }
                    if (WhenAlreadyExists == "delete")
                    {
                        WriteLog
                            (
                                "Найдено {0} предыдущих версий статей, удаляем... ",
                                previousArticles.Length
                            );
                        lock (SyncRoot)
                        {
                            Client.DeleteRecords
                                (
                                    Client.Database,
                                    previousArticles
                                );
                        }
                        WriteLog("удалено" + Environment.NewLine);
                    }
                    else
                    {
                        throw new ApplicationException
                            (
                                string.Format
                                (
                                    "Неизвестное управляющее слово {0}",
                                    WhenAlreadyExists
                                )
                            );
                    }
                } // if

                MarcRecord issueRecord = null;

                lock (SyncRoot)
                {
                    try
                    {
                        issueRecord = Client.SearchReadOneRecord
                            (
                                "\"I={0}\"",
                                issueIndex
                            );
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        return true;
                    }
                }
                if (issueRecord == null)
                {
                    WriteLog("Нет записи I={0}, создаем ее... ", issueIndex);
                    issueRecord = new MarcRecord();
                    issueRecord
                        .SetField(920, "NJ")
                        .SetField(933, magazine.Index)
                        .SetField(903, issueIndex)
                        .SetField(934, year)
                        .SetField(936, number)
                        .SetField(300, "Запись создана при импорте статей МАРС");
                    if (!string.IsNullOrEmpty(volume))
                    {
                        issueRecord.SetField(935, volume);
                    }
                    lock (SyncRoot)
                    {
                        //Buffer.Append(issueRecord);
                        Client.WriteRecord(issueRecord, false, true);
                        RecordCount++;
                    }
                    WriteLog("создана ");
                } // if

                CurrentIssue = issueIndex;
            } // if

            RecordTask recordTask = new RecordTask
            {
                CurrentIssue = CurrentIssue,
                Magazine = magazine,
                Record = record
            };

            Queue.QueueTask
                (
                    DoFormat,
                    recordTask
                );
            WriteLog("=> в очередь ");

            return true;
        }

        private static void DoFormat
            (
                object arg
            )
        {
            try
            {
                RecordTask taskInfo = (RecordTask)arg;
                using (IrbisConnection client = new IrbisConnection())
                {
                    client.ParseConnectionString(ConnectionString);
                    client.Connect();

                    MarcRecord targetRecord = Fst.Execute
                        (
                            client,
                            taskInfo.Record
                        );
                    if (targetRecord != null)
                    {
                        targetRecord.SetSubField(463, 'w', taskInfo.CurrentIssue);
                        targetRecord.SetSubField(463, 'c', taskInfo.Magazine.Title);

                        lock (SyncRoot)
                        {
                            RecordCount++;
                            //Buffer.Append(targetRecord);
                            client.WriteRecord(targetRecord, false, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogLine("EXCEPTION: {0}", ex);
            }
        }

        static void ProcessFile
            (
                int index,
                string fullName
            )
        {
            string fileName = Path.GetFileNameWithoutExtension(fullName);
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            WriteLog
                (
                    "{0}) ФАЙЛ {1}: ",
                    index,
                    fileName
                );

            Match match = NameRegex.Match(fileName);
            if (!match.Success)
            {
                WriteLogLine("неподдерживаемое имя файла, пропускаем");
                return;
            }

            WriteLogLine(string.Empty);

            using (FileStream stream = File.OpenRead(fullName))
            {
                while (GoOn)
                {
                    MarcRecord sourceRecord;

                    try
                    {
                        sourceRecord = Iso2709.ReadRecord
                            (
                                stream,
                                Encoding.Default
                            );
                    }
                    catch (Exception ex)
                    {
                        WriteLogLine("EXCEPTION: {0}",ex);
                        WriteLogLine("пропускаем файл");
                        continue;
                    }
                    if (sourceRecord == null)
                    {
                        break;
                    }

                    if (!ProcessRecord
                        (
                            sourceRecord
                        ))
                    {
                        break;
                    }
                    WriteLogLine(string.Empty);
                } // while

                FileCount++;
            } // using
        }

        static void Main()
        {
            try
            {
                StartTime = DateTime.Now;

                // Устанавливаем обработчик прерывания
                Console.CancelKeyPress += Console_CancelKeyPress;

                ReadConfiguration();

                using (Log = new StreamWriter(LogFileName, true, Encoding.Default))
                {
                    int mfnBefore, mfnAfter;

                    WriteLogLine(new string('=', 70));
                    WriteLogLine("Импорт начат: {0}", StartTime);
                    WriteLogLine(string.Empty);

                    string[] inputFiles = DiscoverFiles();
                    WriteLogLine("Найдено файлов: {0}", inputFiles.Length);
                    if (inputFiles.Length == 0)
                    {
                        WriteLogLine("Нет импортируемых файлов, завершаемся");
                        return;
                    }
                    Array.Sort(inputFiles);

                    using (Client = new IrbisConnection())
                    {
                        //using (Buffer = new RecordBuffer(Client, 100))
                        {
                            //Buffer.Actualize = true;
                            //Buffer.Database = Client.Database;
                            //Buffer.BatchWrite += Buffer_BatchWrite;

                            Client.ParseConnectionString(ConnectionString);
                            WriteLog("Подключение к серверу... ");
                            Client.Connect();
                            WriteLogLine("успешно");

                            Fst = FstFile.ParseServerFile
                                (
                                    Client,
                                    FstFileName
                                );
                            WriteLogLine("Файл FST содержит {0} строк", Fst.Lines.Count);
                            if (Fst.Lines.Count == 0)
                            {
                                WriteLogLine("Плохой файл FST, завершаемся");
                                return;
                            }

                            Magazines = LoadMagazines();
                            WriteLogLine
                                (
                                    "Сводных описаний импортируемых журналов в каталоге: {0}",
                                    Magazines.Count
                                );
                            if (Magazines.Count == 0)
                            {
                                WriteLogLine("Нет импортируемых журналов в каталоге");
                                return;
                            }

                            mfnBefore = Client.GetMaxMfn();

                            using (Queue = new QueueEngine<RecordTask>
                            (
                                Environment.ProcessorCount,
                                QueueLength
                            ))
                            {
                                //Queue.Waiting += Queue_Waiting;

                                int index = 0;
                                foreach (string inputFile in inputFiles)
                                {
                                    if (GoOn)
                                    {
                                        ProcessFile
                                            (
                                                ++index,
                                                inputFile
                                            );
                                        if (GoOn && DeleteProcessedFiles)
                                        {
                                            WriteLog("Удаляем файл... ");
                                            File.Delete(inputFile);
                                            WriteLogLine("удален");
                                        }
                                    } // if
                                } // foreach

                                WriteLogLine("Длина очереди: {0}", Queue.QueueLength);
                                WriteLog("Ожидаем завершения обработки записей в очереди: ");
                            } // using Queue

                            if (!GoOn)
                            {
                                WriteLogLine("Импорт прерван пользователем");
                            }

                            WriteLogLine("Отправка на сервер оставшихся записей");

                        } // using Buffer

                        mfnAfter = Client.GetMaxMfn();
                    } // using Client

                    WriteLogLine("Произведено корректное отключение от сервера");

                    TimeSpan elapsedTime = DateTime.Now - StartTime;
                    WriteLogLine
                        (
                            "Затрачено времени: {0}",
                            elapsedTime
                        );
                    WriteLogLine
                        (
                            "Импортировано файлов: {0}, записей: {1}",
                            FileCount,
                            RecordCount
                        );
                    WriteLogLine
                        (
                            "MFN до={0}, после={1}",
                            mfnBefore,
                            mfnAfter
                        );
                }
            }
            catch (Exception exception)
            {
                Log = null;
                WriteLogLine
                    (
                        "ВОЗНИКЛА ОШИБКА: {0}",
                        exception
                    );
                WriteLogLine("Аварийное завершение");
            }
        }

        //static void Queue_Waiting(object sender, EventArgs e)
        //{
        //    WriteLog
        //        (
        //            " [{0}] ", 
        //            Queue.QueueLength
        //        );
        //}

        //static void Buffer_BatchWrite(object sender, EventArgs e)
        //{
        //    WriteLog(" [Отсылка записей на сервер] ");
        //}

        // Обработка нажатия Control-Break и Control-C
        static void Console_CancelKeyPress
            (
                object sender,
                ConsoleCancelEventArgs e
            )
        {
            GoOn = false;
            e.Cancel = true;
        }

    }
}
