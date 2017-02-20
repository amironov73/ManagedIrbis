// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BookRequest.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Requests
{
    using Readers;

    /// <summary>
    /// Информация о читательском заказе.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("request")]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("{RequestDate} {BookDescription}")]
#endif
    public sealed class BookRequest
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// MFN записи с заказом.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonProperty("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Краткое описание заказанного издания.
        /// Поле 201.
        /// </summary>
        [CanBeNull]
        [Field("201")]
        [XmlAttribute("bookDescription")]
        [JsonProperty("bookDescription")]
        public string BookDescription { get; set; }

        /// <summary>
        /// Шифр заказанного издания.
        /// Поле 903.
        /// </summary>
        [CanBeNull]
        [Field("903")]
        [XmlAttribute("bookCode")]
        [JsonProperty("bookCode")]
        public string BookCode { get; set; }

        /// <summary>
        /// Дата создания заказа.
        /// Поле 40.
        /// </summary>
        [CanBeNull]
        [Field("40")]
        [XmlAttribute("requestDate")]
        [JsonProperty("requestDate")]
        public string RequestDate { get; set; }

        /// <summary>
        /// Дата выполнения заказа.
        /// </summary>
        [CanBeNull]
        [Field("41")]
        [XmlAttribute("fulfillmentDate")]
        [JsonProperty("fulfillmentDate")]
        public string FulfillmentDate { get; set; }

        /// <summary>
        /// Дата бронирования.
        /// </summary>
        [CanBeNull]
        [Field("43")]
        [XmlAttribute("reservationDate")]
        [JsonProperty("reservationDate")]
        public string ReservationDate { get; set; }

        /// <summary>
        /// Идентификатор читателя.
        /// Поле 30.
        /// </summary>
        [CanBeNull]
        [Field("30")]
        [XmlAttribute("readerID")]
        [JsonProperty("readerID")]
        // ReSharper disable once InconsistentNaming
        public string ReaderID { get; set; }

        /// <summary>
        /// Краткое описание читателя.
        /// Поле 31.
        /// </summary>
        [CanBeNull]
        [Field("31")]
        [XmlAttribute("readerDescription")]
        [JsonProperty("readerDescription")]
        public string ReaderDescription { get; set; }

        /// <summary>
        /// Имя БД электронного каталога.
        /// Поле 1.
        /// Как правило, IBIS.
        /// </summary>
        [CanBeNull]
        [Field("1")]
        [XmlAttribute("database")]
        [JsonProperty("database")]
        public string Database { get; set; }

        /// <summary>
        /// Сведения об отказе.
        /// Поле 44.
        /// Подполе A: причина отказа.
        /// Подполе B: дата.
        /// </summary>
        [CanBeNull]
        [Field("44")]
        [XmlAttribute("rejectInfo")]
        [JsonProperty("rejectInfo")]
        public string RejectInfo { get; set; }

        /// <summary>
        /// Место выдачи.
        /// Поле 102.
        /// Часто равно *.
        /// </summary>
        [CanBeNull]
        [Field("102")]
        [XmlAttribute("department")]
        [JsonProperty("department")]
        public string Department { get; set; }

        /// <summary>
        /// Ответственное лицо.
        /// Поле 50.
        /// Берется из логина.
        /// </summary>
        [CanBeNull]
        [Field("50")]
        [XmlAttribute("responsiblePerson")]
        [JsonProperty("responsiblePerson")]
        public string ResponsiblePerson { get; set; }

        /// <summary>
        /// Сведения о заказе можно удалять,
        /// т. к. он либо выполнен, либо отказан.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool CanBeDeleted
        {
            get
            {
                return !string.IsNullOrEmpty(FulfillmentDate)
                       || !string.IsNullOrEmpty(RejectInfo);
            }
        }

        /// <summary>
        /// Библиографическая запись о книге.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public MarcRecord BookRecord { get; set; }

        /// <summary>
        /// Сведения о читателе.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public ReaderInfo Reader { get; set; }

        /// <summary>
        /// Свободные инвентарные номера.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        [XmlIgnore]
        [JsonIgnore]
        public string[] FreeNumbers { get; set; }

        /// <summary>
        /// Свободные номера, предназначенные для данного АРМ.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        [XmlIgnore]
        [JsonIgnore]
        public string[] MyNumbers { get; set; }

        /// <summary>
        /// Запись, на осонове которой построен запрос
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public MarcRecord RequestRecord { get; set; }

        #endregion

        #region Private members

        private static void _AddField
            (
                MarcRecord record,
                string tag,
                string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                record.Fields.Add(new RecordField(tag, text));
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [NotNull]
        public static BookRequest Parse
            (
                [NotNull] MarcRecord record
            )
        {
            // TODO Support for unknown fields

            Code.NotNull(record, "record");

            BookRequest result = new BookRequest
                {
                    Mfn = record.Mfn,
                    BookDescription = record.FM("201"),
                    BookCode = record.FM("903"),
                    RequestDate = record.FM("40"),
                    FulfillmentDate = record.FM("41"),
                    ReservationDate = record.FM("43"),
                    ReaderID = record.FM("30"),
                    ReaderDescription = record.FM("31"),
                    Database = record.FM("1"),
                    Department = record.FM("102"),
                    ResponsiblePerson = record.FM("50"),
                    RequestRecord = record
                };

            RecordField field44 = record.Fields
                .GetField("44")
                .FirstOrDefault();
            if (!ReferenceEquals(field44, null))
            {
                result.RejectInfo = field44.ToText();
            }

            return result;
        }

        /// <summary>
        /// Кодируем запись.
        /// </summary>
        [NotNull]
        public MarcRecord Encode()
        {
            MarcRecord result = new MarcRecord
                {
                    Mfn = Mfn
                };

            _AddField(result, "201", BookDescription);
            _AddField(result, "903", BookCode);
            _AddField(result, "40", RequestDate);
            _AddField(result, "41", FulfillmentDate);
            _AddField(result, "43", ReservationDate);
            _AddField(result, "30", ReaderID);
            _AddField(result, "31", ReaderDescription);
            _AddField(result, "1", Database);
            _AddField(result, "44", RejectInfo);
            _AddField(result, "102", Department);
            _AddField(result, "50", ResponsiblePerson);

            return result;
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Mfn = reader.ReadPackedInt32();
            BookDescription = reader.ReadNullableString();
            BookCode = reader.ReadNullableString();
            RequestDate = reader.ReadNullableString();
            ReaderID = reader.ReadNullableString();
            ReaderDescription = reader.ReadNullableString();
            Database = reader.ReadNullableString();
            RejectInfo = reader.ReadNullableString();
            Department = reader.ReadNullableString();
            ResponsiblePerson = reader.ReadNullableString();
        }

        /// <inheritdoc />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32(Mfn)
                .WriteNullable(BookDescription)
                .WriteNullable(BookCode)
                .WriteNullable(RequestDate)
                .WriteNullable(ReaderID)
                .WriteNullable(ReaderDescription)
                .WriteNullable(Database)
                .WriteNullable(RejectInfo)
                .WriteNullable(Department)
                .WriteNullable(ResponsiblePerson);
        }

        #endregion

        #region Object members

        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat
                (
                    "Reader: {0}",
                    ReaderDescription.ToVisibleString()
                );
            result.AppendLine();
            result.AppendLine(BookDescription);
            result.AppendLine();
            if (FreeNumbers != null)
            {
                result.AppendFormat
                (
                    "Free exemplars: {0}",
                    string.Join(", ", FreeNumbers)
                );
                result.AppendLine();
            }
            if (MyNumbers != null)
            {
                result.AppendFormat
                    (
                        "My exemplars: {0}",
                        string.Join(", ", MyNumbers)
                    );
                result.AppendLine();
            }
            result.AppendFormat
                (
                    "Department: {0}",
                    Department.ToVisibleString()
                );
            result.AppendLine();

            return result.ToString();
        }

        #endregion
    }
}
