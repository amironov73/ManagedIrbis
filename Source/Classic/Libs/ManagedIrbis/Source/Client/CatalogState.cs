// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CatalogState.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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

namespace ManagedIrbis.Client
{
    /// <summary>
    /// State of the catalog.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("database")]
    [DebuggerDisplay("{Database} {Date} {MaxMfn}")]
    public sealed class CatalogState
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Identifier for LiteDB.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Date.
        /// </summary>
        [XmlAttribute("date")]
        [JsonProperty("date", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("database")]
        [JsonProperty("database", NullValueHandling = NullValueHandling.Ignore)]
        public string Database { get; set; }

        /// <summary>
        /// Maximal MFN.
        /// </summary>
        [XmlAttribute("maxMfn")]
        [JsonProperty("maxMfn", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int MaxMfn { get; set; }

        /// <summary>
        /// Records.
        /// </summary>
        [CanBeNull]
        [XmlArray("records")]
        [XmlArrayItem("record")]
        [JsonProperty("records", NullValueHandling = NullValueHandling.Ignore)]
        public RecordState[] Records { get; set; }

        /// <summary>
        /// Logically deleted records.
        /// </summary>
        [CanBeNull]
        [XmlArray("logicallyDeleted")]
        [XmlArrayItem("mfn")]
        [JsonProperty("logicallyDeleted", NullValueHandling = NullValueHandling.Ignore)]
        public int[] LogicallyDeleted { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Should serialize the <see cref="Date"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeDate()
        {
            return Date != DateTime.MinValue;
        }

        /// <summary>
        /// Should serialize the <see cref="MaxMfn"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeMaxMfn()
        {
            return MaxMfn != 0;
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Id = reader.ReadPackedInt32();
            Date = reader.ReadDateTime();
            Database = reader.ReadNullableString();
            MaxMfn = reader.ReadPackedInt32();
            Records = reader.ReadNullableArray<RecordState>();
            LogicallyDeleted = reader.ReadNullableInt32Array();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WritePackedInt32(Id)
                .Write(Date)
                .WriteNullable(Database)
                .WritePackedInt32(MaxMfn)
                .WriteNullableArray(Records)
                .WriteNullableArray(LogicallyDeleted);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Database.ToVisibleString()
                + " "
                + Date.ToLongUniformString()
                + " "
                + MaxMfn.ToInvariantString();
        }

        #endregion
    }
}
