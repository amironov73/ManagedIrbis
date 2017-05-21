// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DataTableInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using AM.Collections;

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

        /// <summary>
        /// Gets the columns.
        /// </summary>
        [NotNull]
        [JsonProperty("columns")]
        [XmlElement("column")]
        public NonNullCollection<DataColumnInfo> Columns
            {
                get;
                private set;
            }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        [CanBeNull]
        [JsonProperty("name")]
        [XmlAttribute("name")]
        public string Name { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DataTableInfo()
        {
            Columns = new NonNullCollection<DataColumnInfo>();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Name.ToVisibleString();
        }

        #endregion
    }
}
