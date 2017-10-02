// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MenuEntry.cs -- MNU file entry
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Menus
{
    /// <summary>
    /// MNU file entry. Represents two lines.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("entry")]
    [DebuggerDisplay("{Code} = {Comment}")]
    public sealed class MenuEntry
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// First line -- the code.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("code")]
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Second line -- the comment.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("comment")]
        [JsonProperty("comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Other menu entry.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public MenuEntry OtherEntry { get; set; }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code = reader.ReadNullableString();
            Comment = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Code)
                .WriteNullable(Comment);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Should JSON serialize the comment?
        /// </summary>
        public bool ShouldSerializeComment()
        {
            return !string.IsNullOrEmpty(Comment);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Comment))
            {
                return Code.ToVisibleString();
            }

            return string.Format
                (
                    "{0} - {1}",
                    Code,
                    Comment
                );
        }

        #endregion
    }
}
