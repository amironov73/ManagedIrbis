// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IsbnInfo.cs -- 
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
    /// ISBN и цена, поле 10.
    /// </summary>
    [PublicAPI]
    [XmlRoot("isbn")]
    [MoonSharpUserData]
    public sealed class IsbnInfo
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Known codes.
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
        [SubField('d')]
        [XmlElement("price")]
        [JsonProperty("price")]
        [Description("Цена общая для всех экземпляров")]
        [DisplayName("Цена общая для всех экземпляров")]
        public decimal Price { get; set; }

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
        /// Associated field.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [DisplayName("Поле с подполями")]
        public RecordField Field { get; set; }

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
                .ApplySubField
                    (
                        'd',
                        Price != 0 ? Price.ToInvariantString() : null
                    )
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

            List<IsbnInfo> result = new List<IsbnInfo>();
            foreach (RecordField field in record.Fields)
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

            IsbnInfo result = new IsbnInfo
            {
                Isbn = field.GetFirstSubFieldValue('a'),
                Refinement = field.GetFirstSubFieldValue('b'),
                Erroneous = field.GetFirstSubFieldValue('z'),
                Price = field.GetFirstSubFieldValue('d')
                    .SafeToDecimal(0.0m),
                Currency = field.GetFirstSubFieldValue('c'),
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
            RecordField result = new RecordField(Tag)
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
#if SILVERLIGHT

            throw new NotImplementedException();

#else

            Code.NotNull(reader, "reader");

            Isbn = reader.ReadNullableString();
            Refinement = reader.ReadNullableString();
            Erroneous = reader.ReadNullableString();
            Currency = reader.ReadNullableString();
            Price = reader.ReadDecimal();

#endif
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
#if SILVERLIGHT

            throw new NotImplementedException();

#else

            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Isbn)
                .WriteNullable(Refinement)
                .WriteNullable(Erroneous)
                .WriteNullable(Currency)
                .Write(Price);

#endif
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Isbn.ToVisibleString();
        }

        #endregion
    }
}
