// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExternalResource.cs -- данные о внешнем ресурсе
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

// ReSharper disable StringLiteralTypo
// ReSharper disable ConvertClosureToMethodGroup

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Данные о внешнем ресурсе (поле 951).
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("File={FileName} Url={Url} Description={Description}")]
    public sealed class ExternalResource
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "12356abdefhiklmnptwx";

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const int Tag = 951;

        #endregion

        #region Properties

        /// <summary>
        /// Имя файла. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("filename")]
        [JsonProperty("filename", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Имя файла")]
        [DisplayName("Имя файла")]
        public string FileName { get; set; }

        /// <summary>
        /// URL. Подполе i.
        /// </summary>
        [CanBeNull]
        [SubField('i')]
        [XmlAttribute("url")]
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        [Description("URL")]
        [DisplayName("URL")]
        public string Url { get; set; }

        /// <summary>
        /// Текст для ссылки. Подполе t.
        /// </summary>
        [CanBeNull]
        [SubField('t')]
        [XmlAttribute("description")]
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Текст для ссылки")]
        [DisplayName("Текст для ссылки")]
        public string Description { get; set; }

        /// <summary>
        /// Количество файлов. Подполе n.
        /// </summary>
        [CanBeNull]
        [SubField('n')]
        [XmlAttribute("fileCount")]
        [JsonProperty("fileCount", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [Description("Количество файлов")]
        [DisplayName("Количество файлов")]
        public int? FileCount { get; set; }

        /// <summary>
        /// Имя-шаблон первого файла. Подполе m.
        /// </summary>
        [CanBeNull]
        [SubField('m')]
        [XmlAttribute("nameTemplate")]
        [JsonProperty("nameTemplate", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Имя-шаблон первого файла")]
        [DisplayName("Имя-шаблон первого файла")]
        public string NameTemplate { get; set; }

        /// <summary>
        /// Тип внешнего файла. Подполе h.
        /// </summary>
        [CanBeNull]
        [SubField('h')]
        [XmlAttribute("fileType")]
        [JsonProperty("fileType", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Тип внешнего файла")]
        [DisplayName("Тип внешнего файла")]
        public string FileType { get; set; }

        /// <summary>
        /// Признак электронного учебника. Подполе k.
        /// </summary>
        [CanBeNull]
        [SubField('k')]
        [XmlAttribute("textbook")]
        [JsonProperty("textbook", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Признак электронного учебника")]
        [DisplayName("Признак электронного учебника")]
        public string Textbook { get; set; }

        /// <summary>
        /// Уровень доступа по категориям пользователей. Подполе d.
        /// Оно же - дата начала предоставления информации.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("access")]
        [JsonProperty("access", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Уровень доступа")]
        [DisplayName("Уровень доступа")]
        public string AccessLevel { get; set; }

        /// <summary>
        /// Доступен только в ЛВС. Подполе l.
        /// </summary>
        [SubField('l')]
        [XmlAttribute("lan")]
        [JsonProperty("lan", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [Description("Доступен только в ЛВС")]
        [DisplayName("Доступен только в ЛВС")]
        public bool LanOnly { get; set; }

        /// <summary>
        /// Дата ввода информации. Подполе 1.
        /// </summary>
        [CanBeNull]
        [SubField('1')]
        [XmlAttribute("inputDate")]
        [JsonProperty("inputDate", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [Description("Дата ввода информации")]
        [DisplayName("Дата ввода информации")]
        public DateTime? InputDate { get; set; }

        /// <summary>
        /// Размер файла. Подполе 2.
        /// </summary>
        [SubField('2')]
        [XmlAttribute("fileSize")]
        [JsonProperty("fileSize", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [Description("Размер файла")]
        [DisplayName("Размер файла")]
        public long FileSize { get; set; }

        /// <summary>
        /// Номер источника копии. Подполе 3.
        /// </summary>
        [CanBeNull]
        [SubField('3')]
        [XmlAttribute("number")]
        [JsonProperty("number")]
        [Description("Номер источника копии")]
        [DisplayName("Номер источника копии")]
        public string Number { get; set; }

        /// <summary>
        /// Дата последней проверки доступности. Подполе 5.
        /// </summary>
        [CanBeNull]
        [SubField('5')]
        [XmlAttribute("lastCheck")]
        [JsonProperty("lastCheck", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [Description("Дата последней проверки")]
        [DisplayName("Дата последней проверки")]
        public DateTime? LastCheck { get; set; }

        /// <summary>
        /// Размеры изображения в пикселах. Подполе 6.
        /// </summary>
        [CanBeNull]
        [SubField('6')]
        [XmlAttribute("imageSize")]
        [JsonProperty("imageSize", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Размеры изображения")]
        [DisplayName("Размеры изображения")]
        public string ImageSize { get; set; }

        /// <summary>
        /// ISSN. Подполе X.
        /// </summary>
        [CanBeNull]
        [SubField('x')]
        [XmlAttribute("issn")]
        [JsonProperty("issn", NullValueHandling = NullValueHandling.Ignore)]
        [Description("ISSN")]
        [DisplayName("ISSN")]
        public string Issn { get; set; }

        /// <summary>
        /// Форма представления. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("form")]
        [JsonProperty("form")]
        [Description("Форма представления")]
        [DisplayName("Форма представления")]
        public string Form { get; set; }

        /// <summary>
        /// Код поставщика информации. Подполе f.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlAttribute("provider")]
        [JsonProperty("provider", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Код поставщика информации")]
        [DisplayName("Код поставщика информации")]
        public string Provider { get; set; }

        /// <summary>
        /// Цена. Подполе e.
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        [XmlAttribute("price")]
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Цена")]
        [DisplayName("Цена")]
        public string Price { get; set; }

        /// <summary>
        /// Шифр в БД. Подполе w.
        /// </summary>
        [CanBeNull]
        [SubField('w')]
        [XmlAttribute("index")]
        [JsonProperty("index")]
        [Description("Шифр в БД")]
        [DisplayName("Шифр в БД")]
        public string Index { get; set; }

        /// <summary>
        /// Примечания в свободной форме. Подполе p.
        /// </summary>
        [CanBeNull]
        [SubField('p')]
        [XmlAttribute("remarks")]
        [JsonProperty("remarks", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Примечания в свободной форме")]
        [DisplayName("Примечания в свободной форме")]
        public string Remarks { get; set; }

        /// <summary>
        /// Электронная библиотечная система. Подполе s.
        /// </summary>
        [CanBeNull]
        [SubField('s')]
        [XmlAttribute("system")]
        [JsonProperty("system", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Электронная библиотечная система")]
        [DisplayName("Электронная библиотечная система")]
        public string System { get; set; }

        /// <summary>
        /// Associated field.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        [Description("Поле")]
        [DisplayName("Поле")]
        public RecordField Field { get; private set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        [Description("Пользовательские данные")]
        [DisplayName("Пользовательские данные")]
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
                [CanBeNull] string url
            )
        {
            Url = url;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Apply to the field.
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            // TODO check the applying

            field
                .ApplySubField('a', FileName)
                .ApplySubField('i', Url)
                .ApplySubField('t', Description)
                .ApplySubField('m', NameTemplate)
                .ApplySubField('h', FileType)
                .ApplySubField('n', FileCount)
                .ApplySubField('k', Textbook)
                .ApplySubField('d', AccessLevel)
                .ApplySubField('l', LanOnly, "да")
                .ApplySubField('1', InputDate)
                .ApplySubField('2', FileSize)
                .ApplySubField('3', Number)
                .ApplySubField('5', LastCheck)
                .ApplySubField('6', ImageSize)
                .ApplySubField('x', Issn)
                .ApplySubField('b', Form)
                .ApplySubField('f', Provider)
                .ApplySubField('e', Price)
                .ApplySubField('w', Index)
                .ApplySubField('p', Remarks)
                .ApplySubField('s', System);
        }

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
                FileSize = SubFieldMapper.ToInt64(field, '2') ?? 0,
                Number = field.GetFirstSubFieldValue('3'),
                LastCheck = SubFieldMapper.ToDateTime(field, '5'),
                ImageSize = field.GetFirstSubFieldValue('6'),
                Issn = field.GetFirstSubFieldValue('x'),
                Form = field.GetFirstSubFieldValue('b'),
                Provider = field.GetFirstSubFieldValue('f'),
                Price = field.GetFirstSubFieldValue('e'),
                Index = field.GetFirstSubFieldValue('w'),
                Remarks = field.GetFirstSubFieldValue('p'),
                System = field.GetFirstSubFieldValue('s'),
                Field = field
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
                int tag
            )
        {
            Code.NotNull(record, "record");

            return record.Fields
                .GetField(tag)
                .Select(field => Parse(field))
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

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
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

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
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

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ExternalResource> verifier
                = new Verifier<ExternalResource>(this, throwOnError);

            verifier.Assert
                (
                    !string.IsNullOrEmpty(FileName)
                    ^ !string.IsNullOrEmpty(Url),
                    "FileName or URL"
                );

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "FileName: {0}, Url: {1}, Description: {2}",
                    FileName.ToVisibleString(),
                    Url.ToVisibleString(),
                    Description.ToVisibleString()
                );
        }

        #endregion
    }
}
