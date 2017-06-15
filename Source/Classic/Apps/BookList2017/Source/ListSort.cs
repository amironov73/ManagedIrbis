// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ListSort.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace BookList2017
{
    public sealed class ListSort
    {
        #region Properties

        [CanBeNull]
        [XmlElement("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        [CanBeNull]
        [XmlElement("field")]
        [JsonProperty("field")]
        public string Field { get; set; }

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
