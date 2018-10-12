// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ViafResponse.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using Newtonsoft.Json;

#endregion

namespace RestfulIrbis.Viaf
{
    /// <summary>
    /// VIAF response
    /// </summary>
    public class ViafSuggestResponse
    {
        /// <summary>
        /// Query.
        /// </summary>
        [JsonProperty("query")]
        public string Query { get; set; }

        /// <summary>
        /// Results.
        /// </summary>
        [JsonProperty("result")]
        public ViafSuggestResult[] SuggestResults { get; set; }
    }
}