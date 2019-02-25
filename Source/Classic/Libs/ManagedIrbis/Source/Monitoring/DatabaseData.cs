// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DatabaseData.cs --
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

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Monitoring
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("database")]
    public sealed class DatabaseData
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("name")]
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Number of deleted records.
        /// </summary>
        [XmlAttribute("deletedRecords")]
        [JsonProperty("deletedRecords", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int DeletedRecords { get; set; }

        /// <summary>
        /// Array of locked records.
        /// </summary>
        [CanBeNull]
        [XmlArray("locked")]
        [XmlArrayItem("mfn")]
        [JsonProperty("lockedRecords", NullValueHandling = NullValueHandling.Ignore)]
        public int[] LockedRecords { get; set; }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Name = reader.ReadNullableString();
            DeletedRecords = reader.ReadPackedInt32();
            LockedRecords = reader.ReadNullableInt32Array();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Name)
                .WritePackedInt32(DeletedRecords)
                .WriteNullableArray(LockedRecords);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Name.ToVisibleString();
        }

        #endregion
    }
}
