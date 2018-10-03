// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FullTextInfo.cs -- сведения о полном тексте документа
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
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

namespace ManagedIrbis.Fields
{
    //
    // Начиная с версии 2018.1
    //

    /// <summary>
    /// Сведения о полном тексте документа (поле 955).
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FullTextInfo
        : IHandmadeSerializable,
          IVerifiable
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abnt";

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const int Tag = 955;

        #endregion

        #region Properties

        /// <summary>
        /// Текст для ссылки. Подполе t.
        /// </summary>
        [CanBeNull]
        [SubField('t')]
        [XmlAttribute("display-text")]
        [JsonProperty("displayText", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Текст для ссылки")]
        [DisplayName("Текст для ссылки")]
        public string DisplayText { get; set; }

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
        /// Количество страниц. Подполе n.
        /// </summary>
        [CanBeNull]
        [SubField('n')]
        [XmlAttribute("page-count")]
        [JsonProperty("pageCount", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [Description("Количество страниц")]
        [DisplayName("Количество страниц")]
        public int? PageCount { get; set; }

        /// <summary>
        /// Идентификатор записи права доступа. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("access-rights")]
        [JsonProperty("accessRights", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Идентификатор записи права доступа")]
        [DisplayName("Идентификатор записи права доступа")]
        public string AccessRights { get; set; }

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
        public FullTextInfo()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FullTextInfo
            (
                [CanBeNull] string fileName
            )
        {
            FileName = fileName;
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
                .ApplySubField('t', DisplayText)
                .ApplySubField('a', FileName)
                .ApplySubField('n', PageCount)
                .ApplySubField('b', AccessRights);
        }

        /// <summary>
        /// Parse the field.
        /// </summary>
        [NotNull]
        public static FullTextInfo Parse
            (
                [NotNull] RecordField field
            )
        {
            FullTextInfo result = new FullTextInfo
            {
                DisplayText = field.GetFirstSubFieldValue('t'),
                FileName = field.GetFirstSubFieldValue('a'),
                PageCount = SubFieldMapper.ToInt32(field, 'n'),
                AccessRights = field.GetFirstSubFieldValue('b'),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [NotNull][ItemNotNull]
        public static FullTextInfo[] Parse
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
        [NotNull][ItemNotNull]
        public static FullTextInfo[] Parse
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
                .AddNonEmptySubField('t', FileName)
                .AddNonEmptySubField('a', FileName)
                .AddNonEmptySubField('n', PageCount)
                .AddNonEmptySubField('b', AccessRights);

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

            DisplayText = reader.ReadNullableString();
            FileName = reader.ReadNullableString();
            AccessRights = reader.ReadNullableString();
            PageCount = null;
            if (reader.ReadBoolean())
            {
                PageCount = reader.ReadPackedInt32();
            }
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        void IHandmadeSerializable.SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(DisplayText)
                .WriteNullable(FileName)
                .WriteNullable(AccessRights);
            writer.Write(PageCount.HasValue);
            if (PageCount.HasValue)
            {
                writer.WritePackedInt32(PageCount.Value);
            }
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<FullTextInfo> verifier
                = new Verifier<FullTextInfo>(this, throwOnError);

            verifier.Assert
                (
                    !string.IsNullOrEmpty(FileName),
                    "FileName"
                );

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return FileName.ToVisibleString();
        }

        #endregion
    }
}
