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
        [JsonProperty]
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
        /// Record.
        /// </summary>
        [CanBeNull]
        [JsonProperty("record")]
        public MarcRecord Record { get; set; }

        /// <summary>
        /// Input.
        /// </summary>
        [CanBeNull]
        public string Input { get; set; }

        /// <summary>
        /// Output text.
        /// </summary>
        [CanBeNull]
        [JsonProperty("output")]
        public string Output { get; set; }


        #endregion
    }
}
