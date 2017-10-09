// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PingData.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Statistics
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("ping")]
    public struct PingData
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Moment.
        /// </summary>
        [XmlElement("moment")]
        [JsonProperty("moment")]
        public DateTime Moment { get; set; }

        /// <summary>
        /// Success.
        /// </summary>
        [XmlElement("success")]
        [JsonProperty("success", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Success { get; set; }

        /// <summary>
        /// Roundtrip time.
        /// </summary>
        [XmlElement("roundtrip")]
        [JsonProperty("roundtrip")]
        public int RoundTripTime { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Should serialize the <see cref="Success"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSuccess()
        {
            return Success;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Moment = new DateTime(reader.ReadPackedInt64());
            Success = reader.ReadBoolean();
            RoundTripTime = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WritePackedInt64(Moment.Ticks);
            writer.Write(Success);
            writer.WritePackedInt32(RoundTripTime);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="ValueType.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "{0:HH:mm:ss} {1} {2}",
                    Moment,
                    Success,
                    RoundTripTime
                );
        }

        #endregion
    }
}
