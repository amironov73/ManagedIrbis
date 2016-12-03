// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TermParameters.cs -- parameters for ReadTermsCommand
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

using ManagedIrbis.Infrastructure.Commands;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Search
{
    /// <summary>
    /// Signature for <see cref="ReadTermsCommand"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("term")]
    public sealed class TermParameters
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("database")]
        [JsonProperty("database")]
        public string Database { get; set; }

        /// <summary>
        /// Number of terms to return.
        /// </summary>
        [XmlAttribute("number")]
        [JsonProperty("number")]
        public int NumberOfTerms { get; set; }

        /// <summary>
        /// Reverse order?
        /// </summary>
        [XmlAttribute("reverse")]
        [JsonProperty("reverse")]
        public bool ReverseOrder { get; set; }

        /// <summary>
        /// Start term.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("start")]
        [JsonProperty("start")]
        public string StartTerm { get; set; }

        /// <summary>
        /// Format.
        /// </summary>
        [CanBeNull]
        [XmlElement("format")]
        [JsonProperty("format")]
        public string Format { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the <see cref="TermParameters"/>.
        /// </summary>
        [NotNull]
        public TermParameters Clone()
        {
            return (TermParameters) MemberwiseClone();
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Database = reader.ReadNullableString();
            NumberOfTerms = reader.ReadPackedInt32();
            StartTerm = reader.ReadNullableString();
            Format = reader.ReadNullableString();
            ReverseOrder = reader.ReadBoolean();
        }

        /// <inheritdoc />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Database)
                .WritePackedInt32(NumberOfTerms)
                .WriteNullable(StartTerm)
                .WriteNullable(Format)
                .Write(ReverseOrder);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<TermParameters> verifier
                = new Verifier<TermParameters>
                (
                    this,
                    throwOnError
                );

            return verifier.Result;
        }

        #endregion
    }
}
