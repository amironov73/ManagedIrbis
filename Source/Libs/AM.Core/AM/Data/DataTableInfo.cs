/* DataTableInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace AM.Data
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [XmlRoot("table")]
    [MoonSharpUserData]
    public class DataTableInfo
    {
        #region Properties

        private List<DataColumnInfo> _columns
            = new List<DataColumnInfo>();

        /// <summary>
        /// Gets the columns.
        /// </summary>
        [NotNull]
        [JsonProperty("columns")]
        [XmlElement("column")]
        public List<DataColumnInfo> Columns
        {
            [DebuggerStepThrough]
            get
            {
                return _columns;
            }
        }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        [CanBeNull]
        [JsonProperty("name")]
        [XmlAttribute("name")]
        public string TableName { get; set; }

        #endregion
    }
}
