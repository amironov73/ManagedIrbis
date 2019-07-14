// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ContentType.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Поле 181 - вид содержания.
    /// Введено в связи с ГОСТ 7.0.100-2018.
    /// </summary>
    [PublicAPI]
    public sealed class ContentType
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Tag number.
        /// </summary>
        public const int Tag = 181;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abcdef";

        #endregion

        #region Propertites

        /// <summary>
        /// Вид содержания. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("content-kind")]
        [JsonProperty("contentKind", NullValueHandling = NullValueHandling.Ignore)]
        public string ContentKind { get; set; }

        /// <summary>
        /// Степень применимости. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("degree-of-applicability")]
        [JsonProperty("degreeOfApplicability", NullValueHandling = NullValueHandling.Ignore)]
        public string DegreeOfApplicability { get; set; }

        /// <summary>
        /// Спецификация типа. Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("type-specification")]
        [JsonProperty("typeSpecification", NullValueHandling = NullValueHandling.Ignore)]
        public string TypeSpecification { get; set; }

        /// <summary>
        /// Спецификация движения. Подполе d.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("movement-specification")]
        [JsonProperty("movementSpecification", NullValueHandling = NullValueHandling.Ignore)]
        public string MovementSpecification { get; set; }

        /// <summary>
        /// Спецификация размерности. Подполе e.
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        [XmlAttribute("dimension-specification")]
        [JsonProperty("dimensionSpecification", NullValueHandling = NullValueHandling.Ignore)]
        public string DimensionSpecification { get; set; }

        /// <summary>
        /// Сенсорная спецификация. Подполе f.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlAttribute("sensory-specification")]
        [JsonProperty("sensorySpecification", NullValueHandling = NullValueHandling.Ignore)]
        public string SensorySpecification { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the <see cref="ContentType"/>
        /// to the <see cref="RecordField"/>.
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField('a', ContentKind)
                .ApplySubField('b', DegreeOfApplicability)
                .ApplySubField('c', TypeSpecification)
                .ApplySubField('d', MovementSpecification)
                .ApplySubField('e', DimensionSpecification)
                .ApplySubField('f', SensorySpecification);
        }

        /// <summary>
        /// Parse the field.
        /// </summary>
        [NotNull]
        public static ContentType Parse
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            ContentType result = new ContentType
            {
                ContentKind = field.GetFirstSubFieldValue('a'),
                DegreeOfApplicability = field.GetFirstSubFieldValue('b'),
                TypeSpecification = field.GetFirstSubFieldValue('c'),
                MovementSpecification = field.GetFirstSubFieldValue('d'),
                DimensionSpecification = field.GetFirstSubFieldValue('e'),
                SensorySpecification = field.GetFirstSubFieldValue('f')
            };

            return result;
        }

        /// <summary>
        /// Convert <see cref="ContentType"/>
        /// back to <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(Tag)
                .AddNonEmptySubField('a', ContentKind)
                .AddNonEmptySubField('b', DegreeOfApplicability)
                .AddNonEmptySubField('c', TypeSpecification)
                .AddNonEmptySubField('d', MovementSpecification)
                .AddNonEmptySubField('e', DimensionSpecification)
                .AddNonEmptySubField('f', SensorySpecification);

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

            ContentKind = reader.ReadNullableString();
            DegreeOfApplicability = reader.ReadNullableString();
            TypeSpecification = reader.ReadNullableString();
            MovementSpecification = reader.ReadNullableString();
            DimensionSpecification = reader.ReadNullableString();
            SensorySpecification = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(ContentKind)
                .WriteNullable(DegreeOfApplicability)
                .WriteNullable(TypeSpecification)
                .WriteNullable(MovementSpecification)
                .WriteNullable(DimensionSpecification)
                .WriteNullable(SensorySpecification);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "ContentKind: {0}, DegreeOfApplicability: {1}, "
                    + "TypeSpecification: {2}, MovementSpecification: {3}, "
                    + "DimensionSpecification: {4}, SensorySpecification: {5}",
                    ContentKind.ToVisibleString(),
                    DegreeOfApplicability.ToVisibleString(),
                    TypeSpecification.ToVisibleString(),
                    MovementSpecification.ToVisibleString(),
                    DimensionSpecification.ToVisibleString(),
                    SensorySpecification.ToVisibleString()
                );
        }

        #endregion
    }
}
