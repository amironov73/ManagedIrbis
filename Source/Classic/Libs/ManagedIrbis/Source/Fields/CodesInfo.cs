// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CodesInfo.cs -- коды (поле 900)
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
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
    /// Коды (поле 900).
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class CodesInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Known codes.
        /// </summary>
        public const string KnownCodes = "bctxyz234569";

        /// <summary>
        /// Tag.
        /// </summary>
        public const int Tag = 900;

        #endregion

        #region Properties

        /// <summary>
        /// Тип документа. Подполе t.
        /// </summary>
        [CanBeNull]
        [SubField('t')]
        [XmlAttribute("type")]
        [JsonProperty("type")]
        [Description("Тип документа")]
        [DisplayName("Тип документа")]
        public string DocumentType { get; set; }

        /// <summary>
        /// Вид документа. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("kind")]
        [JsonProperty("kind")]
        [Description("Вид документа")]
        [DisplayName("Вид документа")]
        public string DocumentKind { get; set; }

        /// <summary>
        /// Характер документа. Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("character1")]
        [JsonProperty("character1")]
        [Description("Характер документа")]
        [DisplayName("Характер документа (1)")]
        public string DocumentCharacter1 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 2.
        /// </summary>
        [CanBeNull]
        [SubField('2')]
        [XmlAttribute("character2")]
        [JsonProperty("character2")]
        [Description("Характер документа")]
        [DisplayName("Характер документа (2)")]
        public string DocumentCharacter2 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 3.
        /// </summary>
        [CanBeNull]
        [SubField('3')]
        [XmlAttribute("character3")]
        [JsonProperty("character3")]
        [Description("Характер документа")]
        [DisplayName("Характер документа (3)")]
        public string DocumentCharacter3 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 4.
        /// </summary>
        [CanBeNull]
        [SubField('4')]
        [XmlAttribute("character4")]
        [JsonProperty("character4")]
        [Description("Характер документа")]
        [DisplayName("Характер документа (4)")]
        public string DocumentCharacter4 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 5.
        /// </summary>
        [CanBeNull]
        [SubField('5')]
        [XmlAttribute("character5")]
        [JsonProperty("character5")]
        [Description("Характер документа")]
        [DisplayName("Характер документа (5)")]
        public string DocumentCharacter5 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 6.
        /// </summary>
        [CanBeNull]
        [SubField('6')]
        [XmlAttribute("character6")]
        [JsonProperty("character6")]
        [Description("Характер документа")]
        [DisplayName("Характер документа (6)")]
        public string DocumentCharacter6 { get; set; }

        /// <summary>
        /// Код целевого назначения. Подполе x.
        /// </summary>
        [CanBeNull]
        [SubField('x')]
        [XmlAttribute("purpose1")]
        [JsonProperty("purpose1")]
        [Description("Код целевого назначения")]
        [DisplayName("Код целевого назначения (1)")]
        public string PurposeCode1 { get; set; }

        /// <summary>
        /// Код целевого назначения. Подполе y.
        /// </summary>
        [CanBeNull]
        [SubField('y')]
        [XmlAttribute("purpose2")]
        [JsonProperty("purpose2")]
        [Description("Код целевого назначения")]
        [DisplayName("Код целевого назначения (2)")]
        public string PurposeCode2 { get; set; }

        /// <summary>
        /// Код целевого назначения. Подполе 9.
        /// </summary>
        [CanBeNull]
        [SubField('9')]
        [XmlAttribute("purpose3")]
        [JsonProperty("purpose3")]
        [Description("Код целевого назначения")]
        [DisplayName("Код целевого назначения (3)")]
        public string PurposeCode3 { get; set; }

        /// <summary>
        /// Возрастные ограничения. Подполе z.
        /// </summary>
        [CanBeNull]
        [SubField('z')]
        [XmlAttribute("age")]
        [JsonProperty("age")]
        [Description("Возрастные ограничения")]
        [DisplayName("Возрастные ограничения")]
        public string AgeRestrictions { get; set; }

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
        [Description("Пользовательские данные")]
        [DisplayName("Пользовательские данные")]
        public object UserData { get; set; }

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
            Code.NotNull(field, "field");

            field
                .ApplySubField('t', DocumentType)
                .ApplySubField('b', DocumentKind)
                .ApplySubField('c', DocumentCharacter1)
                .ApplySubField('2', DocumentCharacter2)
                .ApplySubField('3', DocumentCharacter3)
                .ApplySubField('4', DocumentCharacter4)
                .ApplySubField('5', DocumentCharacter5)
                .ApplySubField('6', DocumentCharacter6)
                .ApplySubField('x', PurposeCode1)
                .ApplySubField('y', PurposeCode2)
                .ApplySubField('9', PurposeCode3)
                .ApplySubField('z', AgeRestrictions);
        }

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        [NotNull]
        public static CodesInfo Parse
            (
                [NotNull] RecordField field
            )
        {
            CodesInfo result = new CodesInfo
                {
                    DocumentType = field.GetFirstSubFieldValue('t'),
                    DocumentKind = field.GetFirstSubFieldValue('b'),
                    DocumentCharacter1 = field.GetFirstSubFieldValue('c'),
                    DocumentCharacter2 = field.GetFirstSubFieldValue('2'),
                    DocumentCharacter3 = field.GetFirstSubFieldValue('3'),
                    DocumentCharacter4 = field.GetFirstSubFieldValue('4'),
                    DocumentCharacter5 = field.GetFirstSubFieldValue('5'),
                    DocumentCharacter6 = field.GetFirstSubFieldValue('6'),
                    PurposeCode1 = field.GetFirstSubFieldValue('x'),
                    PurposeCode2 = field.GetFirstSubFieldValue('y'),
                    PurposeCode3 = field.GetFirstSubFieldValue('9'),
                    AgeRestrictions = field.GetFirstSubFieldValue('z'),
                    Field = field
                };

            return result;
        }

        /// <summary>
        /// Transform back to field.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField("900")
                .AddNonEmptySubField('t', DocumentType)
                .AddNonEmptySubField('b', DocumentKind)
                .AddNonEmptySubField('c', DocumentCharacter1)
                .AddNonEmptySubField('2', DocumentCharacter2)
                .AddNonEmptySubField('3', DocumentCharacter3)
                .AddNonEmptySubField('4', DocumentCharacter4)
                .AddNonEmptySubField('5', DocumentCharacter5)
                .AddNonEmptySubField('6', DocumentCharacter6)
                .AddNonEmptySubField('x', PurposeCode1)
                .AddNonEmptySubField('y', PurposeCode2)
                .AddNonEmptySubField('9', PurposeCode3)
                .AddNonEmptySubField('z', AgeRestrictions);

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

            DocumentType = reader.ReadNullableString();
            DocumentKind = reader.ReadNullableString();
            DocumentCharacter1 = reader.ReadNullableString();
            DocumentCharacter2 = reader.ReadNullableString();
            DocumentCharacter3 = reader.ReadNullableString();
            DocumentCharacter4 = reader.ReadNullableString();
            DocumentCharacter5 = reader.ReadNullableString();
            DocumentCharacter6 = reader.ReadNullableString();
            PurposeCode1 = reader.ReadNullableString();
            PurposeCode2 = reader.ReadNullableString();
            PurposeCode3 = reader.ReadNullableString();
            AgeRestrictions = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(DocumentType)
                .WriteNullable(DocumentKind)
                .WriteNullable(DocumentCharacter1)
                .WriteNullable(DocumentCharacter2)
                .WriteNullable(DocumentCharacter3)
                .WriteNullable(DocumentCharacter4)
                .WriteNullable(DocumentCharacter5)
                .WriteNullable(DocumentCharacter6)
                .WriteNullable(PurposeCode1)
                .WriteNullable(PurposeCode2)
                .WriteNullable(PurposeCode3)
                .WriteNullable(AgeRestrictions);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<CodesInfo> verifier
                = new Verifier<CodesInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(DocumentType, "DocumentType");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "DocumentType: {0}, DocumentKind: {1}, "
                    + "DocumentCharacter1: {2}",
                    DocumentType,
                    DocumentKind,
                    DocumentCharacter1
                );
        }

        #endregion
    }
}
