// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PostingsParameters.cs -- parameters for ReadPostingsCommand
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
    /// Signature for <see cref="ReadPostingsCommand"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("postings")]
    public sealed class PostingParameters
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
        /// First posting to return.
        /// </summary>
        [XmlAttribute("first")]
        [JsonProperty("first")]
        public int FirstPosting { get; set; }

        /// <summary>
        /// Format.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("format")]
        [JsonProperty("format")]
        public string Format { get; set; }

        /// <summary>
        /// Number of postings to return.
        /// </summary>
        [XmlAttribute("number")]
        [JsonProperty("number")]
        public int NumberOfPostings { get; set; }

        /// <summary>
        /// Term.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("term")]
        [JsonProperty("term")]
        public string Term { get; set; }

        /// <summary>
        /// List of terms.
        /// </summary>
        [CanBeNull]
        [XmlArray("terms")]
        [XmlArrayItem("term")]
        [JsonProperty("terms")]
        public string[] ListOfTerms { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PostingParameters()
        {
            FirstPosting = 1;
        }

        #endregion
        
        #region Public methods

        /// <summary>
        /// Clone the parameters.
        /// </summary>
        [NotNull]
        public PostingParameters Clone()
        {
            return (PostingParameters) MemberwiseClone();
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
            Verifier<PostingParameters> verifier
                = new Verifier<PostingParameters>
                    (
                        this,
                        throwOnError
                    );

            verifier                
                .Assert(FirstPosting >= 0, "FirstPosting")
                .Assert(NumberOfPostings >= 0, "NumberOfPostings");

            verifier
                .Assert
                (
                    Term != null || ListOfTerms != null,
                    "Term or ListOfTerms"
                );

            return verifier.Result;
        }

        #endregion
    }
}
