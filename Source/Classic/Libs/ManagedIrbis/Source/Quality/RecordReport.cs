// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordReport.cs -- отчёт о проверке записи.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

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
    [XmlRoot("report")]
    [MoonSharpUserData]
    public sealed class RecordReport
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// MFN записи.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonProperty("mfn")]
        [DisplayName("MFN")]
        public int Mfn { get; set; }

        /// <summary>
        /// Шифр записи.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("index")]
        [JsonProperty("index")]
        [DisplayName("Шифр")]
        public string Index { get; set; }

        /// <summary>
        /// Краткое БО.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("description")]
        [JsonProperty("description")]
        [DisplayName("Описание")]
        public string Description { get; set; }

        /// <summary>
        /// Дефекты.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        [XmlElement("defect")]
        [JsonProperty("defects")]
        [DisplayName("Дефекты")]
        public DefectList Defects { get; internal set; }

        /// <summary>
        /// Формальная оценка качества.
        /// </summary>
        [XmlAttribute("quality")]
        [JsonProperty("quality")]
        [DisplayName("Качество")]
        public int Quality { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordReport()
        {
            Defects = new DefectList();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Should serialize <see cref="Defects"/> property?
        /// </summary>
        public bool ShouldSerializeDefects()
        {
            return Defects.Count != 0;
        }

        /// <summary>
        /// Should serialize <see cref="Description"/> property?
        /// </summary>
        public bool ShouldSerializeDescription()
        {
            return !string.IsNullOrEmpty(Description);
        }

        /// <summary>
        /// Should serialize <see cref="Index"/> property?
        /// </summary>
        public bool ShouldSerializeIndex()
        {
            return !string.IsNullOrEmpty(Index);
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Mfn = reader.ReadPackedInt32();
            Index = reader.ReadNullableString();
            Description = reader.ReadNullableString();
            Quality = reader.ReadPackedInt32();
            Defects.RestoreFromStream(reader);
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32(Mfn)
                .WriteNullable(Index)
                .WriteNullable(Description)
                .WritePackedInt32(Quality);
            Defects.SaveToStream(writer);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "MFN: {0}, Defects: {1}, Quality: {2}, Description: {3}",
                    Mfn,
                    Defects.Count,
                    Quality,
                    Description.ToVisibleString()
                );
        }

        #endregion
    }
}
