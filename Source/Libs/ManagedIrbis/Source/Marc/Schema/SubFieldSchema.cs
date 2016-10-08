/* SubFieldSchema.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !SILVERLIGHT

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

using System.Xml.Linq;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Marc.Schema
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("subfield")]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("[{Code}] {Name}")]
#endif
    public sealed class SubFieldSchema
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Code.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public char Code { get; set; }

        /// <summary>
        /// For serialization.
        /// </summary>
        [CanBeNull]
#if !WINMOBILE && !PocketPC && !UAP
        [Browsable(false)]
#endif
        [XmlAttribute("code")]
        [JsonProperty("code")]
        public string CodeString
        {
            get { return Code.ToString(); }
            set
            {
                Code = string.IsNullOrEmpty(value)
                    ? '\0'
                    : value[0];
            }
        }

        /// <summary>
        /// Description.
        /// </summary>
        [CanBeNull]
        [XmlElement("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Display.
        /// </summary>
        [XmlAttribute("display")]
        [JsonProperty("display")]
        public bool Display { get; set; }

        /// <summary>
        /// Mandatory?
        /// </summary>
        [XmlAttribute("mandatory")]
        [JsonProperty("mandatory")]
        public bool Mandatory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        [XmlElement("mandatory-text")]
        [JsonProperty("mandatory-text")]
        public string MandatoryText { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Repeatable?
        /// </summary>
        [XmlAttribute("repeatable")]
        [JsonProperty("repeatable")]
        public bool Repeatable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        [XmlElement("repeatable-text")]
        [JsonProperty("repeatable-text")]
        public string RepeatableText { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members


        #endregion

        #region Public methods

        /// <summary>
        /// Parse XML element.
        /// </summary>
        [NotNull]
        public static SubFieldSchema ParseElement
            (
                [NotNull] XElement element
            )
        {
            CodeJam.Code.NotNull(element, "element");

            SubFieldSchema result = new SubFieldSchema
            {
                Code = element.GetAttributeCharacter("tag"),
                Name = element.GetAttributeText("name", null),
                Mandatory = element.GetAttributeBoolean("mandatory", false),
                MandatoryText = element.GetAttributeText("nm", null),
                Repeatable = element.GetAttributeBoolean("repeatable", false),
                RepeatableText = element.GetAttributeText("nr", null),
                Description = element.GetInnerXml("DESCRIPTION"),
                Display = element.GetAttributeBoolean("display", false)
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
            Code = reader.ReadChar();
            Description = reader.ReadNullableString();
            Display = reader.ReadBoolean();
            Mandatory = reader.ReadBoolean();
            MandatoryText = reader.ReadNullableString();
            Name = reader.ReadNullableString();
            Repeatable = reader.ReadBoolean();
            RepeatableText = reader.ReadNullableString();
        }

        /// <summary>
        /// Save object stat to the given stream
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.Write(Code);
            writer.WriteNullable(Description);
            writer.Write(Display);
            writer.Write(Mandatory);
            writer.WriteNullable(MandatoryText);
            writer.WriteNullable(Name);
            writer.Write(Repeatable);
            writer.WriteNullable(RepeatableText);
        }

        #endregion


        #region Object members

        #endregion
    }
}

#endif

