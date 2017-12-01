// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CatalogDescription.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using AM;

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace Bulletin2017
{
    [XmlRoot("catalog")]
    public sealed class CatalogDescription
        : IVerifiable
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
        public List<ReportReference> Reports { get; set; }

        #endregion

        #region Construction

        public CatalogDescription()
        {
            Reports = new List<ReportReference>();
        }

        #endregion

        #region Public methods

        [NotNull]
        public ReportReference GetDefaultReport()
        {
            return Reports
                       .FirstOrDefault(report => report.Default)
                   ?? Reports.First();
        }

        #endregion

        #region IVerifiable members

        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<CatalogDescription> verifier
                = new Verifier<CatalogDescription>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Title, "Title")
                .NotNullNorEmpty(ConnectionString, "ConnectionString");
            foreach (ReportReference report in Reports)
            {
                verifier
                    .VerifySubObject(report, "report");
            }

            return verifier.Result;
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
