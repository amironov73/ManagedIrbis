// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MagazineCumulation.cs -- данные о кумуляции номеров
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

// ReSharper disable ConvertClosureToMethodGroup

namespace ManagedIrbis.Magazines
{
    /// <summary>
    /// Данные о кумуляции номеров. Поле 909.
    /// </summary>
    [PublicAPI]
    [XmlRoot("cumulation")]
    [MoonSharpUserData]
    public sealed class MagazineCumulation
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const int Tag = 909;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "dfhkq";

        #endregion

        #region Properties

        /// <summary>
        /// Год. Подполе Q.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("year")]
        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public string Year { get; set; }

        /// <summary>
        /// Том. Подполе F.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("volume")]
        [JsonProperty("volume", NullValueHandling = NullValueHandling.Ignore)]
        public string Volume { get; set; }

        /// <summary>
        /// Место хранения. Подполе D.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("place")]
        [JsonProperty("place", NullValueHandling = NullValueHandling.Ignore)]
        public string Place { get; set; }

        /// <summary>
        /// Кумулированные номера. Подполе H.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("numbers")]
        [JsonProperty("numbers", NullValueHandling = NullValueHandling.Ignore)]
        public string Numbers { get; set; }

        /// <summary>
        /// Номер комплекта. Подполе K.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("set")]
        [JsonProperty("set", NullValueHandling = NullValueHandling.Ignore)]
        public string Set { get; set; }

        /// <summary>
        /// Unknown subfields.
        /// </summary>
        [CanBeNull]
        [XmlElement("unknown")]
        [JsonProperty("unknown", NullValueHandling = NullValueHandling.Ignore)]
        public SubField[] UnknownSubFields { get; set; }

        /// <summary>
        /// Field.
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

        #region Public methods

        /// <summary>
        /// Apply to the <see cref="RecordField"/>.
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField('q', Year)
                .ApplySubField('f', Volume)
                .ApplySubField('d', Place)
                .ApplySubField('h', Numbers)
                .ApplySubField('k', Set);
        }

        /// <summary>
        /// Разбор поля.
        /// </summary>
        [NotNull]
        public static MagazineCumulation Parse
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            MagazineCumulation result = new MagazineCumulation
            {
                Year = field.GetFirstSubFieldValue('q'),
                Volume = field.GetFirstSubFieldValue('f'),
                Place = field.GetFirstSubFieldValue('d'),
                Numbers = field.GetFirstSubFieldValue('h'),
                Set = field.GetFirstSubFieldValue('k'),
                UnknownSubFields = field.SubFields.GetUnknownSubFields(KnownCodes),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static MagazineCumulation[] Parse
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
        public static MagazineCumulation[] Parse
            (
                [NotNull] MarcRecord record
            )
        {
            return Parse(record, Tag);
        }

        /// <summary>
        /// Should serialize the <see cref="UnknownSubFields"/> array?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeUnknownSubFields()
        {
            return !ReferenceEquals(UnknownSubFields, null)
                   && UnknownSubFields.Length != 0;
        }

        /// <summary>
        /// Convert back to <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag)
                .AddNonEmptySubField('q', Year)
                .AddNonEmptySubField('f', Volume)
                .AddNonEmptySubField('d', Place)
                .AddNonEmptySubField('h', Numbers)
                .AddNonEmptySubField('k', Set)
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

            Year = reader.ReadNullableString();
            Volume = reader.ReadNullableString();
            Place = reader.ReadNullableString();
            Numbers = reader.ReadNullableString();
            Set = reader.ReadNullableString();
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
                .WriteNullable(Year)
                .WriteNullable(Volume)
                .WriteNullable(Place)
                .WriteNullable(Numbers)
                .WriteNullable(Set)
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
            Verifier<MagazineCumulation> verifier
                = new Verifier<MagazineCumulation>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Year, "Year")
                .NotNullNorEmpty(Numbers, "Number");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Year.ToVisibleString() + ":" + Numbers.ToVisibleString();
        }

        #endregion
    }
}
