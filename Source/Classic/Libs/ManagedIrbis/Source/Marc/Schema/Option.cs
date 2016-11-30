// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Option.cs --
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
    /// Option.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("option")]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("[{Value}] {Name}")]
#endif
    public sealed class Option
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Name.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("value")]
        [JsonProperty("value")]
        public string Value { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse XML element.
        /// </summary>
        [NotNull]
        public static Option ParseElement
            (
                [NotNull] XElement element
            )
        {
            Code.NotNull(element, "element");

            Option result = new Option
            {
                Value = element.GetAttributeText("value"),
                Name = element.GetAttributeText("name")
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
            Name = reader.ReadNullableString();
            Value = reader.ReadNullableString();
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
                .WriteNullable(Value);
        }

        #endregion
    }
}

#endif

