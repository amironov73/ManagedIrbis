// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportTestResult.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ReportTestResult
    {
        #region Properties

        /// <summary>
        /// Duration.
        /// </summary>
        [JsonProperty("duration")]
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Test failed?
        /// </summary>
        [JsonProperty("failed")]
        public bool Failed { get; set; }

        /// <summary>
        /// Finish time.
        /// </summary>
        [JsonProperty("finish")]
        public DateTime FinishTime { get; set; }

        /// <summary>
        /// Name of the test.
        /// </summary>
        [CanBeNull]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        [CanBeNull]
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Start time.
        /// </summary>
        [JsonProperty("start")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Input.
        /// </summary>
        [CanBeNull]
        [JsonProperty("input")]
        public string Input { get; set; }

        /// <summary>
        /// Tokens.
        /// </summary>
        [CanBeNull]
        [JsonProperty("tokens")]
        public string Tokens { get; set; }

        /// <summary>
        /// Output text.
        /// </summary>
        [CanBeNull]
        [JsonProperty("expected")]
        public string Expected { get; set; }

        /// <summary>
        /// Output text.
        /// </summary>
        [CanBeNull]
        [JsonProperty("output")]
        public string Output { get; set; }

        /// <summary>
        /// Exception text (if any).
        /// </summary>
        [CanBeNull]
        [JsonProperty("exception")]
        public string Exception { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
