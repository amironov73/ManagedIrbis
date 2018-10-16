using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using AM;
using AM.Configuration;

using BLToolkit.Data;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using JetBrains.Annotations;

using ManagedIrbis;


// ReSharper disable StringLiteralTypo

namespace SciInvent
{
    /// <summary>
    /// Информация о подсобном фонде.
    /// </summary>
    [PublicAPI]
    [TableName("podsob")]
    public class PodsobRecord
    {
        #region Properties

        ///<summary>
        /// Инвентарный номер книги.
        ///</summary>
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
        public static IrbisConnection Irbis { get; set; }

        public static DbManager Db { get; set; }

        public static MainWindow LogWindow { get; set; }

        public static void Clear()
        {
            LogWindow.Dispatcher.Invoke
                (
                    () =>
                    {
                        LogWindow.Clear();
                    }
                );
        }

        public static void WriteLine
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

        public static bool ResolveByInventory(PodsobRecord book)
        {
            MarcRecord record = Irbis.SearchReadOneRecord("\"IN={0}\"", book.Inventory);
            if (ReferenceEquals(record, null))
            {
                return false;
            }

            book.Record = record;
            book.Description = Irbis.FormatRecord("@sbrief", record.Mfn);

            return true;
        }

        public static string ListMissingBooks()
        {
            Clear();
            Table<PodsobRecord> podsob = Db.GetTable<PodsobRecord>();
            PodsobRecord[] missingBooks = podsob
                .Where(r => r.Ticket == "Ж" && r.Seen == null)
                .ToArray();
            WriteLine("Всего книг: {0}", missingBooks.Length);
            missingBooks = missingBooks.Take(200).ToArray();
            for (int i = 0; i < missingBooks.Length; i++)
            {
                if (i % 100 == 0)
                {
                    WriteLine("Обработано: {0}", i);
                }

                if (!ResolveByInventory(missingBooks[i]))
                {
                    WriteLine("Неизвестный номер {0}", missingBooks[i].Inventory);
                }
            }

            return null;
        }

        public static void Disconnect()
        {
            Irbis.Dispose();
            Db.Dispose();
        }
    }
}
