/* Example.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
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

namespace ManagedIrbis.Marc.Schema
{
    /// <summary>
    /// Example.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("example")]
    [DebuggerDisplay("{Text}")]
    public sealed class Example
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Number.
        /// </summary>
        [XmlAttribute("number")]
        [JsonProperty("number")]
        public int Number { get; set; }

        /// <summary>
        /// Text.
        /// </summary>
        [CanBeNull]
        [XmlText]
        [JsonProperty("text")]
        public string Text { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse XML element.
        /// </summary>
        [NotNull]
        public static Example ParseElement
            (
                [NotNull] XElement element
            )
        {
            Code.NotNull(element, "element");

            Example result = new Example
            {
                Number = element.GetAttributeInt32("N", 0),
                Text = element.GetInnerXml()
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
            Number = reader.ReadPackedInt32();
            Text = reader.ReadNullableString();
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
                .WritePackedInt32(Number)
                .WriteNullable(Text);
        }

        #endregion

        #region Object members
        
        #endregion
    }
}
