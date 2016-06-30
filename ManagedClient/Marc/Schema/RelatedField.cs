/* RelatedField.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Marc.Schema
{
    /// <summary>
    /// Related field.
    /// </summary>
    [PublicAPI]
    [Serializable]
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
        [CanBeNull]
        [XmlAttribute("tag")]
        [JsonProperty("tag")]
        public string Tag { get; set; }

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
                Tag = element.GetAttributeText("tag", null),
                Name = element.GetAttributeText("name", null),
                Description = element.GetInnerXml("DESCRIPTION")
            };

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore object state from the given stream
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Description = reader.ReadNullableString();
            Name = reader.ReadNullableString();
            Tag = reader.ReadNullableString();
        }

        /// <summary>
        /// Save object stat to the given stream
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Description)
                .WriteNullable(Name)
                .WriteNullable(Tag);
        }

        #endregion

        #region Object members
        
        #endregion
    }
}
