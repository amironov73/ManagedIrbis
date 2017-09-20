// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RelatedField.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !SILVERLIGHT

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Marc.Schema
{
    /// <summary>
    /// Related field.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("related")]
    [DebuggerDisplay("[{Tag}] {Name}")]
    public sealed class RelatedField
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Description.
        /// </summary>
        [CanBeNull]
        [XmlElement("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Field.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public FieldSchema Field { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Tag.
        /// </summary>
        [XmlAttribute("tag")]
        [JsonProperty("tag")]
        public int Tag { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse XML element.
        /// </summary>
        [NotNull]
        public static RelatedField ParseElement
            (
                [NotNull] XElement element
            )
        {
            Code.NotNull(element, "element");

            RelatedField result = new RelatedField
            {
                Tag = element.GetAttributeText("tag", null).SafeToInt32(),
                Name = element.GetAttributeText("name", null),
                Description = element.GetInnerXml("DESCRIPTION")
            };

            return result;
        }

        /// <summary>
        /// Should serialize the <see cref="Description"/> field?
        /// </summary>
        public bool ShouldSerializeDescription()
        {
            return !string.IsNullOrEmpty(Description);
        }

        /// <summary>
        /// Should serialize the <see cref="Name"/> field?
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeName()
        {
            return !string.IsNullOrEmpty(Name);
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

            Description = reader.ReadNullableString();
            Name = reader.ReadNullableString();
            Tag = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Description)
                .WriteNullable(Name)
                .WritePackedInt32(Tag);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "[{0}]: {1}",
                    Tag,
                    Name.ToVisibleString()
                );
        }

        #endregion
    }
}

#endif

