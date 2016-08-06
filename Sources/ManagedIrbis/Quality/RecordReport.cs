/* RecordReport.cs -- отчёт о проверке записи.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Quality
{
    /// <summary>
    /// Отчёт о проверке записи.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RecordReport
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// MFN записи.
        /// </summary>
        [JsonProperty("mfn")]
        [XmlAttribute("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Шифр записи.
        /// </summary>
        [CanBeNull]
        [JsonProperty("index")]
        [XmlAttribute("index")]
        public string Index { get; set; }

        /// <summary>
        /// Краткое БО.
        /// </summary>
        [CanBeNull]
        [JsonProperty("description")]
        [XmlAttribute("description")]
        public string Description { get; set; }

        /// <summary>
        /// Дефекты.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        [JsonProperty("defects")]
        [XmlArray("defects")]
        public DefectList Defects { get; private set; }

        /// <summary>
        /// Формальная оценка качества.
        /// </summary>
        [JsonProperty("gold")]
        [XmlAttribute("gold")]
        public int Gold { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public RecordReport()
        {
            Defects = new DefectList();
        }

        #endregion

        #region IHandmadeSerializable

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Mfn = reader.ReadPackedInt32();
            Index = reader.ReadNullableString();
            Description = reader.ReadNullableString();
            Gold = reader.ReadPackedInt32();
            Defects.RestoreFromStream(reader);
        }

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32(Mfn)
                .WriteNullable(Index)
                .WriteNullable(Description)
                .WritePackedInt32(Gold);
            Defects.SaveToStream(writer);
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format
                (
                    "Mfn: {0}, Defects: {1}, Description: {2}",
                    Mfn,
                    Defects.Count,
                    Description
                );
        }

        #endregion
    }
}
