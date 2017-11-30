// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BulletinConfiguration.cs -- 
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

namespace Bulletin2017.Source
{
    [XmlRoot("configuration")]
    public sealed class BulletinConfiguration
    {
        #region Properties

        [NotNull]
        [XmlElement("catalog")]
        [JsonProperty("catalogs")]
        public List<CatalogDescription> Catalogs { get; set; }

        #endregion

        #region Construction

        public BulletinConfiguration()
        {
            Catalogs = new List<CatalogDescription>();
        }

        #endregion

        #region Public methods

        [NotNull]
        public static BulletinConfiguration Load
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            BulletinConfiguration result = JsonUtility
                .ReadObjectFromFile<BulletinConfiguration>(fileName);

            return result;
        }

        #endregion
    }
}
