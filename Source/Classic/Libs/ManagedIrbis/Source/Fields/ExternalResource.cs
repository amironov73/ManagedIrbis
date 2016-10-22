/* ExternalResource.cs -- данные о внешнем ресурсе
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Данные о внешнем ресурсе (поле 951).
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("File={FileName} Url={Url} Description={Description}")]
#endif
    public sealed class ExternalResource
#if !WINMOBILE && !PocketPC && !SILVERLIGHT
        : IHandmadeSerializable
#endif
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "12356abdefhiklmnptwx";

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const string Tag = "951";

        #endregion

        #region Properties

        /// <summary>
        /// Имя файла. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("filename")]
        [JsonProperty("filename")]
        public string FileName { get; set; }

        /// <summary>
        /// URL. Подполе i.
        /// </summary>
        [CanBeNull]
        [SubField('i')]
        [XmlAttribute("url")]
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Текст для ссылки. Подполе t.
        /// </summary>
        [CanBeNull]
        [SubField('t')]
        [XmlAttribute("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Количество файлов. Подполе n.
        /// </summary>
        [CanBeNull]
        [SubField('n')]
        [XmlAttribute("fileCount")]
        [JsonProperty("fileCount")]
        public int? FileCount { get; set; }

        /// <summary>
        /// Имя-шаблон первого файла. Подполе m.
        /// </summary>
        [CanBeNull]
        [SubField('m')]
        [XmlAttribute("nameTemplate")]
        [JsonProperty("nameTemplate")]
        public string NameTemplate { get; set; }

        /// <summary>
        /// Тип внешнего файла. Подполе h.
        /// </summary>
        [CanBeNull]
        [SubField('h')]
        [XmlAttribute("fileType")]
        [JsonProperty("fileType")]
        public string FileType { get; set; }

        /// <summary>
        /// Признак электронного учебника. Подполе k.
        /// </summary>
        [CanBeNull]
        [SubField('k')]
        [XmlAttribute("textbook")]
        [JsonProperty("textbook")]
        public string Textbook { get; set; }

        /// <summary>
        /// Уровень доступа по категориям пользователей. Подполе d.
        /// Оно же - дата начала предоставления информации.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("access")]
        [JsonProperty("access")]
        public string AccessLevel { get; set; }

        /// <summary>
        /// Доступен только в ЛВС. Подполе l.
        /// </summary>
        [SubField('l')]
        [XmlAttribute("lan")]
        [JsonProperty("lan")]
        public bool LanOnly { get; set; }

        /// <summary>
        /// Дата ввода информации. Подполе 1.
        /// </summary>
        [CanBeNull]
        [SubField('1')]
        [XmlAttribute("inputDate")]
        [JsonProperty("inputDate")]
        public DateTime? InputDate { get; set; }

        /// <summary>
        /// Размер файла. Подполе 2.
        /// </summary>
        [SubField('2')]
        [XmlAttribute("fileSize")]
        [JsonProperty("fileSize")]
        public long FileSize { get; set; }

        /// <summary>
        /// Номер источника копии. Подполе 3.
        /// </summary>
        [CanBeNull]
        [SubField('3')]
        [XmlAttribute("number")]
        [JsonProperty("number")]
        public string Number { get; set; }

        /// <summary>
        /// Дата последней проверки доступности. Подполе 5.
        /// </summary>
        [CanBeNull]
        [SubField('5')]
        [XmlAttribute("lastCheck")]
        [JsonProperty("lastCheck")]
        public DateTime? LastCheck { get; set; }

        /// <summary>
        /// Размеры изображения в пикселах. Подполе 6.
        /// </summary>
        [CanBeNull]
        [SubField('6')]
        [XmlAttribute("imageSize")]
        [JsonProperty("imageSize")]
        public string ImageSize { get; set; }

        /// <summary>
        /// ISSN. Подполе X.
        /// </summary>
        [CanBeNull]
        [SubField('x')]
        [XmlAttribute("issn")]
        [JsonProperty("issn")]
        public string Issn { get; set; }

        /// <summary>
        /// Форма представления. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("form")]
        [JsonProperty("form")]
        public string Form { get; set; }

        /// <summary>
        /// Код поставщика информации. Подполе f.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlAttribute("provider")]
        [JsonProperty("provider")]
        public string Provider { get; set; }

        /// <summary>
        /// Цена. Подполе e.
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        [XmlAttribute("price")]
        [JsonProperty("price")]
        public string Price { get; set; }

        /// <summary>
        /// Шифр в БД. Подполе w.
        /// </summary>
        [CanBeNull]
        [SubField('w')]
        [XmlAttribute("index")]
        [JsonProperty("index")]
        public string Index { get; set; }

        /// <summary>
        /// Примечания в свободной форме. Подполе p.
        /// </summary>
        [CanBeNull]
        [SubField('p')]
        [XmlAttribute("remarks")]
        [JsonProperty("remarks")]
        public string Remarks { get; set; }

        /// <summary>
        /// Электронная библиотечная система. Подполе s.
        /// </summary>
        [CanBeNull]
        [SubField('s')]
        [XmlAttribute("system")]
        [JsonProperty("system")]
        public string System { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExternalResource()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExternalResource
            (
                string url
            )
        {
            Url = url;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the field.
        /// </summary>
        [NotNull]
        public static ExternalResource Parse
            (
                [NotNull] RecordField field
            )
        {
            ExternalResource result = new ExternalResource
            {
                FileName = field.GetFirstSubFieldValue('a'),
                Url = field.GetFirstSubFieldValue('i'),
                Description = field.GetFirstSubFieldValue('t'),
                FileCount = SubFieldMapper.ToInt32(field, 'n'),
                NameTemplate = field.GetFirstSubFieldValue('m'),
                FileType = field.GetFirstSubFieldValue('h'),
                Textbook = field.GetFirstSubFieldValue('k'),
                AccessLevel = field.GetFirstSubFieldValue('d'),
                LanOnly = SubFieldMapper.ToBoolean(field, 'l'),
                InputDate = SubFieldMapper.ToDateTime(field, '1'),
                FileSize = SubFieldMapper.ToInt64(field, '2'),
                Number = field.GetFirstSubFieldValue('3'),
                LastCheck = SubFieldMapper.ToDateTime(field, '5'),
                ImageSize = field.GetFirstSubFieldValue('6'),
                Issn = field.GetFirstSubFieldValue('x'),
                Form = field.GetFirstSubFieldValue('b'),
                Provider = field.GetFirstSubFieldValue('f'),
                Price = field.GetFirstSubFieldValue('e'),
                Index = field.GetFirstSubFieldValue('w'),
                Remarks = field.GetFirstSubFieldValue('p'),
                System = field.GetFirstSubFieldValue('s')
            };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static ExternalResource[] Parse
            (
                [NotNull] MarcRecord record,
                string tag
            )
        {
            Code.NotNull(record, "record");
            Code.NotNullNorEmpty(tag, "tag");


            return record.Fields
                .GetField(tag)

#if !WINMOBILE && !PocketPC

                .Select(Parse)

#else

                .Select(field => Parse(field))

#endif

.ToArray();
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static ExternalResource[] Parse
            (
                [NotNull] MarcRecord record
            )
        {
            return Parse
                (
                    record,
                    Tag
                );
        }

        /// <summary>
        /// Превращение обратно в поле.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag)
                .AddNonEmptySubField('a', FileName)
                .AddNonEmptySubField('i', Url)
                .AddNonEmptySubField('t', Description)
                .AddNonEmptySubField('m', NameTemplate)
                .AddNonEmptySubField('h', FileType)
                .AddNonEmptySubField('n', FileCount)
                .AddNonEmptySubField('k', Textbook)
                .AddNonEmptySubField('d', AccessLevel)
                .AddNonEmptySubField('l', LanOnly, "да")
                .AddNonEmptySubField('1', InputDate)
                .AddNonEmptySubField('2', FileSize)
                .AddNonEmptySubField('3', Number)
                .AddNonEmptySubField('5', LastCheck)
                .AddNonEmptySubField('6', ImageSize)
                .AddNonEmptySubField('x', Issn)
                .AddNonEmptySubField('b', Form)
                .AddNonEmptySubField('f', Provider)
                .AddNonEmptySubField('e', Price)
                .AddNonEmptySubField('w', Index)
                .AddNonEmptySubField('p', Remarks)
                .AddNonEmptySubField('s', System);

            return result;
        }

        #endregion

#if !WINMOBILE && !PocketPC && !SILVERLIGHT

        #region IHandmadeSerializable members

        /// <inheritdoc/>
        void IHandmadeSerializable.RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            FileName = reader.ReadNullableString();
            Url = reader.ReadNullableString();
            Description = reader.ReadNullableString();
            FileCount = reader.ReadNullableInt32();
            NameTemplate = reader.ReadNullableString();
            FileType = reader.ReadNullableString();
            Textbook = reader.ReadNullableString();
            AccessLevel = reader.ReadNullableString();
            LanOnly = reader.ReadBoolean();
            InputDate = reader.ReadNullableDateTime();
            FileSize = reader.ReadPackedInt64();
            Number = reader.ReadNullableString();
            LastCheck = reader.ReadNullableDateTime();
            ImageSize = reader.ReadNullableString();
            Issn = reader.ReadNullableString();
            Form = reader.ReadNullableString();
            Provider = reader.ReadNullableString();
            Price = reader.ReadNullableString();
            Index = reader.ReadNullableString();
            Remarks = reader.ReadNullableString();
            System = reader.ReadNullableString();
        }

        /// <inheritdoc/>
        void IHandmadeSerializable.SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(FileName)
                .WriteNullable(Url)
                .WriteNullable(Description)
                .Write(FileCount)
                .WriteNullable(NameTemplate)
                .WriteNullable(FileType)
                .WriteNullable(Textbook)
                .WriteNullable(AccessLevel)
                .Write(LanOnly);
            writer
                .Write(InputDate)
                .WritePackedInt64(FileSize)
                .WriteNullable(Number)
                .Write(LastCheck)
                .WriteNullable(ImageSize)
                .WriteNullable(Issn)
                .WriteNullable(Form)
                .WriteNullable(Provider)
                .WriteNullable(Price)
                .WriteNullable(Index)
                .WriteNullable(Remarks)
                .WriteNullable(System);
        }

        #endregion

#endif

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format
                (
                    "FileName: {0}, Url: {1}, Description: {2}",
                    FileName,
                    Url,
                    Description
                );
        }

        #endregion
    }
}
