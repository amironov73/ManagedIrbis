// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ListVariant.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using AM.UI;

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace BookList2017
{
    public sealed class ListVariant
    {
        #region Properties

        [CanBeNull]
        [XmlElement("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [CanBeNull]
        [XmlElement("fileName")]
        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [XmlElement("firstLine")]
        [JsonProperty("firstLine")]
        public int FirstLine { get; set; }

        [CanBeNull]
        [XmlElement("column")]
        [JsonProperty("columns")]
        public ExcelColumn[] Columns { get; set; }

        #endregion
    }
}
