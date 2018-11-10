// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PerfRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Performance
{
    /// <summary>
    /// Запись о произведенной сетевой транзакции.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("transaction")]
    public sealed class PerfRecord
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Moment.
        /// </summary>
        [XmlAttribute("moment")]
        [JsonProperty("moment", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime Moment { get; set; }

        /// <summary>
        /// Код операции.
        /// </summary>
        [XmlAttribute("code")]
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        /// <summary>
        /// Размер исходящего пакета (байты).
        /// </summary>
        [XmlAttribute("outgoing")]
        [JsonProperty("outgoing", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int OutgoingSize { get; set; }

        /// <summary>
        /// Размер входящего пакета (байты).
        /// </summary>
        [XmlAttribute("incoming")]
        [JsonProperty("incoming", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int IncomingSize { get; set; }

        /// <summary>
        /// Затрачено времени (миллисекунды).
        /// </summary>
        [XmlAttribute("elapsed")]
        [JsonProperty("elapsed", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long ElapsedTime { get; set; }

        /// <summary>
        /// Сообщение об ошибке (если есть).
        /// </summary>
        [XmlElement("error")]
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorMessage { get; set; }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            CodeJam.Code.NotNull(reader, "reader");

            Moment = reader.ReadDateTime();
            Code = reader.ReadNullableString();
            OutgoingSize = reader.ReadPackedInt32();
            IncomingSize = reader.ReadPackedInt32();
            ElapsedTime = reader.ReadPackedInt64();
            ErrorMessage = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            CodeJam.Code.NotNull(writer, "writer");

            writer
                .Write(Moment)
                .WriteNullable(Code)
                .WritePackedInt32(OutgoingSize)
                .WritePackedInt32(IncomingSize)
                .WritePackedInt64(ElapsedTime)
                .WriteNullable(ErrorMessage);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    CultureInfo.InvariantCulture,
                    "{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
                    Moment,
                    Code,
                    OutgoingSize,
                    IncomingSize,
                    ElapsedTime,
                    ErrorMessage
                );
        }

        #endregion
    }
}
