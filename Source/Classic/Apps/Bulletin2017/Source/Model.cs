// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Model.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Json;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace Bulletin2017
{
    public class Model
        : IVerifiable
    {
        #region Properties

        [NotNull]
        [XmlElement("catalog")]
        [JsonProperty("catalogs")]
        public List<CatalogDescription> Catalogs { get; private set; }

        [NotNull]
        [XmlElement("report")]
        [JsonProperty("reports")]
        public List<ReportDescription> Reports { get; private set; }

        #endregion

        #region Construction

        public Model()
        {
            Catalogs = new  List<CatalogDescription>();
            Reports = new List<ReportDescription>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        [NotNull]
        public CatalogDescription GetDefaultCatalog()
        {
            CatalogDescription result = Catalogs
                .FirstOrDefault(catalog => catalog.Default)
                ?? Catalogs.First();

            return result;
        }

        public static Model LoadFromJson
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            Model result = JsonUtility.ReadObjectFromFile<Model>(fileName);

            return result;
        }

        #endregion

        #region IVerifiable members

        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<Model> verifier = new Verifier<Model>(this, throwOnError);

            verifier
                .Assert(Catalogs.Count != 0, "Catalogs.Count")
                .Assert(Reports.Count != 0, "Reports.Count");

            foreach (CatalogDescription catalog in Catalogs)
            {
                verifier
                    .VerifySubObject(catalog, "catalog");
            }
            foreach (ReportDescription report in Reports)
            {
                verifier
                    .VerifySubObject(report, "report");
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
