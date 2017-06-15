// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExcelColumn.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace AM.UI
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]

    public sealed class ExcelColumn
    {
        #region Properties

        /// <summary>
        /// Just for information.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Field to output.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("expression")]
        [JsonProperty("expression")]
        public string Expression { get; set; }

        /// <summary>
        /// Draw border?
        /// </summary>
        [XmlAttribute("border")]
        [JsonProperty("border")]
        public bool Border { get; set; }

        /// <summary>
        /// Wrap text?
        /// </summary>
        [XmlAttribute("wrap")]
        [JsonProperty("wrap")]
        public bool Wrap { get; set; }

        /// <summary>
        /// Column height.
        /// </summary>
        [XmlAttribute("height")]
        [JsonProperty("height")]
        public double Height { get; set; }

        /// <summary>
        /// Column width.
        /// </summary>
        [XmlAttribute("width")]
        [JsonProperty("width")]
        public double Width { get; set; }

        #endregion
    }
}
