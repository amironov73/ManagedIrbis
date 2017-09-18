// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RuleReport.cs -- отчёт о работе правила.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.ComponentModel;
using System.Xml.Serialization;

using AM;
using AM.Collections;
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
    /// Отчёт о работе правила.
    /// </summary>
    [PublicAPI]
    [XmlRoot("report")]
    [MoonSharpUserData]
    public sealed class RuleReport
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Дефекты, обнаруженные правилом.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        [XmlElement("defect")]
        [JsonProperty("defects")]
        [DisplayName("Дефекты")]
        public NonNullCollection<FieldDefect> Defects { get; private set; }

        /// <summary>
        /// Общий урон.
        /// </summary>
        [XmlAttribute("damage")]
        [JsonProperty("damage")]
        [DisplayName("Штраф")]
        public int Damage { get; set; }

        /// <summary>
        /// Начисленный бонус.
        /// </summary>
        [XmlAttribute("bonus")]
        [JsonProperty("bonus")]
        [DisplayName("Бонус")]
        public int Bonus { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public RuleReport()
        {
            Defects = new NonNullCollection<FieldDefect>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Should serialize the <see cref="Bonus"/> property?
        /// </summary>
        public bool ShouldSerializeBonus()
        {
            return Bonus != 0;
        }

        /// <summary>
        /// Should serialize the <see cref="Damage"/> property?
        /// </summary>
        public bool ShouldSerializeDamage()
        {
            return Damage != 0;
        }

        /// <summary>
        /// Should serialize the <see cref="Defects"/> property?
        /// </summary>
        public bool ShouldSerializeDefects()
        {
            return Defects.Count != 0;
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

            Defects = reader.ReadNonNullCollection<FieldDefect>();
            Damage = reader.ReadPackedInt32();
            Bonus = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteCollection(Defects)
                .WritePackedInt32(Damage)
                .WritePackedInt32(Bonus);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<RuleReport> verifier
                = new Verifier<RuleReport>(this, throwOnError);

            verifier
                .Assert(Damage >= 0, "Damage >= 0")
                .Assert(Bonus >= 0, "Bonus >= 0");

            foreach (FieldDefect defect in Defects)
            {
                verifier.VerifySubObject(defect, "defect");
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Damage={0}, Bonus={1}, Defects={2}",
                    Damage,
                    Bonus,
                    Defects.Count
                );
        }

        #endregion
    }
}
