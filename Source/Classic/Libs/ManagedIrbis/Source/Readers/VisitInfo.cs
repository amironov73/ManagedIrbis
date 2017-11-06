// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* VisitInfo.cs -- информация о посещении/выдаче
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
    [DebuggerDisplay("{DateGivenString} {Index} {Description}")]
    public sealed class VisitInfo
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Known codes.
        /// </summary>
        public const string KnownCodes = "124abcdefghikluv";

        #endregion

        #region Properties

        /// <summary>
        /// Identifier for LiteDB.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public int Id { get; set; }

        /// <summary>
        /// подполе G, имя БД каталога.
        /// </summary>
        [CanBeNull]
        [SubField('g')]
        [XmlAttribute("database")]
        [JsonProperty("database", NullValueHandling = NullValueHandling.Ignore)]
        public string Database { get; set; }

        /// <summary>
        /// подполе A, шифр документа.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("index")]
        [JsonProperty("index", NullValueHandling = NullValueHandling.Ignore)]
        public string Index { get; set; }

        /// <summary>
        /// подполе B, инвентарный номер экземпляра
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("inventory")]
        [JsonProperty("inventory", NullValueHandling = NullValueHandling.Ignore)]
        public string Inventory { get; set; }

        /// <summary>
        /// подполе H, штрих-код экземпляра.
        /// </summary>
        [CanBeNull]
        [SubField('h')]
        [XmlAttribute("barcode")]
        [JsonProperty("barcode", NullValueHandling = NullValueHandling.Ignore)]
        public string Barcode { get; set; }

        /// <summary>
        /// подполе K, место хранения экземпляра
        /// </summary>
        [CanBeNull]
        [SubField('k')]
        [XmlAttribute("sigla")]
        [JsonProperty("sigla", NullValueHandling = NullValueHandling.Ignore)]
        public string Sigla { get; set; }

        /// <summary>
        /// подполе D, дата выдачи
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("dateGiven")]
        [JsonProperty("dateGiven", NullValueHandling = NullValueHandling.Ignore)]
        public string DateGivenString { get; set; }

        /// <summary>
        /// подполе V, место выдачи
        /// </summary>
        [CanBeNull]
        [SubField('v')]
        [XmlAttribute("department")]
        [JsonProperty("department", NullValueHandling = NullValueHandling.Ignore)]
        public string Department { get; set; }

        /// <summary>
        /// подполе E, дата предполагаемого возврата
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        [XmlAttribute("dateExpected")]
        [JsonProperty("dateExpected", NullValueHandling = NullValueHandling.Ignore)]
        public string DateExpectedString { get; set; }

        /// <summary>
        /// подполе F, дата фактического возврата
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlAttribute("dateReturned")]
        [JsonProperty("dateReturned", NullValueHandling = NullValueHandling.Ignore)]
        public string DateReturnedString { get; set; }

        /// <summary>
        /// подполе L, дата продления
        /// </summary>
        [CanBeNull]
        [SubField('l')]
        [XmlAttribute("dateProlong")]
        [JsonProperty("dateProlong", NullValueHandling = NullValueHandling.Ignore)]
        public string DateProlongString { get; set; }

        /// <summary>
        /// подполе U, признак утерянной книги
        /// </summary>
        [CanBeNull]
        [SubField('u')]
        [XmlAttribute("lost")]
        [JsonProperty("lost", NullValueHandling = NullValueHandling.Ignore)]
        public string Lost { get; set; }

        /// <summary>
        /// подполе C, краткое библиографическое описание
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("description")]
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// подполе I, ответственное лицо
        /// </summary>
        [CanBeNull]
        [SubField('i')]
        [XmlAttribute("responsible")]
        [JsonProperty("responsible", NullValueHandling = NullValueHandling.Ignore)]
        public string Responsible { get; set; }

        /// <summary>
        /// подполе 1, время начала визита в библиотеку
        /// </summary>
        [CanBeNull]
        [SubField('1')]
        [XmlAttribute("timeIn")]
        [JsonProperty("timeIn", NullValueHandling = NullValueHandling.Ignore)]
        public string TimeIn { get; set; }

        /// <summary>
        /// подполе 2, время окончания визита в библиотеку
        /// </summary>
        [CanBeNull]
        [SubField('2')]
        [XmlAttribute("timeOut")]
        [JsonProperty("timeOut", NullValueHandling = NullValueHandling.Ignore)]
        public string TimeOut { get; set; }

        /// <summary>
        /// Счетчик продлений.
        /// </summary>
        /// <remarks>
        /// http://irbis.gpntb.ru/read.php?3,105310,108175#msg-108175
        /// </remarks>
        [CanBeNull]
        [SubField('4')]
        [XmlAttribute("prolong")]
        [JsonProperty("prolong", NullValueHandling = NullValueHandling.Ignore)]
        public string Prolong { get; set; }

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
        /// Счетчик продлений.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public int ProlongCount
        {
            get { return Prolong.SafeToInt32(); }
            set { Prolong = value.ToInvariantString(); }
        }

        /// <summary>
        /// Год издания книги.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("year")]
        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public string Year { get; set; }

        /// <summary>
        /// Цена книги.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("price")]
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public string Price { get; set; }

        /// <summary>
        /// Поле, в котором хранится посещение/выдача.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public RecordField Field { get; set; }

        /// <summary>
        /// Ссылка на читателя, сделавшего посещение.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public ReaderInfo Reader { get; set; }

        /// <summary>
        /// Unknown subfields.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        [XmlElement("unknown")]
        [JsonProperty("unknown", NullValueHandling = NullValueHandling.Ignore)]
        [Browsable(false)]
        public SubField[] UnknownSubFields { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object UserData { get; set; }

        #endregion

        #region Private members

        // ReSharper disable once InconsistentNaming
        private string FM
            (
                char code
            )
        {
            // ReSharper disable once PossibleNullReferenceException
            return Field.GetFirstSubFieldValue(code);
        }

        private void _Parse()
        {
            Database = FM('g');
            Index = FM('a');
            Inventory = FM('b');
            Barcode = FM('h');
            Sigla = FM('k');
            DateGivenString = FM('d');
            Department = FM('v');
            DateExpectedString = FM('e');
            DateReturnedString = FM('f');
            DateProlongString = FM('l');
            Lost = FM('u');
            Description = FM('c');
            Responsible = FM('i');
            TimeIn = FM('1');
            TimeOut = FM('2');
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
                .GetField(910);

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
                        result = exemplar.Price;
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                result = bookRecord.FM(10, 'd');
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

            string result = bookRecord.FM(210, 'd')
                ?? bookRecord.FM(934);

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
                Field = field,
                UnknownSubFields = field.SubFields.GetUnknownSubFields(KnownCodes)
            };
            result._Parse();

            return result;
        }

        /// <summary>
        /// Формирование поля 40 
        /// из данных о выдаче/посещении.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField("40")
                .AddNonEmptySubField('g', Database)
                .AddNonEmptySubField('a', Index)
                .AddNonEmptySubField('b', Inventory)
                .AddNonEmptySubField('h', Barcode)
                .AddNonEmptySubField('k', Sigla)
                .AddNonEmptySubField('d', DateGivenString)
                .AddNonEmptySubField('v', Department)
                .AddNonEmptySubField('e', DateExpectedString)
                .AddNonEmptySubField('f', DateReturnedString)
                .AddNonEmptySubField('l', DateProlongString)
                .AddNonEmptySubField('u', Lost)
                .AddNonEmptySubField('c', Description)
                .AddNonEmptySubField('i', Responsible)
                .AddNonEmptySubField('1', TimeIn)
                .AddNonEmptySubField('2', TimeOut);

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WritePackedInt32(Id);
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
            writer.WriteNullable(Prolong);
            writer.WriteNullable(Year);
            writer.WriteNullable(Price);
            writer.WriteNullableArray(UnknownSubFields);
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


        /// <summary>
        /// Сохранение в файл.
        /// </summary>
        public static void SaveToFile
            (
                [NotNull] string fileName,
                [NotNull][ItemNotNull] VisitInfo[] visits
            )
        {
#if SILVERLIGHT || WIN81 || PORTABLE

            throw new NotImplementedException();

#else

            visits.SaveToFile(fileName);

#endif
        }


        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Id = reader.ReadPackedInt32();
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
            Prolong = reader.ReadNullableString();
            Year = reader.ReadNullableString();
            Price = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();
        }


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
#if SILVERLIGHT || WIN81 || PORTABLE

            throw new NotImplementedException();

#else

            VisitInfo[] result = SerializationUtility
                .RestoreArrayFromFile<VisitInfo>(fileName);

            return result;

#endif
        }


        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result
                .AppendFormat("Посещение: \t\t\t{0}", IsVisit)
                .AppendLine()
                .AppendFormat("Описание: \t\t\t{0}", Description.ToVisibleString())
                .AppendLine()
                .AppendFormat("Шифр документа: \t\t{0}", Index.ToVisibleString())
                .AppendLine()
                .AppendFormat("Штрих-код: \t\t\t{0}", Barcode.ToVisibleString())
                .AppendLine()
                .AppendFormat("Место хранения: \t\t{0}", Sigla.ToVisibleString())
                .AppendLine()
                .AppendFormat("Дата выдачи: \t\t\t{0:d}", DateGiven)
                .AppendLine()
                .AppendFormat("Место выдачи: \t\t\t{0}", Department.ToVisibleString())
                .AppendLine()
                .AppendFormat("Ответственное лицо: \t\t{0}", Responsible.ToVisibleString())
                .AppendLine()
                .AppendFormat("Дата предполагаемого возврата: \t{0:d}", DateExpected)
                .AppendLine()
                .AppendFormat("Возвращена: \t\t\t{0}", IsReturned)
                .AppendLine()
                .AppendFormat("Дата возврата: \t\t\t{0:d}", DateReturned)
                .AppendLine()
                .AppendFormat("Счетчик продлений: \t\t\t{0}", Prolong.ToVisibleString())
                .AppendLine()
                .AppendLine(new string('-', 60));

            return result.ToString();
        }

        #endregion
    }
}
