// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using AM;

using ManagedIrbis;
using ManagedIrbis.Batch;

using static System.Console;

using CM=System.Configuration.ConfigurationManager;

#endregion

// ReSharper disable LocalizableElement

namespace NewGost
{
    internal class Program
    {
        private static bool _stop;
        private static Stopwatch _stopwatch;
        private static IrbisConnection _connection;
        //private static BatchRecordWriter _writer;

        private static void Add203
            (
                MarcRecord record,
                string contentType,
                string access,
                string characteristics = null
            )
        {
            var field = new RecordField (203);
            field.AddSubField ('a', contentType);
            field.AddSubField ('c', access);
            if (!string.IsNullOrEmpty (characteristics))
            {
                field.AddSubField ('o', characteristics);
            }
            record.Fields.Add (field);

            _connection.WriteRecord(record);
            //_writer.Append (record);
            Write ('.');
        }

        private static void ProcessRecord
            (
                MarcRecord record
            )
        {
            if (record.Deleted)
            {
                // Это удаленная запись, игнорируем ее.
                Write ('/');
                return;
            }

            // Получаем имя рабочего листа
            var worksheet = record.FM (920);
            if (string.IsNullOrEmpty (worksheet))
            {
                // Если рабочего листа нет, не будем связываться с такой записью
                Write ('?');
                return;
            }

            if (worksheet.SameString("NJ")
                || worksheet.SameString("NJP")
                || worksheet.SameString("NJK"))
            {
                Write ('-');
                return;
            }

            var field203 = record.Fields.GetFirstField (203);
            if (!ReferenceEquals (field203, null))
            {
                // Уже есть поле 203, конвертировать не надо
                Write ('+');
                return;
            }

            var field900 = record.Fields.GetFirstField (900);
            if (ReferenceEquals (field900, null))
            {
                // Нет поля 900, похоже, это простой текст.
                // Заодно восстанавливаем поле 900.
                record.AddField
                    (
                        900,
                        new RecordField(900)
                            .AddSubField ('t', "a")
                    );
                Add203 (record, "Текст", "непосредственный");
                return;
            }

            // Получаем тип документа
            var documentType = field900.GetFirstSubFieldValue ('t');
            if (string.IsNullOrEmpty (documentType))
            {
                // Нет типа документа, похоже, это простой текст
                // Заодно восстанавливаем поле 900
                field900.AddSubField ('t', "a");
                Add203 (record, "Текст", "непосредственный");
                return;
            }

            documentType = documentType.ToLowerInvariant();
            switch (documentType)
            {
                // Разные виды текста:
                case "a":
                case "a1":
                case "a2":
                case "b":
                    Add203 (record, "Текст", "непосредственный");
                    return;

                // Ноты
                case "c":
                case "d":
                    Add203 (record, "Музыка",
                        "непосредственная", "знаковая");
                    return;

                // Карты
                case "e":
                case "f":
                    Add203 (record, "Изображение",
                        "непосредственное", "картографическое");
                    return;

                // Кино
                case "g":
                case "g1":
                case "g2":
                case "g3":
                    Add203 (record, "Изображение",
                        "видео", "движущееся");
                    return;

                // Звукозаписи немузыкальные
                case "i":
                    Add203 (record, "Звуки",
                        "аудио");
                    return;

                // Звукозаписи музыкальные
                case "j":
                    Add203 (record, "Музыка",
                        "аудио", "исполнительская");
                    return;

                // Графика
                case "k":
                    Add203 (record, "Изображение",
                        "непосредственное");
                    return;

                // Компьютерные файлы
                case "l":
                    Add203 (record, "Электронные данные",
                        "электронные");
                    return;

                // Мультимедиа
                case "m":
                case "m2":
                    Add203 (record, "Текст",
                        "электронный");
                    return;

                // Трехмерные объекты
                case "r":
                    Add203 (record, "Предмет",
                        "непосредственный", "трехмерный");
                    return;

                // Шрифты Брайля и т. д.
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                    Add203 (record, "Текст",
                        "непосредственный", "тактильный");
                    return;

                // Непонятный тип документа
                default:
                    Write ('U');
                    return;
            }

            // Мы обработали запись, но ничего не сделали
        }

        private static void ProcessAll()
        {
            var all = BatchRecordReader.WholeDatabase
                (
                    _connection,
                    _connection.Database,
                    500
                );
            var index = 0;
            try
            {
                foreach (var record in all)
                {
                    if (_stop)
                    {
                        // Пользователь нажал Ctrl-C, завершаемся
                        break;
                    }

                    ++index;
                    if (index % 50 == 1)
                    {
                        WriteLine();
                        var elapsed = _stopwatch.Elapsed.ToHourString();
                        var minutes = _stopwatch.Elapsed.TotalMinutes;
                        var speed = 0.0;
                        if (minutes > 1)
                        {
                            speed = (index - 1) / minutes;
                        }
                        Write($"{elapsed} | {speed:F} | {index - 1:0000000}> ");
                    }

                    if (index % 10 == 1)
                    {
                        Write(' ');
                    }

                    ProcessRecord(record);
                }
            }
            catch (Exception exception)
            {
                var irbisException = exception as IrbisException;
                if (!ReferenceEquals(irbisException, null))
                {
                    var attachments = irbisException.ListAttachments();
                    foreach (var attachment in attachments)
                    {
                        File.WriteAllBytes(attachment.Name, attachment.Content);
                    }
                }
                WriteLine (exception);
            }

            //_writer.Flush();
        }

        public static void Main (string[] args)
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            CancelKeyPress += (sender, eventArgs) =>
            {
                WriteLine();
                WriteLine ("Break pressed");
                eventArgs.Cancel = true;
                _stop = true;
            };

            var connectionString = CM.AppSettings["connection-string"];
            using (_connection = new IrbisConnection(connectionString))
            {
                // _writer = new BatchRecordWriter
                //     (
                //         _connection,
                //         _connection.Database,
                //         200
                //     );
                ProcessAll();
            }

            _stopwatch.Stop();
            var elapsed = _stopwatch.Elapsed;
            WriteLine();
            WriteLine ($"Elapsed={elapsed.ToAutoString()}");
        }
    }
}
