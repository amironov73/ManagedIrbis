// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DataColumnInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
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
    [XmlRoot("column")]
    [MoonSharpUserData]
    public sealed class DataColumnInfo
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        [CanBeNull]
        [DefaultValue(null)]
        [JsonProperty("name")]
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the column title.
        /// </summary>
        [CanBeNull]
        [DefaultValue(null)]
        [JsonProperty("title")]
        [XmlAttribute("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the width of the column.
        /// </summary>
        /// <value>The width of the column.</value>
        [DefaultValue(0)]
        [JsonProperty("width")]
        [XmlAttribute("width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the data grid column.
        /// </summary>
        /// <value>The data grid column.</value>
        [DefaultValue(null)]
        [JsonProperty("type")]
        [XmlAttribute("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("defaultValue")]
        [XmlAttribute("defaultValue")]
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this 
        /// the column is frozen.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("frozen")]
        [XmlAttribute("frozen")]
        public bool Frozen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this column is invisible.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("invisible")]
        [XmlAttribute("invisible")]
        public bool Invisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 
        /// the column is read only.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("readOnly")]
        [XmlAttribute("readOnly")]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether
        /// this column is sorted.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("sorted")]
        [XmlAttribute("sorted")]
        public bool Sorted { get; set; }

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return Name.ToVisibleString();
        }

        #endregion
    }
}
