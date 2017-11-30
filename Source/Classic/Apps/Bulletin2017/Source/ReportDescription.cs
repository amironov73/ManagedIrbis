// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportDescription.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Xml.Serialization;

using AM;

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace Bulletin2017
{
    [XmlRoot("report")]
    public sealed class ReportDescription
    {
        #region Properties

        [XmlAttribute("default")]
        [JsonProperty("default", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Default { get; set; }

        [CanBeNull]
        [XmlElement("title")]
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [CanBeNull]
        [XmlAttribute("class")]
        [JsonProperty("class")]
        public string ReportClass { get; set; }

        [NotNull]
        [XmlElement("group")]
        [JsonProperty("groups")]
        public List<GroupDescription> Groups { get; set; }

        [NotNull]
        [XmlElement("mailto")]
        [JsonProperty("mailto")]
        public List<string> MailTo { get; set; }

        [CanBeNull]
        [XmlElement("template")]
        [JsonProperty("template", NullValueHandling = NullValueHandling.Ignore)]
        public string MailTemplate { get; set; }

        [CanBeNull]
        [XmlElement("brief")]
        [JsonProperty("brief", NullValueHandling = NullValueHandling.Ignore)]
        public string Brief { get; set; }

        [XmlElement("dontSort")]
        [JsonProperty("dontSort", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool DontSort { get; set; }

        #endregion

        #region Construction

        public ReportDescription()
        {
            Groups = new List<GroupDescription>();
            MailTo = new List<string>();
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return Title.ToVisibleString();
        }

        #endregion
    }
}
