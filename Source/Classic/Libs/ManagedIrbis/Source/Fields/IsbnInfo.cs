// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IsbnInfo.cs -- информация об ISBN и цене
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
    /// ISBN и цена, поле 10.
    /// </summary>
    [PublicAPI]
    [XmlRoot("isbn")]
    [MoonSharpUserData]
    public sealed class IsbnInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abcdz";

        /// <summary>
        /// Tag.
        /// </summary>
        public const int Tag = 10;

        #endregion

        #region Properties

        /// <summary>
        /// ISBN.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlElement("isbn")]
        [JsonProperty("isbn")]
        [Description("ISBN. Подполе a.")]
        [DisplayName("ISBN")]
        public string Isbn { get; set; }

        /// <summary>
        /// Уточнение. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlElement("refinement")]
        [JsonProperty("refinement")]
        [Description("Уточнение")]
        [DisplayName("Уточнение")]
        public string Refinement { get; set; }

        /// <summary>
        /// Ошибочный ISBN. Подполе z.
        /// </summary>
        [CanBeNull]
        [SubField('z')]
        [XmlElement("erroneous")]
        [JsonProperty("erroneous")]
        [Description("Ошибочный ISBN")]
        [DisplayName("Ошибочный ISBN")]
        public string Erroneous { get; set; }

        /// <summary>
        /// Цена общая для всех экземпляров. Подполе d.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlElement("price")]
        [JsonProperty("price")]
        [Description("Цена общая для всех экземпляров")]
        [DisplayName("Цена общая для всех экземпляров")]
        public string PriceString { get; set; }

        /// <summary>
        /// Цена общая для всех экземпляров. Подполе d.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public decimal Price
        {
            get { return PriceString.SafeToDecimal(0.0m); }
            set
            {
                PriceString = value.ToString("#.00", CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Обозначение валюты. Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlElement("currency")]
        [JsonProperty("currency")]
        [Description("Обозначение валюты")]
        [DisplayName("Обозначение валюты")]
        public string Currency { get; set; }

        /// <summary>
        /// Unknown subfields.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
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
        /// Apply the <see cref="IsbnInfo"/>
        /// to the <see cref="RecordField"/>.
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField('a', Isbn)
                .ApplySubField('b', Refinement)
                .ApplySubField('z', Erroneous)
                .ApplySubField('d', PriceString)
                .ApplySubField('c', Currency);
        }

        /// <summary>
        /// Parse the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static IsbnInfo[] ParseRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            var result = new List<IsbnInfo>();
            foreach (var field in record.Fields)
            {
                if (field.Tag == Tag)
                {
                    IsbnInfo isbn = ParseField(field);
                    result.Add(isbn);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        [NotNull]
        public static IsbnInfo ParseField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            var result = new IsbnInfo
            {
                Isbn = field.GetFirstSubFieldValue('a'),
                Refinement = field.GetFirstSubFieldValue('b'),
                Erroneous = field.GetFirstSubFieldValue('z'),
                PriceString = field.GetFirstSubFieldValue('d'),
                Currency = field.GetFirstSubFieldValue('c'),
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
        /// Transform back to field.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            var result = new RecordField(Tag)
                .AddNonEmptySubField('a', Isbn)
                .AddNonEmptySubField('b', Refinement)
                .AddNonEmptySubField('z', Erroneous)
                .AddNonEmptySubField
                    (
                        'd',
                        Price != 0.0m ? Price.ToInvariantString() : null
                    )
                .AddNonEmptySubField('c', Currency);

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

            Isbn = reader.ReadNullableString();
            Refinement = reader.ReadNullableString();
            Erroneous = reader.ReadNullableString();
            Currency = reader.ReadNullableString();
            PriceString = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Isbn)
                .WriteNullable(Refinement)
                .WriteNullable(Erroneous)
                .WriteNullable(Currency)
                .WriteNullable(PriceString);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<IsbnInfo>(this, throwOnError);

            verifier.Assert
                (
                    !string.IsNullOrEmpty(PriceString)
                    || !string.IsNullOrEmpty(Isbn)
                    || !string.IsNullOrEmpty(Erroneous)
                );

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Isbn))
            {
                if (string.IsNullOrEmpty(PriceString))
                {
                    return "(null)";
                }
                return PriceString;
            }

            if (string.IsNullOrEmpty(PriceString))
            {
                return Isbn;
            }
            return Isbn + " : " + PriceString;
        }

        #endregion

    } // 
}
