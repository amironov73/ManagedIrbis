/* SearchParameters.cs -- parameters for SearchCommand
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AM;
using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure.Commands;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Search
{
    /// <summary>
    /// Parameters for <see cref="SearchCommand"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("search")]
    public sealed class SearchParameters
        : IVerifiable
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

        #region IVerifiable members

        /// <summary>
        /// Verify object state.
        /// </summary>
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
