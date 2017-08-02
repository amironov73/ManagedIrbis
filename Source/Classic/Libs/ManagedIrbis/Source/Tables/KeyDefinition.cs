// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* KeyDefinition.cs -- 
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
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Tables
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class KeyDefinition
    {
        #region Properties

        /// <summary>
        /// Length of the key.
        /// </summary>
        [XmlAttribute("length")]
        [JsonProperty("length")]
        public int Length { get; set; }

        /// <summary>
        /// Multiple values allowed?
        /// </summary>
        [XmlAttribute("multiple")]
        [JsonProperty("multiple")]
        public bool Multiple { get; set; }

        /// <summary>
        /// Format.
        /// </summary>
        [CanBeNull]
        [XmlElement("format")]
        [JsonProperty("format")]
        public string Format { get; set; }

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
