/* PftTestResult.cs --
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

using CodeJam;

using JetBrains.Annotations;
using MoonSharp.Interpreter;
using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Testing
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftTestResult
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

        ///// <summary>
        ///// Record.
        ///// </summary>
        //[CanBeNull]
        //[JsonProperty("record")]
        //public MarcRecord Record { get; set; }

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
        /// Program AST dump.
        /// </summary>
        [CanBeNull]
        [JsonProperty("ast")]
        public string Ast { get; set; }

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
    }
}
