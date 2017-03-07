// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchParameters.cs -- parameters for SearchCommand
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
    /// Signature for <see cref="SearchCommand"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("search")]
    public sealed class SearchParameters
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
        /// First record offset.
        /// </summary>
        [XmlAttribute("first")]
        [JsonProperty("first")]
        public int FirstRecord { get; set; }

        /// <summary>
        /// Format specification.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("format")]
        [JsonProperty("format")]
        public string FormatSpecification { get; set; }

        /// <summary>
        /// Maximal MFN.
        /// </summary>
        [XmlAttribute("max")]
        [JsonProperty("max")]
        public int MaxMfn { get; set; }

        /// <summary>
        /// Minimal MFN.
        /// </summary>
        [XmlAttribute("min")]
        [JsonProperty("min")]
        public int MinMfn { get; set; }

        /// <summary>
        /// Number of records.
        /// </summary>
        [XmlAttribute("records")]
        [JsonProperty("records")]
        public int NumberOfRecords { get; set; }

        /// <summary>
        /// Search query expression.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("expression")]
        [JsonProperty("expression")]
        public string SearchExpression { get; set; }

        /// <summary>
        /// Specification of sequential search.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("sequential")]
        [JsonProperty("sequential")]
        public string SequentialSpecification { get; set; }

        /// <summary>
        /// Specification for local filter.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("filter")]
        [JsonProperty("filter")]
        public string FilterSpecification { get; set; }

        /// <summary>
        /// Use UTF8 encoding for
        /// <see cref="FormatSpecification"/>?
        /// </summary>
        public bool UtfFormat { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchParameters()
        {
            FirstRecord = 1;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the parameters.
        /// </summary>
        [NotNull]
        public SearchParameters Clone()
        {
            return (SearchParameters) MemberwiseClone();
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
            FirstRecord = reader.ReadPackedInt32();
            FormatSpecification = reader.ReadNullableString();
            MaxMfn = reader.ReadPackedInt32();
            MinMfn = reader.ReadPackedInt32();
            NumberOfRecords = reader.ReadPackedInt32();
            SearchExpression = reader.ReadNullableString();
            SequentialSpecification = reader.ReadNullableString();
            FilterSpecification = reader.ReadNullableString();
            UtfFormat = reader.ReadBoolean();
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
                .WritePackedInt32(FirstRecord)
                .WriteNullable(FormatSpecification)
                .WritePackedInt32(MaxMfn)
                .WritePackedInt32(MinMfn)
                .WritePackedInt32(NumberOfRecords)
                .WriteNullable(SearchExpression)
                .WriteNullable(SequentialSpecification)
                .WriteNullable(FilterSpecification)
                .Write(UtfFormat);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<SearchParameters> verifier
                = new Verifier<SearchParameters>(this, throwOnError);

            //if (!string.IsNullOrEmpty(SequentialSpecification))
            //{
            //    verifier
            //        .NotNullNorEmpty(SearchExpression, "SearchExpression");
            //}

            return verifier.Result;
        }

        #endregion
    }
}
