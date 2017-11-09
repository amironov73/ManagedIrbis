// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TableDefinition.cs -- parameters for table command
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

// ReSharper disable ConvertToAutoProperty

namespace ManagedIrbis
{
    /// <summary>
    /// Signature for Table command.
    /// </summary>
    [PublicAPI]
    [XmlRoot("table")]
    [MoonSharpUserData]
    public sealed class TableDefinition
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("database")]
        [JsonProperty("database", NullValueHandling = NullValueHandling.Ignore)]
        public string DatabaseName { get; set; }

        /// <summary>
        /// Table name.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("table")]
        [JsonProperty("table", NullValueHandling = NullValueHandling.Ignore)]
        public string Table { get; set; }

        /// <summary>
        /// Table headers.
        /// </summary>
        [NotNull]
        [XmlElement("header")]
        [JsonProperty("headers")]
        public List<string> Headers
        {
            get { return _headers; }
        }

        /// <summary>
        /// Mode.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("mode")]
        [JsonProperty("mode", NullValueHandling = NullValueHandling.Ignore)]
        public string Mode { get; set; }

        /// <summary>
        /// Search query.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("search")]
        [JsonProperty("search", NullValueHandling = NullValueHandling.Ignore)]
        public string SearchQuery { get; set; }

        /// <summary>
        /// Minimal MFN.
        /// </summary>
        [XmlAttribute("minMfn")]
        [JsonProperty("minMfn", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int MinMfn { get; set; }

        /// <summary>
        /// Maximal MFN.
        /// </summary>
        [XmlAttribute("maxMfn")]
        [JsonProperty("maxMfn", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int MaxMfn { get; set; }

        /// <summary>
        /// Optional sequential query.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("sequential")]
        [JsonProperty("sequential", NullValueHandling = NullValueHandling.Ignore)]
        public string SequentialQuery { get; set; }

        /// <summary>
        /// List of MFN.
        /// </summary>
        [NotNull]
        [XmlElement("mfn")]
        [JsonProperty("mfn")]
        public List<int> MfnList
        {
            get { return _mfnList; }
        }

        #endregion

        #region Private members

        private readonly List<string> _headers = new List<string>();

        private readonly List<int> _mfnList = new List<int>();

        #endregion

        #region Public methods

        /// <summary>
        /// Should serialize the <see cref="Headers"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeHeaders()
        {
            return Headers.Count != 0;
        }

        /// <summary>
        /// Should serialize the <see cref="MaxMfn"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMaxMfn()
        {
            return MaxMfn != 0;
        }

        /// <summary>
        /// Should serialize the <see cref="MinMfn"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMinMfn()
        {
            return MinMfn != 0;
        }

        /// <summary>
        /// Should serialize the <see cref="MfnList"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMfnList()
        {
            return MfnList.Count != 0;
        }

        #endregion
    }
}
