// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NumberOfTitlesInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

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
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("numberOfTitles")]
    public sealed class NumberOfTitlesInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Tag number.
        /// </summary>
        public const int Tag = 18;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "1234567";

        #endregion

        #region Properties

        /// <summary>
        /// Вновь поступившие. Подполе 1.
        /// </summary>
        [CanBeNull]
        [SubField('1')]
        [XmlAttribute("arrivals")]
        [JsonProperty("arrivals", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Вновь поступившие")]
        [DisplayName("Вновь поступившие")]
        public string NewArrivals { get; set; }

        /// <summary>
        /// Книги. Подполе 2.
        /// </summary>
        [CanBeNull]
        [SubField('2')]
        [XmlAttribute("books")]
        [JsonProperty("books", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Книги")]
        [DisplayName("Книги")]
        public string Books { get; set; }

        /// <summary>
        /// Монографические издания. Подполе 3.
        /// </summary>
        [CanBeNull]
        [SubField('3')]
        [XmlAttribute("monographic")]
        [JsonProperty("monographic", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Монографические издания")]
        [DisplayName("Монографические издания")]
        public string Monographic { get; set; }

        /// <summary>
        /// Брошюры. Подполе 4.
        /// </summary>
        [CanBeNull]
        [SubField('4')]
        [XmlAttribute("brochures")]
        [JsonProperty("brochures", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Брошюры")]
        [DisplayName("Брошюры")]
        public string Brochures { get; set; }

        /// <summary>
        /// Число томов. Подполе 5.
        /// </summary>
        [CanBeNull]
        [SubField('5')]
        [XmlAttribute("volumes")]
        [JsonProperty("volumes", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Число томов")]
        [DisplayName("Число томов")]
        public string Volumes { get; set; }

        /// <summary>
        /// Отечественные издания. Подполе 6.
        /// </summary>
        [CanBeNull]
        [SubField('6')]
        [XmlAttribute("domestic")]
        [JsonProperty("domestic", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Отечественные издания")]
        [DisplayName("Отечественные издания")]
        public string Domestic { get; set; }

        /// <summary>
        /// Иностранные книги. Подполе 7.
        /// </summary>
        [CanBeNull]
        [SubField('7')]
        [XmlAttribute("foreign")]
        [JsonProperty("foreign", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Иностранные книги")]
        [DisplayName("Иностранные книги")]
        public string Foreign { get; set; }

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

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the <see cref="NumberOfTitlesInfo"/>
        /// to the <see cref="RecordField"/>.
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField('1', NewArrivals)
                .ApplySubField('2', Books)
                .ApplySubField('3', Monographic)
                .ApplySubField('4', Brochures)
                .ApplySubField('5', Volumes)
                .ApplySubField('6', Domestic)
                .ApplySubField('7', Foreign);
        }

        /// <summary>
        /// Parse the <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public static NumberOfTitlesInfo ParseField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            NumberOfTitlesInfo result = new NumberOfTitlesInfo
            {
                NewArrivals = field.GetFirstSubFieldValue('1'),
                Books = field.GetFirstSubFieldValue('2'),
                Monographic = field.GetFirstSubFieldValue('3'),
                Brochures = field.GetFirstSubFieldValue('4'),
                Volumes = field.GetFirstSubFieldValue('5'),
                Domestic = field.GetFirstSubFieldValue('6'),
                Foreign = field.GetFirstSubFieldValue('7'),
                UnknownSubFields = field.SubFields.GetUnknownSubFields(KnownCodes),
                Field = field
            };

            return result;
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
        /// Convert the <see cref="NumberOfTitlesInfo"/>
        /// back to <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag)
                .AddNonEmptySubField('1', NewArrivals)
                .AddNonEmptySubField('2', Books)
                .AddNonEmptySubField('3', Monographic)
                .AddNonEmptySubField('4', Brochures)
                .AddNonEmptySubField('5', Volumes)
                .AddNonEmptySubField('6', Domestic)
                .AddNonEmptySubField('7', Foreign)
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

            NewArrivals = reader.ReadNullableString();
            Books = reader.ReadNullableString();
            Monographic = reader.ReadNullableString();
            Brochures = reader.ReadNullableString();
            Volumes = reader.ReadNullableString();
            Domestic = reader.ReadNullableString();
            Foreign = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(NewArrivals)
                .WriteNullable(Books)
                .WriteNullable(Monographic)
                .WriteNullable(Brochures)
                .WriteNullable(Volumes)
                .WriteNullable(Domestic)
                .WriteNullable(Foreign)
                .WriteNullableArray(UnknownSubFields);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
           Verifier<NumberOfTitlesInfo> verifier
                = new Verifier<NumberOfTitlesInfo>(this, throwOnError);

            verifier.Assert
                (
                    !string.IsNullOrEmpty(NewArrivals)
                    || !string.IsNullOrEmpty(Books)
                    || !string.IsNullOrEmpty(Monographic)
                    || !string.IsNullOrEmpty(Brochures)
                    || !string.IsNullOrEmpty(Volumes)
                    || !string.IsNullOrEmpty(Domestic)
                    || !string.IsNullOrEmpty(Foreign)
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
                    "NewArrivals: {0}, Books: {1}, Monographic: {2}, "
                    + "Brochures: {3}, Volumes: {4}, Domestic: {5}, "
                    + "Foreign: {6}",
                    NewArrivals.ToVisibleString(),
                    Books.ToVisibleString(),
                    Monographic.ToVisibleString(),
                    Brochures.ToVisibleString(),
                    Volumes.ToVisibleString(),
                    Domestic.ToVisibleString(),
                    Foreign.ToVisibleString()
                );
        }

        #endregion
    }
}
