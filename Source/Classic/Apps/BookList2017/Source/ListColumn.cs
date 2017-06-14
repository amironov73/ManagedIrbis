// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ListColumn.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using CodeJam;

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace BookList2017
{
    public sealed class ListColumn
    {
        #region Properties

        [CanBeNull]
        [XmlAttribute("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [CanBeNull]
        [XmlAttribute("expression")]
        [JsonProperty("expression")]
        public string Expression { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
