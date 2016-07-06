/* BookRequest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Requests
{
    using Readers;

    /// <summary>
    /// Информация о читательском заказе.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [DebuggerDisplay("Date={RequestDate} Description={BookDescription}")]
    public sealed class BookRequest
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// MFN записи с заказом.
        /// </summary>
        [JsonProperty("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Краткое описание заказанного издания.
        /// Поле 201.
        /// </summary>
        [CanBeNull]
        [JsonProperty("book-description")]
        public string BookDescription { get; set; }

        /// <summary>
        /// Шифр заказанного издания.
        /// Поле 903.
        /// </summary>
        [CanBeNull]
        [JsonProperty("book-code")]
        public string BookCode { get; set; }

        /// <summary>
        /// Дата создания заказа.
        /// Поле 40.
        /// </summary>
        [CanBeNull]
        [JsonProperty("request-date")]
        public string RequestDate { get; set; }

        /// <summary>
        /// Идентификатор читателя.
        /// Поле 30.
        /// </summary>
        [CanBeNull]
        [JsonProperty("reader-id")]
        public string ReaderId { get; set; }

        /// <summary>
        /// Краткое описание читателя.
        /// Поле 31.
        /// </summary>
        [CanBeNull]
        [JsonProperty("reader-description")]
        public string ReaderDescription { get; set; }

        /// <summary>
        /// Имя БД электронного каталога.
        /// Поле 1.
        /// Как правило, IBIS.
        /// </summary>
        [CanBeNull]
        [JsonProperty("database")]
        public string Database { get; set; }

        /// <summary>
        /// Сведения об отказе.
        /// Поле 44.
        /// Подполе A: причина отказа.
        /// Подполе B: дата.
        /// </summary>
        [CanBeNull]
        [JsonProperty("reject-info")]
        public string RejectInfo { get; set; }

        /// <summary>
        /// Место выдачи.
        /// Поле 102.
        /// Часто равно *.
        /// </summary>
        [CanBeNull]
        [JsonProperty("place")]
        public string Place { get; set; }

        /// <summary>
        /// Ответственное лицо.
        /// Поле 50.
        /// Берется из логина.
        /// </summary>
        [CanBeNull]
        [JsonProperty("responsible-person")]
        public string ResponsiblePerson { get; set; }

        /// <summary>
        /// Библиографическая запись о книге.
        /// </summary>
        [CanBeNull]
        [JsonIgnore]
        public IrbisRecord BookRecord { get; set; }

        /// <summary>
        /// Сведения о читателе.
        /// </summary>
        [CanBeNull]
        [JsonIgnore]
        public ReaderInfo Reader { get; set; }

        /// <summary>
        /// Свободные инвентарные номера.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        [JsonIgnore]
        public string[] FreeNumbers { get; set; }

        /// <summary>
        /// Свободные номера, предназначенные для данного АРМ.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        [JsonIgnore]
        public string[] MyNumbers { get; set; }

        /// <summary>
        /// Запись, на осонове которой построен запрос
        /// </summary>
        [CanBeNull]
        [JsonIgnore]
        public IrbisRecord RequestRecord { get; set; }

        #endregion

        #region Private members

        private static void _AddField
            (
                IrbisRecord record,
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
                [NotNull] IrbisRecord record
            )
        {
            Code.NotNull(record, "record");

            BookRequest result = new BookRequest
                {
                    Mfn = record.Mfn,
                    BookDescription = record.FM("201"),
                    BookCode = record.FM("903"),
                    RequestDate = record.FM("40"),
                    ReaderId = record.FM("30"),
                    ReaderDescription = record.FM("31"),
                    Database = record.FM("1"),
                    RejectInfo = record.FM("44"),
                    Place = record.FM("102"),
                    ResponsiblePerson = record.FM("50"),
                    RequestRecord = record
                };

            return result;
        }

        /// <summary>
        /// Кодируем запись.
        /// </summary>
        [NotNull]
        public IrbisRecord Encode()
        {
            IrbisRecord result = new IrbisRecord
                {
                    Mfn = Mfn
                };

            _AddField(result, "201", BookDescription);
            _AddField(result, "903", BookCode);
            _AddField(result, "40", RequestDate);
            _AddField(result, "30", ReaderId);
            _AddField(result, "31", ReaderDescription);
            _AddField(result, "1", Database);
            _AddField(result, "44", RejectInfo);
            _AddField(result, "102", Place);
            _AddField(result, "50", ResponsiblePerson);

            return result;
        }

        #endregion

        #region IHandmadeSerializable

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Mfn = reader.ReadPackedInt32();
            BookDescription = reader.ReadNullableString();
            BookCode = reader.ReadNullableString();
            RequestDate = reader.ReadNullableString();
            ReaderId = reader.ReadNullableString();
            ReaderDescription = reader.ReadNullableString();
            Database = reader.ReadNullableString();
            RejectInfo = reader.ReadNullableString();
            Place = reader.ReadNullableString();
            ResponsiblePerson = reader.ReadNullableString();
        }

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
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
                .WriteNullable(ReaderId)
                .WriteNullable(ReaderDescription)
                .WriteNullable(Database)
                .WriteNullable(RejectInfo)
                .WriteNullable(Place)
                .WriteNullable(ResponsiblePerson);
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" />
        /// that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat
                (
                    "Читатель: {0}",
                    ReaderDescription.ToVisibleString()
                );
            result.AppendLine();
            result.AppendLine(BookDescription);
            result.AppendLine();
            if (FreeNumbers != null)
            {
                result.AppendFormat
                (
                    "Свободные экземпляры: {0}",
                    string.Join(", ", FreeNumbers)
                );
                result.AppendLine();
            }
            if (MyNumbers != null)
            {
                result.AppendFormat
                    (
                        "Мои экземпляры: {0}",
                        string.Join(", ", MyNumbers)
                    );
                result.AppendLine();
            }
            result.AppendFormat
                (
                    "Место выдачи: {0}",
                    Place.ToVisibleString()
                );
            result.AppendLine();

            return result.ToString();
        }

        #endregion
    }
}
