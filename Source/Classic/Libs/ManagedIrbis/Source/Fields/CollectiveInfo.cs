// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CollectiveInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Коллективный (в т. ч. временный) автор.
    /// Раскладка полей 710, 711, 962, 972.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("collective")]
    public sealed class CollectiveInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abcfnrsx7";

        #endregion

        #region Properties

        /// <summary>
        /// Empty array of the <see cref="CollectiveInfo"/>.
        /// </summary>
        public static readonly CollectiveInfo[] EmptyArray
            = new CollectiveInfo[0];

        /// <summary>
        /// Known tags.
        /// </summary>
        [NotNull]
        public static int[] KnownTags { get { return _knownTags; } }

        /// <summary>
        /// Наименование. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlElement("title")]
        [JsonProperty("title")]
        [Description("Наименование")]
        [DisplayName("Наименование")]
        public string Title { get; set; }

        /// <summary>
        /// Страна. Подполе s.
        /// </summary>
        [CanBeNull]
        [SubField('s')]
        [XmlElement("country")]
        [JsonProperty("country")]
        [Description("Страна")]
        [DisplayName("Страна")]
        public string Country { get; set; }

        /// <summary>
        /// Аббревиатура. Подполе r.
        /// </summary>
        [CanBeNull]
        [SubField('r')]
        [XmlElement("abbreviation")]
        [JsonProperty("abbreviation")]
        [Description("Аббревиатура")]
        [DisplayName("Аббревиатура")]
        public string Abbreviation { get; set; }

        /// <summary>
        /// Номер. Подполе n.
        /// </summary>
        [CanBeNull]
        [SubField('n')]
        [XmlElement("number")]
        [JsonProperty("number")]
        [Description("Номер")]
        [DisplayName("Номер")]
        public string Number { get; set; }

        /// <summary>
        /// Дата проведения мероприятия. Подполе f.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlElement("date")]
        [JsonProperty("date")]
        [Description("Дата проведения мероприятия")]
        [DisplayName("Дата проведения мероприятия")]
        public string Date { get; set; }

        /// <summary>
        /// Город. Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlElement("city")]
        [JsonProperty("city")]
        [Description("Город")]
        [DisplayName("Город")]
        public string City1 { get; set; }

        /// <summary>
        /// Подразделение. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlElement("department")]
        [JsonProperty("department")]
        [Description("Подразделение")]
        [DisplayName("Подразделение")]
        public string Department { get; set; }

        /// <summary>
        /// Характерное название подразделения. Подполе x.
        /// </summary>
        [SubField('x')]
        [XmlElement("characteristic")]
        [JsonProperty("characteristic")]
        [Description("Характерное название подразделения")]
        [DisplayName("Характерное название подразделения")]
        public bool Characteristic { get; set; }

        /// <summary>
        /// Сокращение по ГОСТ. Подполе 7.
        /// </summary>
        [CanBeNull]
        [SubField('7')]
        [XmlElement("gost")]
        [JsonProperty("gost")]
        [Description("Сокращение по ГОСТ")]
        [DisplayName("Сокращение по ГОСТ")]
        public string Gost { get; set; }

        /// <summary>
        /// Unknown subfields.
        /// </summary>
        [CanBeNull]
        [XmlElement("unknown")]
        [JsonProperty("unknown")]
        [Browsable(false)]
        public SubField[] UnknownSubFields { get; set; }

        /// <summary>
        /// Associated field.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public RecordField Field { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CollectiveInfo()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CollectiveInfo
            (
                [CanBeNull] string title
            )
        {
            Title = title;
        }

        #endregion

        #region Private members

        private static readonly int[] _knownTags =
        {
            710, 711, 962, 972
        };

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the <see cref="CollectiveInfo"/>
        /// to the <see cref="RecordField"/>.
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField('a', Title)
                .ApplySubField('s', Country)
                .ApplySubField('r', Abbreviation)
                .ApplySubField('n', Number)
                .ApplySubField('f', Date)
                .ApplySubField('c', City1)
                .ApplySubField('b', Department)
                .ApplySubField('x', Characteristic ? "1" : null)
                .ApplySubField('7', Gost);
        }

        /// <summary>
        /// Parse the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static CollectiveInfo[] ParseRecord
            (
                [NotNull] MarcRecord record,
                [NotNull] int[] tags
            )
        {
            Code.NotNull(record, "record");
            Code.NotNull(tags, "tags");

            List<CollectiveInfo> result = new List<CollectiveInfo>();
            foreach (RecordField field in record.Fields)
            {
                if (field.Tag.OneOf(tags))
                {
                    CollectiveInfo collective = ParseField(field);
                    result.Add(collective);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        [NotNull]
        public static CollectiveInfo ParseField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            CollectiveInfo result = new CollectiveInfo
            {
                Title = field.GetFirstSubFieldValue('a'),
                Country = field.GetFirstSubFieldValue('s'),
                Abbreviation = field.GetFirstSubFieldValue('r'),
                Number = field.GetFirstSubFieldValue('n'),
                Date = field.GetFirstSubFieldValue('f'),
                City1 = field.GetFirstSubFieldValue('c'),
                Department = field.GetFirstSubFieldValue('b'),
                Characteristic = !string.IsNullOrEmpty
                    (
                        field.GetFirstSubFieldValue('x')
                    ),
                Gost = field.GetFirstSubFieldValue('7'),
                UnknownSubFields = field.SubFields.GetUnknownSubFields(KnownCodes),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Should serialize the <see cref="Characteristic"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCharacteristic()
        {
            return Characteristic;
        }

        /// <summary>
        /// Should serialize the <see cref="UnknownSubFields"/> array?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeUnknownSubFields()
        {
            return !ArrayUtility.IsNullOrEmpty(UnknownSubFields);
        }

        /// <summary>
        /// Convert the <see cref="CollectiveInfo"/>
        /// back to <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public RecordField ToField
            (
                int tag
            )
        {
            Code.Positive(tag, "tag");

            RecordField result = new RecordField(tag);
            result
                .AddNonEmptySubField('a', Title)
                .AddNonEmptySubField('s', Country)
                .AddNonEmptySubField('r', Abbreviation)
                .AddNonEmptySubField('n', Number)
                .AddNonEmptySubField('f', Date)
                .AddNonEmptySubField('c', City1)
                .AddNonEmptySubField('b', Department)
                .AddNonEmptySubField('x', Characteristic ? "1" : null)
                .AddNonEmptySubField('7', Gost)
                .AddSubFields(UnknownSubFields);

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Title = reader.ReadNullableString();
            Country = reader.ReadNullableString();
            Abbreviation = reader.ReadNullableString();
            Number = reader.ReadNullableString();
            Date = reader.ReadNullableString();
            City1 = reader.ReadNullableString();
            Department = reader.ReadNullableString();
            Gost = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();
            Characteristic = reader.ReadBoolean();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Title)
                .WriteNullable(Country)
                .WriteNullable(Abbreviation)
                .WriteNullable(Number)
                .WriteNullable(Date)
                .WriteNullable(City1)
                .WriteNullable(Department)
                .WriteNullable(Gost)
                .WriteNullableArray(UnknownSubFields)
                .Write(Characteristic);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<CollectiveInfo> verifier
                = new Verifier<CollectiveInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Title, "Title");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Title.ToVisibleString();
        }

        #endregion
    }
}
