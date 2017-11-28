// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MonitoringData.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Linq;
using System.Text;
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
    [XmlRoot("monitoring")]
    public sealed class MonitoringData
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Moment of time.
        /// </summary>
        [XmlAttribute("moment")]
        [JsonProperty("moment", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime Moment { get; set; }

        /// <summary>
        /// Number of running clients.
        /// </summary>
        [XmlAttribute("clients")]
        [JsonProperty("clients", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Clients { get; set; }

        /// <summary>
        /// Command count.
        /// </summary>
        [XmlAttribute("commands")]
        [JsonProperty("commands", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Commands { get; set; }

        /// <summary>
        /// Data for databases.
        /// </summary>
        [CanBeNull]
        [XmlElement("database")]
        [JsonProperty("databases", NullValueHandling = NullValueHandling.Ignore)]
        public DatabaseData[] Databases { get; set; }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            long ticks = reader.ReadInt64();
            Moment = new DateTime(ticks);
            Clients = reader.ReadPackedInt32();
            Commands = reader.ReadPackedInt32();
            Databases = reader.ReadNullableArray<DatabaseData>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.Write(Moment.Ticks);
            writer
                .WritePackedInt32(Clients)
                .WritePackedInt32(Commands)
                .WriteNullableArray(Databases);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append(Moment);
            if (!ReferenceEquals(Databases, null)
                && Databases.Length != 0)
            {
                result.Append(':');
                result.Append(StringUtility.Join(",", Databases.Select(d => d.Name)));
            }

            return result.ToString();
        }

        #endregion
    }
}
