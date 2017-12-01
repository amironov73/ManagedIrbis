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
        : IVerifiable
    {
        #region Properties

        [CanBeNull]
        [XmlAttribute("id")]
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [CanBeNull]
        [XmlAttribute("file")]
        [JsonProperty("file", NullValueHandling = NullValueHandling.Ignore)]
        public string ReportFile { get; set; }

        [NotNull]
        [XmlElement("group")]
        [JsonProperty("groups")]
        public List<GroupDescription> Groups { get; set; }

        [CanBeNull]
        [XmlElement("format")]
        [JsonProperty("format", NullValueHandling = NullValueHandling.Ignore)]
        public string Brief { get; set; }

        #endregion

        #region Construction

        public ReportDescription()
        {
            Groups = new List<GroupDescription>();
        }

        #endregion

        #region IVerifiable members

        public bool Verify(bool throwOnError)
        {
            Verifier<ReportDescription> verifier
                = new Verifier<ReportDescription>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Id, "Id")
                .NotNullNorEmpty(ReportFile, "ReportFile");
            foreach (GroupDescription theGroup in Groups)
            {
                verifier
                    .VerifySubObject(theGroup, "group");
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return Id.ToVisibleString();
        }

        #endregion
    }
}
