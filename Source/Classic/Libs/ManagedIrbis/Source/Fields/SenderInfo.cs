// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SenderInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
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
    /// Отправитель для письма-заказа на книги, поле 15 в БД CMPL.
    /// </summary>
    [PublicAPI]
    [XmlRoot("sender")]
    [MoonSharpUserData]
    public sealed class SenderInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Tag.
        /// </summary>
        public const int Tag = 15;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abcdefgk";

        #endregion

        #region Properties

        /// <summary>
        /// Отправитель (1-я строка на конверте). Подполе f.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlElement("sender1")]
        [JsonProperty("sender1")]
        [Description("Отправитель (первая строка)")]
        [DisplayName("Отправитель (первая строка)")]
        public string Sender1 { get; set; }

        /// <summary>
        /// Отправитель (2-я строка на конверте). Подполе g.
        /// </summary>
        [CanBeNull]
        [SubField('g')]
        [XmlElement("sender2")]
        [JsonProperty("sender2")]
        [Description("Отправитель (вторая строка)")]
        [DisplayName("Отправитель (вторая строка)")]
        public string Sender2 { get; set; }

        /// <summary>
        /// Улица. Подполе d.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlElement("street")]
        [JsonProperty("street")]
        [Description("Улица")]
        [DisplayName("Улица")]
        public string Street { get; set; }

        /// <summary>
        /// Дом. Подполе e.
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        [XmlElement("house")]
        [JsonProperty("house")]
        [Description("Дом")]
        [DisplayName("Дом")]
        public string House { get; set; }

        /// <summary>
        /// Город. Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlElement("city")]
        [JsonProperty("city")]
        [Description("Город")]
        [DisplayName("Город")]
        public string City { get; set; }

        /// <summary>
        /// Страна. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlElement("country")]
        [JsonProperty("country")]
        [Description("Страна")]
        [DisplayName("Страна")]
        public string Country { get; set; }

        /// <summary>
        /// Почтовый индекс. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlElement("postcode")]
        [JsonProperty("postcode")]
        [Description("Почтовый индекс")]
        [DisplayName("Почтовый индекс")]
        public string Postcode { get; set; }

        /// <summary>
        /// Телефон. Подполе k.
        /// </summary>
        [CanBeNull]
        [SubField('k')]
        [XmlElement("phone")]
        [JsonProperty("phone")]
        [Description("Телефон")]
        [DisplayName("Телефон")]
        public string Phone { get; set; }

        /// <summary>
        /// Unknown subfields.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
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

        #endregion

        #region Private members

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
                .ApplySubField('f', Sender1)
                .ApplySubField('g', Sender2)
                .ApplySubField('d', Street)
                .ApplySubField('e', House)
                .ApplySubField('c', City)
                .ApplySubField('b', Country)
                .ApplySubField('a', Postcode)
                .ApplySubField('k', Phone);
        }

        /// <summary>
        /// Parse the <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public static SenderInfo ParseField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            SenderInfo result = new SenderInfo
            {
                Sender1 = field.GetFirstSubFieldValue('f'),
                Sender2 = field.GetFirstSubFieldValue('g'),
                Street = field.GetFirstSubFieldValue('d'),
                House = field.GetFirstSubFieldValue('e'),
                City = field.GetFirstSubFieldValue('c'),
                Country = field.GetFirstSubFieldValue('b'),
                Postcode = field.GetFirstSubFieldValue('a'),
                Phone = field.GetFirstSubFieldValue('k'),
                UnknownSubFields = field.SubFields.GetUnknownSubFields(KnownCodes),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Parse the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SenderInfo[] ParseRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            List<SenderInfo> result = new List<SenderInfo>();
            foreach (RecordField field in record.Fields)
            {
                if (field.Tag == Tag)
                {
                    result.Add(ParseField(field));
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Should serialize the <see cref="UnknownSubFields"/> array?
        /// </summary>
        public bool ShouldSerializeUnknownSubFields()
        {
            return !ArrayUtility.IsNullOrEmpty(UnknownSubFields);
        }

        /// <summary>
        /// Convert back to <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag)
                .AddNonEmptySubField('f', Sender1)
                .AddNonEmptySubField('g', Sender2)
                .AddNonEmptySubField('d', Street)
                .AddNonEmptySubField('e', House)
                .AddNonEmptySubField('c', City)
                .AddNonEmptySubField('b', Country)
                .AddNonEmptySubField('a', Postcode)
                .AddNonEmptySubField('k', Phone)
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

            Sender1 = reader.ReadNullableString();
            Sender2 = reader.ReadNullableString();
            Street = reader.ReadNullableString();
            House = reader.ReadNullableString();
            City = reader.ReadNullableString();
            Country = reader.ReadNullableString();
            Postcode = reader.ReadNullableString();
            Phone = reader.ReadNullableString();
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
                .WriteNullable(Sender1)
                .WriteNullable(Sender2)
                .WriteNullable(Street)
                .WriteNullable(House)
                .WriteNullable(City)
                .WriteNullable(Country)
                .WriteNullable(Postcode)
                .WriteNullable(Phone)
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
            Verifier<SenderInfo> verifier
                = new Verifier<SenderInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Postcode, "Postcode")
                .NotNullNorEmpty(Sender1, "Sender1")
                .NotNullNorEmpty(Street, "Street")
                .NotNullNorEmpty(House, "House");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Индекс: {0}{5}Город: {1}{5}Улица: {2}{5}Дом: {3}{5}"
                    + "Отправитель: {4}",
                    Postcode.ToVisibleString(),
                    City.ToVisibleString(),
                    Street.ToVisibleString(),
                    House.ToVisibleString(),
                    Sender1.ToVisibleString(),
                    Environment.NewLine
                );
        }

        #endregion
    }
}
