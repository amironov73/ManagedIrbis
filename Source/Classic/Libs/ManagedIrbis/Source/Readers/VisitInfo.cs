// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* VisitInfo.cs -- информация о посещении/выдаче
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Fields;
using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Информация о посещении/выдаче.
    /// </summary>
    [XmlRoot("visit")]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("{DateGivenString} {Index} {Description}")]
#endif
    public sealed class VisitInfo
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// подполе G, имя БД каталога.
        /// </summary>
        [CanBeNull]
        [SubField('g')]
        [XmlAttribute("database")]
        [JsonProperty("database")]
        public string Database { get; set; }

        /// <summary>
        /// подполе A, шифр документа.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("index")]
        [JsonProperty("index")]
        public string Index { get; set; }

        /// <summary>
        /// подполе B, инвентарный номер экземпляра
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("inventory")]
        [JsonProperty("inventory")]
        public string Inventory { get; set; }

        /// <summary>
        /// подполе H, штрих-код экземпляра.
        /// </summary>
        [CanBeNull]
        [SubField('h')]
        [XmlAttribute("barcode")]
        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        /// <summary>
        /// подполе K, место хранения экземпляра
        /// </summary>
        [CanBeNull]
        [SubField('k')]
        [XmlAttribute("sigla")]
        [JsonProperty("sigla")]
        public string Sigla { get; set; }

        /// <summary>
        /// подполе D, дата выдачи
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("dateGiven")]
        [JsonProperty("dateGiven")]
        public string DateGivenString { get; set; }

        /// <summary>
        /// подполе V, место выдачи
        /// </summary>
        [CanBeNull]
        [SubField('v')]
        [XmlAttribute("department")]
        [JsonProperty("department")]
        public string Department { get; set; }

        /// <summary>
        /// подполе E, дата предполагаемого возврата
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        [XmlAttribute("dateExpected")]
        [JsonProperty("dateExpected")]
        public string DateExpectedString { get; set; }

        /// <summary>
        /// подполе F, дата фактического возврата
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlAttribute("dateReturned")]
        [JsonProperty("dateReturned")]
        public string DateReturnedString { get; set; }

        /// <summary>
        /// подполе L, дата продления
        /// </summary>
        [CanBeNull]
        [SubField('l')]
        [XmlAttribute("dateProlong")]
        [JsonProperty("dateProlong")]
        public string DateProlongString { get; set; }

        /// <summary>
        /// подполе U, признак утерянной книги
        /// </summary>
        [CanBeNull]
        [SubField('u')]
        [XmlAttribute("lost")]
        [JsonProperty("lost")]
        public string Lost { get; set; }

        /// <summary>
        /// подполе C, краткое библиографическое описание
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// подполе I, ответственное лицо
        /// </summary>
        [CanBeNull]
        [SubField('i')]
        [XmlAttribute("responsible")]
        [JsonProperty("responsible")]
        public string Responsible { get; set; }

        /// <summary>
        /// подполе 1, время начала визита в библиотеку
        /// </summary>
        [CanBeNull]
        [SubField('1')]
        [XmlAttribute("timeIn")]
        [JsonProperty("timeIn")]
        public string TimeIn { get; set; }

        /// <summary>
        /// подполе 2, время окончания визита в библиотеку
        /// </summary>
        [CanBeNull]
        [SubField('2')]
        [XmlAttribute("timeOut")]
        [JsonProperty("timeOut")]
        public string TimeOut { get; set; }

        /// <summary>
        /// Не посещение ли?
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool IsVisit
        {
            get { return string.IsNullOrEmpty(Index); }
        }

        /// <summary>
        /// Возвращена ли книга?
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool IsReturned
        {
            get
            {
                if (string.IsNullOrEmpty(DateReturnedString))
                {
                    return false;
                }

                return !DateReturnedString.StartsWith("*");
            }
        }

        /// <summary>
        /// Дата выдачи/посещения.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime DateGiven
        {
            get
            {
                return IrbisDate.ConvertStringToDate
                    (
                        DateGivenString
                    );
            }
        }

        /// <summary>
        /// Дата возврата
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime DateReturned
        {
            get
            {
                return IrbisDate.ConvertStringToDate
                    (
                        DateReturnedString
                    );
            }
        }

        /// <summary>
        /// Ожидаемая дата возврата
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime DateExpected
        {
            get
            {
                return IrbisDate.ConvertStringToDate
                    (
                        DateExpectedString
                    );
            }
        }

        /// <summary>
        /// Год издания книги.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("year")]
        [JsonProperty("year")]
        public string Year { get; set; }

        /// <summary>
        /// Цена книги.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("price")]
        [JsonProperty("price")]
        public string Price { get; set; }

        /// <summary>
        /// Поле, в котором хранится посещение/выдача.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public RecordField Field { get; set; }

        /// <summary>
        /// Ссылка на читателя, сделавшего посещение.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public ReaderInfo Reader { get; set; }

        #endregion

        #region Private members

        // ReSharper disable once InconsistentNaming
        private static string FM
            (
                RecordField field,
                char code
            )
        {
            return field.GetFirstSubFieldValue(code);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get price for the book.
        /// </summary>
        [CanBeNull]
        public string GetBookPrice
            (
                [NotNull] MarcRecord bookRecord
            )
        {
            Code.NotNull(bookRecord, "bookRecord");

            RecordField[] fields = bookRecord.Fields
                .GetField("910");

            string result = null;

            foreach (RecordField field in fields)
            {
                ExemplarInfo exemplar = ExemplarInfo.Parse(field);

                if (!string.IsNullOrEmpty(Inventory))
                {
                    if (exemplar.Number.SameString(Inventory))
                    {
                        if (!string.IsNullOrEmpty(Barcode))
                        {
                            if (exemplar.Barcode.SameString(Barcode))
                            {
                                result = exemplar.Price;
                                break;
                            }
                        }
                        else
                        {
                            result = exemplar.Price;
                            break;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                result = bookRecord.FM("10", 'd');
            }

            return result;
        }

        /// <summary>
        /// Get year of the book.
        /// </summary>
        [CanBeNull]
        public static string GetBookYear
            (
                [NotNull] MarcRecord bookRecord
            )
        {
            Code.NotNull(bookRecord, "bookRecord");

            string result = bookRecord.FM("210", 'd')
                ?? bookRecord.FM("934");

            return result;
        }

        /// <summary>
        /// Parses the specified field.
        /// </summary>
        [NotNull]
        public static VisitInfo Parse
            (
                [NotNull] RecordField field
            )
        {
            // TODO Support for unknown subfields

            Code.NotNull(field, "field");

            VisitInfo result = new VisitInfo
            {
                Database = FM(field, 'g'),
                Index = FM(field, 'a'),
                Inventory = FM(field, 'b'),
                Barcode = FM(field, 'h'),
                Sigla = FM(field, 'k'),
                DateGivenString = FM(field, 'd'),
                Department = FM(field, 'v'),
                DateExpectedString = FM(field, 'e'),
                DateReturnedString = FM(field, 'f'),
                DateProlongString = FM(field, 'l'),
                Lost = FM(field, 'u'),
                Description = FM(field, 'c'),
                Responsible = FM(field, 'i'),
                TimeIn = FM(field, '1'),
                TimeOut = FM(field, '2'),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Формирование поля 40 
        /// из данных о выдаче/посещении.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField("40");
            result.AddNonEmptySubField('g', Database);
            result.AddNonEmptySubField('a', Index);
            result.AddNonEmptySubField('b', Inventory);
            result.AddNonEmptySubField('h', Barcode);
            result.AddNonEmptySubField('k', Sigla);
            result.AddNonEmptySubField('d', DateGivenString);
            result.AddNonEmptySubField('v', Department);
            result.AddNonEmptySubField('e', DateExpectedString);
            result.AddNonEmptySubField('f', DateReturnedString);
            result.AddNonEmptySubField('l', DateProlongString);
            result.AddNonEmptySubField('u', Lost);
            result.AddNonEmptySubField('c', Description);
            result.AddNonEmptySubField('i', Responsible);
            result.AddNonEmptySubField('1', TimeIn);
            result.AddNonEmptySubField('2', TimeOut);

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(Database);
            writer.WriteNullable(Index);
            writer.WriteNullable(Inventory);
            writer.WriteNullable(Barcode);
            writer.WriteNullable(Sigla);
            writer.WriteNullable(DateGivenString);
            writer.WriteNullable(Department);
            writer.WriteNullable(DateExpectedString);
            writer.WriteNullable(DateReturnedString);
            writer.WriteNullable(DateProlongString);
            writer.WriteNullable(Lost);
            writer.WriteNullable(Description);
            writer.WriteNullable(Responsible);
            writer.WriteNullable(TimeIn);
            writer.WriteNullable(TimeOut);
            writer.WriteNullable(Year);
            writer.WriteNullable(Price);
        }

        /// <summary>
        /// Сохранение в поток.
        /// </summary>
        public static void SaveToStream
            (
                [NotNull] BinaryWriter writer,
                [NotNull][ItemNotNull] VisitInfo[] visits
            )
        {
            writer.WritePackedInt32(visits.Length);
            foreach (VisitInfo visit in visits)
            {
                visit.SaveToStream(writer);
            }
        }

#if !SILVERLIGHT && !WIN81 && !PORTABLE

        /// <summary>
        /// Сохранение в файл.
        /// </summary>
        public static void SaveToFile
            (
                [NotNull] string fileName,
                [NotNull][ItemNotNull] VisitInfo[] visits
            )
        {
            visits.SaveToZipFile(fileName);
        }

#endif

        /// <inheritdoc />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Database = reader.ReadNullableString();
            Index = reader.ReadNullableString();
            Inventory = reader.ReadNullableString();
            Barcode = reader.ReadNullableString();
            Sigla = reader.ReadNullableString();
            DateGivenString = reader.ReadNullableString();
            Department = reader.ReadNullableString();
            DateExpectedString = reader.ReadNullableString();
            DateReturnedString = reader.ReadNullableString();
            DateProlongString = reader.ReadNullableString();
            Lost = reader.ReadNullableString();
            Description = reader.ReadNullableString();
            Responsible = reader.ReadNullableString();
            TimeIn = reader.ReadNullableString();
            TimeOut = reader.ReadNullableString();
            Year = reader.ReadNullableString();
            Price = reader.ReadNullableString();
        }

#if !SILVERLIGHT && !WIN81 && !PORTABLE

        /// <summary>
        /// Считывание из файла.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        public static VisitInfo[] ReadFromFile
            (
                [NotNull] string fileName
            )
        {
            VisitInfo[] result = SerializationUtility
                .RestoreArrayFromZipFile<VisitInfo>(fileName);

            return result;
        }

#endif

        #endregion

        #region Object members

        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result
                .AppendFormat("Посещение: \t\t\t{0}", IsVisit)
                .AppendLine()
                .AppendFormat("Описание: \t\t\t{0}", Description)
                .AppendLine()
                .AppendFormat("Шифр документа: \t\t{0}", Index)
                .AppendLine()
                .AppendFormat("Штрих-код: \t\t\t{0}", Barcode)
                .AppendLine()
                .AppendFormat("Место хранения: \t\t{0}", Sigla)
                .AppendLine()
                .AppendFormat("Дата выдачи: \t\t\t{0:d}", DateGiven)
                .AppendLine()
                .AppendFormat("Место выдачи: \t\t\t{0}", Department)
                .AppendLine()
                .AppendFormat("Ответственное лицо: \t\t{0}", Responsible)
                .AppendLine()
                .AppendFormat("Дата предполагаемого возврата: \t{0:d}", DateExpected)
                .AppendLine()
                .AppendFormat("Возвращена: \t\t\t{0}", IsReturned)
                .AppendLine()
                .AppendFormat("Дата возврата: \t\t\t{0:d}", DateReturned)
                .AppendLine()
                .AppendLine(new string('-', 60));

            return result.ToString();
        }

        #endregion
    }
}
