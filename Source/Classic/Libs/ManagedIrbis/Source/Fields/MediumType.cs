// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MediumType.cs --
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
    /// Поле 182 - тип средства.
    /// Введено в связи с ГОСТ 7.0.100-2018.
    /// </summary>
    [PublicAPI]
    public sealed class MediumType
    {
        #region Constants

        /// <summary>
        /// Tag number.
        /// </summary>
        public const int Tag = 182;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "a";

        #endregion

        #region Properties

        /// <summary>
        /// Код типа средства. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("medium-code")]
        [JsonProperty("medium", NullValueHandling = NullValueHandling.Ignore)]
        public string MediumCode { get; set; }

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

            field.ApplySubField('a', MediumCode);
        }

        /// <summary>
        /// Parse the field.
        /// </summary>
        [NotNull]
        public static MediumType Parse
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            MediumType result = new MediumType
            {
                MediumCode = field.GetFirstSubFieldValue('a')
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
                .AddNonEmptySubField('a', MediumCode);

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

            MediumCode = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WriteNullable(MediumCode);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "MediumCode: {0}",
                    MediumCode.ToVisibleString()
                );
        }

        #endregion
    }
}
