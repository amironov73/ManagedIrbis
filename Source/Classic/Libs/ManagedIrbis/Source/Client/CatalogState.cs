// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CatalogState.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    /// State of the catalog.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{Database}")]
    public sealed class CatalogState
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        [JsonProperty("database")]
        [XmlAttribute("database")]
        public string Database { get; set; }

        /// <summary>
        /// Maximal MFN.
        /// </summary>
        [JsonProperty("maxMfn")]
        [XmlAttribute("maxMfn")]
        public int MaxMfn { get; set; }

        /// <summary>
        /// Records.
        /// </summary>
        [CanBeNull]
        [JsonProperty("records")]
        [XmlArray("records")]
        [XmlArrayItem("record")]
        public RecordState[] Records { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Database = reader.ReadNullableString();
            MaxMfn = reader.ReadPackedInt32();
            Records = reader.ReadNullableArray<RecordState>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Database)
                .WritePackedInt32(MaxMfn)
                .WriteNullableArray(Records);
        }

        #endregion
    }
}
