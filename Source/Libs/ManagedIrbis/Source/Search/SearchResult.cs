/* SearchResult.cs --
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
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure.Commands;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Search
{
    /// <summary>
    /// Search result.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SearchResult
    {
        #region Properties

        /// <summary>
        /// Count of records found.
        /// </summary>
        public int FoundCount { get; set; }

        /// <summary>
        /// Search query text.
        /// </summary>
        [CanBeNull]
        public string Query { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
