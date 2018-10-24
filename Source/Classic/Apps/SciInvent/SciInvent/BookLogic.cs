// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BookLogic.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM;
using AM.Configuration;

using BLToolkit.Data;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;

using JetBrains.Annotations;

using ManagedIrbis;

#endregion

// ReSharper disable StringLiteralTypo

namespace SciInvent
{
    /// <summary>
    /// Информация о подсобном фонде.
    /// </summary>
    [PublicAPI]
    [TableName("podsob")]
    public class PodsobRecord
        : EditableObject
    {
        #region Properties

        ///<summary>
        /// Инвентарный номер книги.
        ///</summary>
        [PrimaryKey]
        [MapField("INVENT")]
        public long Inventory { get; set; }

        ///<summary>
        /// Номер читательского билета.
        ///</summary>
        [MapField("CHB")]
        public string Ticket { get; set; }

        ///<summary>
        /// Дополнительная информация о читателе.
        ///</summary>
        [MapField("IDENT")]
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Момент выдачи.
        /// </summary>
        [MapField("WHE")]
        public DateTime Moment { get; set; }

        ///<summary>
        /// Табельный номер оператора.
        ///</summary>
        [MapField("operator")]
        public int Operator { get; set; }

        ///<summary>
        /// Предполагаемый срок возврата.
        ///</summary>
        [MapField("srok")]
        public DateTime Deadline { get; set; }

        ///<summary>
        /// Количество продлений.
        ///</summary>
        [MapField("prodlen")]
        public int Prolongation { get; set; }

        /// <summary>
        /// На руках у читателя.
        /// </summary>
        [MapField("onhand")]
        public string OnHand { get; set; }

        /// <summary>
        /// Примечания об экземпляре книги.
        /// </summary>
        [MapField("alert")]
        public string Alert { get; set; }

        /// <summary>
        /// Контрольный экземпляр.
        /// </summary>
        [MapField("pilot")]
        public char Pilot { get; set; }

        /// <summary>
        /// Место хранения ЦОР, ЦНИ и т. д.
        /// </summary>
        [MapField("sigla")]
        public string Sigla { get; set; }

        /// <summary>
        /// Дата инвентаризации.
        /// </summary>
        [MapField("seen")]
        public DateTime? Seen { get; set; }

        /// <summary>
        /// Библиографическое описание.
        /// </summary>
        [MapIgnore]
        public string Description { get; set; }

        /// <summary>
        /// MARC record.
        /// </summary>
        [CanBeNull]
        [MapIgnore]
        public MarcRecord Record { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Inventory.ToInvariantString();
        }

        #endregion
    }

    /// <summary>
    /// Трансляция штрих-кодов в инвентарные номера
    /// научного фонда.
    /// </summary>
    [PublicAPI]
    [TableName("translator")]
    public class TranslatorRecord
        : EditableObject
    {
        #region Properties

        /// <summary>
        /// Инвентарный номер.
        /// </summary>
        [MapField("invnum")]
        public int Inventory { get; set; }

        /// <summary>
        /// Штрих-код.
        /// </summary>
        [MapField("barcode")]
        public string Barcode { get; set; }

        /// <summary>
        /// Момент приписки
        /// </summary>
        [MapField("whn")]
        public DateTime Moment { get; set; }

        /// <summary>
        /// Взято на обработку.
        /// </summary>
        [MapField("taken")]
        public bool Taken { get; set; }

        /// <summary>
        /// Дополнительная информация об экземпляре.
        /// </summary>
        [MapField("info")]
        public string Info { get; set; }

        /// <summary>
        /// Табельный номер оператора.
        /// </summary>
        [MapField("operator")]
        public int Operator { get; set; }

        /// <summary>
        /// Контрольный экземпляр.
        /// </summary>
        [MapField("pilot")]
        public bool Pilot { get; set; }

        /// <summary>
        /// RFID.
        /// </summary>
        [MapField("rfid")]
        public string Rfid { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Inventory.ToInvariantString();
        }

        #endregion
    }

    [PublicAPI]
    [TableName("uchtrans")]
    public class UchRecord
        : EditableObject
    {
        #region Properties

        [PrimaryKey]
        [MapField("barcode")]
        public string Barcode { get; set; }

        [MapField("cardnum")]
        public string Cardnum { get; set; }

        [MapField("whn")]
        public DateTime? Moment { get; set; }

        [MapField("operator")]
        public int OperatorId { get; set; }

        [MapField("chb")]
        public string Ticket { get; set; }

        [MapField("invnum")]
        public string Number { get; set; }

        [MapField("price")]
        public decimal Price { get; set; }

        [MapField("prodlen")]
        public int Prolong { get; set; }

        [MapField("srok")]
        public DateTime? Deadline { get; set; }

        [MapField("operator2")]
        public string Operator2 { get; set; }

        [MapField("machine")]
        public string Machine { get; set; }

        [MapField("onhand")]
        public string OnHand { get; set; }

        [MapField("alert")]
        public string Alert { get; set; }

        [MapField("seen")]
        public DateTime? Seen { get; set; }

        [MapField("seenby")]
        public int SeenBy { get; set; }

        [MapField("sigla")]
        public string Sigla { get; set; }

        #endregion
    }

    public static class BookLogic
    {
        private static string ExpectedPlace = "Ж";

        private static IrbisConnection Irbis { get; set; }

        private static DbManager Db { get; set; }

        public static MainWindow LogWindow { private get; set; }

        private static void Clear()
        {
            LogWindow.Dispatcher.Invoke
                (
                    () =>
                    {
                        LogWindow.Clear();
                    }
                );
        }

        private static void WriteLine
            (
                string format,
                params object[] args
            )
        {
            LogWindow.Dispatcher.Invoke
                (
                    () =>
                    {
                        LogWindow.WriteLine(format, args);
                    }
                );
        }

        public static void Connect()
        {
            Db = new DbManager("mssql");
            string connectionString = ConfigurationUtility
                .GetString("ConnectionString.irbis")
                .ThrowIfNull("ConnectionString.irbis = null");
            Irbis = new IrbisConnection(connectionString);
        }

        private static bool ResolveByInventory
            (
                PodsobRecord book
            )
        {
            MarcRecord record = Irbis
                .SearchReadOneRecord("\"IN={0}\"", book.Inventory);
            if (ReferenceEquals(record, null))
            {
                return false;
            }

            book.Record = record;
            book.Description = Irbis.FormatRecord("@sbrief", record.Mfn);

            return true;
        }

        private static RecordField FindBarcode
            (
                MarcRecord record,
                string barcode
            )
        {
            foreach (RecordField field in record.Fields.GetField(910))
            {
                if (field.GetFirstSubFieldValue('h').SameString(barcode))
                {
                    return field;
                }
            }

            return null;
        }

        private static void MarkBook
            (
                string barcode
            )
        {
            string inventory = null;
            RecordField field = null;
            MarcRecord record = Irbis.SearchReadOneRecord("\"BAR={0}\"", barcode);
            Table<TranslatorRecord> translator = Db.GetTable<TranslatorRecord>();
            TranslatorRecord found = translator.FirstOrDefault(b => b.Barcode == barcode);
            if (!ReferenceEquals(found, null))
            {
                inventory = found.Inventory.ToInvariantString();
            }
            else
            {
                if (!ReferenceEquals(record, null))
                {
                    field = FindBarcode(record, barcode);
                    if (ReferenceEquals(field, null))
                    {
                        WriteLine("Не удалось найти поле для штрих-кода " + barcode);
                        return;
                    }

                    string actualPlace = field.GetFirstSubFieldValue('d');
                    if (!ExpectedPlace.SameString(actualPlace))
                    {
                        WriteLine
                            (
                                "Экземпляр {0}: место хранения (ИРБИС) '{1}'",
                                barcode,
                                actualPlace
                            );
                        return;
                    }

                    inventory = field.GetFirstSubFieldValue('b');
                }
            }

            if (string.IsNullOrEmpty(inventory))
            {
                WriteLine("Неизвестный штрих-код: " + barcode);
                return;
            }

            Table<PodsobRecord> podsob = Db.GetTable<PodsobRecord>();
            long longInventory = inventory.SafeToInt64();
            PodsobRecord book = podsob
                .FirstOrDefault(p => p.Inventory == longInventory);
            if (!ReferenceEquals(book, null))
            {
                string actualPlace = book.Ticket;
                if (!ExpectedPlace.SameString(actualPlace))
                {
                    WriteLine
                        (
                            "Экземпляр {0}: место хранения (SQL) '{1}'",
                            barcode,
                            actualPlace
                        );
                    return;
                }

                string onHand = book.OnHand;
                if (!string.IsNullOrEmpty(onHand))
                {
                    WriteLine
                        (
                            "Экземпляр {0}: находится на руках '{1}'",
                            barcode,
                            onHand
                        );
                    return;
                }

                book.Seen = DateTime.Now;
                Db.Update(book);
            }

            if (!ReferenceEquals(record, null) && !ReferenceEquals(field, null))
            {
                field.SetSubField('s', IrbisDate.TodayText);
                field.SetSubField('!', ExpectedPlace);
                Irbis.WriteRecord(record);
            }
        }

        public static void MarkBooks
            (
                string[] barcodes
            )
        {
            WriteLine("Обработка началась");

            try
            {
                foreach (string barcode in barcodes)
                {
                    WriteLine(barcode);

                    MarkBook(barcode);
                }
            }
            catch (Exception exception)
            {
                WriteLine(exception.ToString());
            }

            WriteLine("Обработка завершена");
        }

        public static void ListGoodBooks()
        {
            try
            {
                Clear();
                Table<PodsobRecord> podsob = Db.GetTable<PodsobRecord>();
                PodsobRecord[] seenBooks = podsob
                    .Where(r => r.Ticket == ExpectedPlace && r.Seen != null)
                    .ToArray();
                WriteLine("Всего книг: {0}", seenBooks.Length);
                List<PodsobRecord> bookList
                    = new List<PodsobRecord>(seenBooks.Length);
                WriteLine("Обработка началась");
                for (int i = 0; i < seenBooks.Length; i++)
                {
                    if (i % 100 == 0)
                    {
                        WriteLine("Обработано: {0}", i);
                    }

                    PodsobRecord book = seenBooks[i];
                    if (!ResolveByInventory(book))
                    {
                        WriteLine("Неизвестный номер {0}", book.Inventory);
                    }
                    else
                    {
                        bookList.Add(book);
                    }
                }

                string fileName = "good.csv";
                WriteLine("Запись файла " + fileName);
                CreateBookList(fileName, "Проверенные книги", bookList);
                WriteLine("Обработка завершена");
            }
            catch (Exception exception)
            {
                WriteLine(exception.ToString());
            }
        }

        private static void CreateBookList
            (
                string fileName,
                string title,
                IEnumerable<PodsobRecord> books
            )
        {
            using (TextWriter writer = File.CreateText(fileName))
            {
                writer.WriteLine(title);
                writer.WriteLine();

                foreach (PodsobRecord book in books)
                {
                    writer.WriteLine
                        (
                            "{0};{1}",
                            book.Inventory,
                            book.Description
                        );
                }
            }
        }

        public static void ListMissingBooks()
        {
            try
            {
                Clear();
                Table<PodsobRecord> podsob = Db.GetTable<PodsobRecord>();
                PodsobRecord[] missingBooks = podsob
                    .Where(r => r.Ticket == ExpectedPlace && r.Seen == null)
                    .ToArray();
                WriteLine("Всего книг: {0}", missingBooks.Length);
                List<PodsobRecord> bookList
                    = new List<PodsobRecord>(missingBooks.Length);
                WriteLine("Обработка началась");
                for (int i = 0; i < missingBooks.Length; i++)
                {
                    if (i % 100 == 0)
                    {
                        WriteLine("Обработано: {0}", i);
                    }

                    PodsobRecord book = missingBooks[i];
                    if (!ResolveByInventory(book))
                    {
                        WriteLine("Неизвестный номер {0}", book.Inventory);
                    }
                    else
                    {
                        bookList.Add(book);
                    }
                }

                string fileName = "missing.csv";
                WriteLine("Запись файла " + fileName);
                CreateBookList(fileName, "Отсутствующие книги", bookList);
                WriteLine("Обработка завершена");
            }
            catch (Exception exception)
            {
                WriteLine(exception.ToString());
            }
        }

        public static void Disconnect()
        {
            Irbis.Dispose();
            Db.Dispose();
        }
    }
}
