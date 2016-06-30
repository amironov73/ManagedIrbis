/* IndicatorSchema.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

using AM.Collections;
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
    /// 
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [XmlRoot("indicator")]
    [DebuggerDisplay("{Name}")]
    public sealed class IndicatorSchema
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
        /// Name.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Options.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        [XmlArray("options")]
        [XmlElement("option")]
        public NonNullCollection<Option> Options
        {
            get { return _options; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IndicatorSchema()
        {
            _options = new NonNullCollection<Option>();
        }

        #endregion

        #region Private members

        private readonly NonNullCollection<Option> _options;

        #endregion

        #region Public methods

        /// <summary>
        /// Parse XML element.
        /// </summary>
        [NotNull]
        public static IndicatorSchema ParseElement
            (
                [NotNull] XElement element
            )
        {
            Code.NotNull(element, "element");

            IndicatorSchema result = new IndicatorSchema
            {
                Name = element.GetAttributeText("name", null),
                Description = element.GetElementText("DESCRIPTION", null)
            };

            foreach (XElement subElement in element.Elements("OPTION"))
            {
                Option option = Option.ParseElement(subElement);
                result.Options.Add(option);
            }

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
            Name = reader.ReadNullableString();
            Description = reader.ReadNullableString();
            reader.ReadCollection(Options);
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
                .WriteNullable(Name)
                .WriteNullable(Description)
                .WriteCollection(Options);
        }

        #endregion

        #region Object members

        #endregion
    }
}
