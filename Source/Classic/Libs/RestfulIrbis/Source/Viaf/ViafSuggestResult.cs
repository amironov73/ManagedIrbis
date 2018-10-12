// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ViafResult.cs --
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
    /// Single resulf from VIAF.
    /// </summary>
    public sealed class ViafSuggestResult
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("term")]
        public string Term { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("displayForm")]
        public string DisplayForm { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("nametype")]
        public string NameType { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("lc")]
        public string Lc { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("dnb")]
        public string Dnb { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("selibr")]
        public string Selibr { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("bav")]
        public string Bav { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("bnf")]
        public string Bnf { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("iccu")]
        public string Iccu { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("bne")]
        public string Bne { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("nkc")]
        public string Nkc { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("ptbnp")]
        public string Ptbnp { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("swnl")]
        public string Swnl { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("viafid")]
        public string ViafId { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("score")]
        public string Score { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("recordID")]
        public string RecordId { get; set; }

        #endregion
    }
}