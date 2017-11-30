// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GroupDescription.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using AM;

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace Bulletin2017
{
    [XmlRoot("group")]
    public sealed class GroupDescription
    {
        #region Properties

        [CanBeNull]
        [XmlAttribute("title")]
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [CanBeNull]
        [XmlAttribute("filter")]
        [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
        public string Filter { get; set; }

        #endregion

        #region Object members

        public override string ToString()
        {
            return Title.ToVisibleString();
        }

        #endregion
    }
}
