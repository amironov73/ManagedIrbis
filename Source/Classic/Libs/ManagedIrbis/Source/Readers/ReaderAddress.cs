// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReaderAddress.cs -- адрес читателя
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

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Адрес читателя: поле 13 в базе RDR.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("address")]
    public sealed class ReaderAddress
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const int Tag = 13;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abcdefgh";

        #endregion

        #region Properties

        /// <summary>
        /// Почтовый индекс. Подполе A.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("postcode")]
        [JsonProperty("postcode", NullValueHandling = NullValueHandling.Ignore)]
        public string Postcode { get; set; }

        /// <summary>
        /// Страна/республика. Подполе B.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("country")]
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        /// <summary>
        /// Город. Подполе C.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("city")]
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }

        /// <summary>
        /// Улица. Подполе D.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("street")]
        [JsonProperty("street", NullValueHandling = NullValueHandling.Ignore)]
        public string Street { get; set; }

        /// <summary>
        /// Номер дома. Подполе E.
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        [XmlAttribute("building")]
        [JsonProperty("building", NullValueHandling = NullValueHandling.Ignore)]
        public string Building { get; set; }

        /// <summary>
        /// Номер подъезда. Подполе G.
        /// </summary>
        [CanBeNull]
        [SubField('g')]
        [XmlAttribute("entrance")]
        [JsonProperty("entrance", NullValueHandling = NullValueHandling.Ignore)]
        public string Entrance { get; set; }

        /// <summary>
        /// Номер квартиры. Подполе H.
        /// </summary>
        [CanBeNull]
        [SubField('h')]
        [XmlAttribute("apartment")]
        [JsonProperty("apartment", NullValueHandling = NullValueHandling.Ignore)]
        public string Apartment { get; set; }

        /// <summary>
        /// Дополнительные данные. Подполе F.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlAttribute("additionalData")]
        [JsonProperty("additionalData", NullValueHandling = NullValueHandling.Ignore)]
        public string AdditionalData { get; set; }

        /// <summary>
        /// Поле, в котором хранится адрес.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public RecordField Field { get; set; }

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

        #region Public methods

        /// <summary>
        /// Applty to the field.
        /// </summary>
        [NotNull]
        public RecordField ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");
            field
                .ApplySubField('a', Postcode)
                .ApplySubField('b', Country)
                .ApplySubField('c', City)
                .ApplySubField('d', Street)
                .ApplySubField('e', Building)
                .ApplySubField('g', Entrance)
                .ApplySubField('h', Apartment)
                .ApplySubField('f', AdditionalData);

            return field;
        }

        /// <summary>
        /// Разбор поля 13.
        /// </summary>
        [CanBeNull]
        public static ReaderAddress Parse
            (
                [CanBeNull] RecordField field
            )
        {
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            return new ReaderAddress
            {
                Postcode = field.GetFirstSubFieldValue('A'),
                Country = field.GetFirstSubFieldValue('B'),
                City = field.GetFirstSubFieldValue('C'),
                Street = field.GetFirstSubFieldValue('D'),
                Building = field.GetFirstSubFieldValue('E'),
                Entrance = field.GetFirstSubFieldValue('G'),
                Apartment = field.GetFirstSubFieldValue('H'),
                AdditionalData = field.GetFirstSubFieldValue('F'),
                UnknownSubFields = field.SubFields.GetUnknownSubFields(KnownCodes),
                Field = field
            };
        }

        /// <summary>
        /// Разбор поля 13.
        /// </summary>
        [CanBeNull]
        public static ReaderAddress Parse
            (
                [NotNull]MarcRecord record,
                int tag
            )
        {
            Code.NotNull(record, "record");

            RecordField field = record.Fields
                .GetField(tag)
                .FirstOrDefault();

            return ReferenceEquals(field, null)
                ? null
                : Parse(field);
        }

        /// <summary>
        /// Разбор поля 13.
        /// </summary>
        [CanBeNull]
        public static ReaderAddress Parse
            (
                [NotNull]MarcRecord record
            )
        {
            return Parse
                (
                    record,
                    Tag
                );
        }

        /// <summary>
        /// Преобразование обратно в поле.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag)
                .AddNonEmptySubField('a', Postcode)
                .AddNonEmptySubField('b', Country)
                .AddNonEmptySubField('c', City)
                .AddNonEmptySubField('d', Street)
                .AddNonEmptySubField('e', Building)
                .AddNonEmptySubField('g', Entrance)
                .AddNonEmptySubField('h', Apartment)
                .AddNonEmptySubField('f', AdditionalData)
                .AddSubFields(UnknownSubFields);

            return result;
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Postcode = reader.ReadNullableString();
            Country = reader.ReadNullableString();
            City = reader.ReadNullableString();
            Street = reader.ReadNullableString();
            Building = reader.ReadNullableString();
            Entrance = reader.ReadNullableString();
            Apartment = reader.ReadNullableString();
            AdditionalData = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WriteNullable(Postcode);
            writer.WriteNullable(Country);
            writer.WriteNullable(City);
            writer.WriteNullable(Street);
            writer.WriteNullable(Building);
            writer.WriteNullable(Entrance);
            writer.WriteNullable(Apartment);
            writer.WriteNullable(AdditionalData);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify(bool throwOnError)
        {
            Verifier<ReaderAddress> verifier
                = new Verifier<ReaderAddress>(this, throwOnError);

            bool haveAnyNonNull = new[]
                {
                    Postcode,
                    Country,
                    City,
                    Street,
                    Building,
                    Entrance,
                    Apartment,
                    AdditionalData
                }
                .NonNullItems()
                .Count() != 0;

            verifier.Assert(haveAnyNonNull, "address is empty");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            string[] list = new[]
                {
                    Postcode,
                    Country,
                    City,
                    Street,
                    Building,
                    Entrance,
                    Apartment,
                    AdditionalData
                }
                .NonNullItems()
                .ToArray();

            return string.Join
                (
                    ", ",
                    list
                );
        }

        #endregion
    }
}
