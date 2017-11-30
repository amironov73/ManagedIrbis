// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CatalogDescription.cs -- 
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
    [XmlRoot("catalog")]
    public sealed class CatalogDescription
    {
        #region Properties

        [XmlAttribute("default")]
        [JsonProperty("default", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsDefault { get; set; }

        [CanBeNull]
        [XmlElement("title")]
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [CanBeNull]
        [XmlElement("connectionString")]
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }

        [CanBeNull]
        [XmlElement("filter")]
        [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
        public string Filter { get; set; }

        [NotNull]
        [XmlElement("report")]
        [JsonProperty("reports")]
        public List<ReportDescription> Reports { get; set; }

        #endregion

        #region Construction

        public CatalogDescription()
        {
            Reports = new List<ReportDescription>();
        }

        #endregion

        #region Public methods

        //public ReportDescription GetDefaultReport()
        //{
        //    return (from report in Reports
        //               where report.Default
        //               select report).FirstOrDefault()
        //           ?? (from report in Reports
        //               select report).First();
        //}

        #endregion

        #region Object members

        public override string ToString()
        {
            return Title.ToVisibleString();
        }

        #endregion
    }
}
