// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BiblioFilter.cs -- 
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

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class BiblioFilter
    {
        #region Properties

        /// <summary>
        /// Expression for record formatting.
        /// </summary>
        [CanBeNull]
        [JsonProperty("format")]
        public string FormatExpression { get; set; }

        /// <summary>
        /// Expression for record selection.
        /// </summary>
        [CanBeNull]
        [JsonProperty("select")]
        public string SelectExpression { get; set; }

        /// <summary>
        /// Expression for record sorting.
        /// </summary>
        [CanBeNull]
        [JsonProperty("sort")]
        public string SortExpression { get; set; }

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
