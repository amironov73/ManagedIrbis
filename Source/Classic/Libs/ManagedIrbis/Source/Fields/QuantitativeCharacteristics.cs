// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* QuantitativeCharacteristics.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

// ReSharper disable ConvertClosureToMethodGroup

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Количественные характеристики, поле 215.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class QuantitativeCharacteristics
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "012378acdex";

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const string Tag = "215";

        #endregion

        #region Properties

        /// <summary>
        /// Объем (цифры), подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("volume")]
        [JsonProperty("volume")]
        public string Volume { get; set; }

        /// <summary>
        /// Единица измерения, подполе 1.
        /// </summary>
        [CanBeNull]
        [SubField('1')]
        [XmlAttribute("unit")]
        [JsonProperty("unit")]
        public string Unit { get; set; }

        /// <summary>
        /// Иллюстрации, подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("illustrations1")]
        [JsonProperty("illustrations1")]
        public string Illustrations1 { get; set; }

        /// <summary>
        /// Иллюстрации, подполе 0.
        /// </summary>
        [CanBeNull]
        [SubField('0')]
        [XmlAttribute("illustrations2")]
        [JsonProperty("illustrations2")]
        public string Illustrations2 { get; set; }

        /// <summary>
        /// Иллюстрации, подполе 7.
        /// </summary>
        [CanBeNull]
        [SubField('7')]
        [XmlAttribute("illustrations3")]
        [JsonProperty("illustrations3")]
        public string Illustrations3 { get; set; }

        /// <summary>
        /// Иллюстрации, подполе 8.
        /// </summary>
        [CanBeNull]
        [SubField('8')]
        [XmlAttribute("illustrations4")]
        [JsonProperty("illustrations4")]
        public string Illustrations4 { get; set; }

        /// <summary>
        /// Сопроводительный материал, подполе e.
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        [XmlAttribute("accompanyingMaterial1")]
        [JsonProperty("accompanyingMaterial1")]
        public string AccompanyingMaterial1 { get; set; }

        /// <summary>
        /// Сопроводительный материал, подполе 2.
        /// </summary>
        [CanBeNull]
        [SubField('2')]
        [XmlAttribute("accompanyingMaterial2")]
        [JsonProperty("accompanyingMaterial2")]
        public string AccompanyingMaterial2 { get; set; }

        /// <summary>
        /// Размер текстовых материалов, нот, карт, подполе d.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("size")]
        [JsonProperty("size")]
        public string Size { get; set; }

        /// <summary>
        /// Вид упаковки (в переплете и др.), подполе 3.
        /// </summary>
        [CanBeNull]
        [SubField('3')]
        [XmlAttribute("packaging")]
        [JsonProperty("packaging")]
        public string Packaging { get; set; }

        /// <summary>
        /// Тираж (цифры), подполе x.
        /// </summary>
        [CanBeNull]
        [SubField('x')]
        [XmlAttribute("copies")]
        [JsonProperty("copies")]
        public string NumberOfCopies { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public QuantitativeCharacteristics()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public QuantitativeCharacteristics
            (
                [CanBeNull] string volume
            )
        {
            Volume = volume;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public QuantitativeCharacteristics
            (
                [CanBeNull] string volume,
                [CanBeNull] string unit,
                [CanBeNull] string illustrations1
            )
        {
            Volume = volume;
            Unit = unit;
            Illustrations1 = illustrations1;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the field.
        /// </summary>
        [NotNull]
        public static QuantitativeCharacteristics Parse
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            // TODO: support for unknown subfields

            QuantitativeCharacteristics result
                = new QuantitativeCharacteristics
                {
                    Volume = field.GetFirstSubFieldValue('a'),
                    Unit = field.GetFirstSubFieldValue('1'),
                    Illustrations1 = field.GetFirstSubFieldValue('c'),
                    Illustrations2 = field.GetFirstSubFieldValue('0'),
                    Illustrations3 = field.GetFirstSubFieldValue('7'),
                    Illustrations4 = field.GetFirstSubFieldValue('8'),
                    AccompanyingMaterial1 = field.GetFirstSubFieldValue('e'),
                    AccompanyingMaterial2 = field.GetFirstSubFieldValue('2'),
                    Size = field.GetFirstSubFieldValue('d'),
                    Packaging = field.GetFirstSubFieldValue('3'),
                    NumberOfCopies = field.GetFirstSubFieldValue('x')
                };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static QuantitativeCharacteristics[] Parse
            (
                [NotNull] MarcRecord record,
                string tag
            )
        {
            Code.NotNull(record, "record");
            Code.NotNullNorEmpty(tag, "tag");


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
        public static QuantitativeCharacteristics[] Parse
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
        /// Should serialize <see cref="Volume"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeVolume()
        {
            return !ReferenceEquals(Volume, null);
        }

        /// <summary>
        /// Should serialize <see cref="Unit"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeUnit()
        {
            return !ReferenceEquals(Unit, null);
        }

        /// <summary>
        /// Should serialize <see cref="Illustrations1"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeIllustrations1()
        {
            return !ReferenceEquals(Illustrations1, null);
        }

        /// <summary>
        /// Should serialize <see cref="Illustrations2"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeIllustrations2()
        {
            return !ReferenceEquals(Illustrations2, null);
        }

        /// <summary>
        /// Should serialize <see cref="Illustrations3"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeIllustrations3()
        {
            return !ReferenceEquals(Illustrations3, null);
        }

        /// <summary>
        /// Should serialize <see cref="Illustrations4"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeIllustrations4()
        {
            return !ReferenceEquals(Illustrations4, null);
        }

        /// <summary>
        /// Should serialize <see cref="AccompanyingMaterial1"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeAccompanyingMaterial1()
        {
            return !ReferenceEquals(AccompanyingMaterial1, null);
        }

        /// <summary>
        /// Should serialize <see cref="AccompanyingMaterial2"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeAccompanyingMaterial2()
        {
            return !ReferenceEquals(AccompanyingMaterial2, null);
        }

        /// <summary>
        /// Should serialize <see cref="Size"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSize()
        {
            return !ReferenceEquals(Size, null);
        }

        /// <summary>
        /// Should serialize <see cref="Packaging"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePackaging()
        {
            return !ReferenceEquals(Packaging, null);
        }

        /// <summary>
        /// Should serialize <see cref="NumberOfCopies"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeNumberOfCopies()
        {
            return !ReferenceEquals(NumberOfCopies, null);
        }

        /// <summary>
        /// Превращение обратно в поле.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag)
                .AddNonEmptySubField('a', Volume)
                .AddNonEmptySubField('1', Unit)
                .AddNonEmptySubField('c', Illustrations1)
                .AddNonEmptySubField('0', Illustrations2)
                .AddNonEmptySubField('7', Illustrations3)
                .AddNonEmptySubField('8', Illustrations4)
                .AddNonEmptySubField('e', AccompanyingMaterial1)
                .AddNonEmptySubField('2', AccompanyingMaterial2)
                .AddNonEmptySubField('d', Size)
                .AddNonEmptySubField('3', Packaging)
                .AddNonEmptySubField('x', NumberOfCopies);

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

            Volume = reader.ReadNullableString();
            Unit = reader.ReadNullableString();
            Illustrations1 = reader.ReadNullableString();
            Illustrations2 = reader.ReadNullableString();
            Illustrations3 = reader.ReadNullableString();
            Illustrations4 = reader.ReadNullableString();
            AccompanyingMaterial1 = reader.ReadNullableString();
            AccompanyingMaterial2 = reader.ReadNullableString();
            Size = reader.ReadNullableString();
            Packaging = reader.ReadNullableString();
            NumberOfCopies = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        void IHandmadeSerializable.SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Volume)
                .WriteNullable(Unit)
                .WriteNullable(Illustrations1)
                .WriteNullable(Illustrations2)
                .WriteNullable(Illustrations3)
                .WriteNullable(Illustrations4)
                .WriteNullable(AccompanyingMaterial1)
                .WriteNullable(AccompanyingMaterial2)
                .WriteNullable(Size)
                .WriteNullable(Packaging)
                .WriteNullable(NumberOfCopies);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
            (
                "Volume: {0}, Illustrations: {1}",
                Volume.ToVisibleString(),
                Illustrations1.ToVisibleString()
            );
        }

        #endregion
    }
}
