/* IndexRange.cs --
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Index range
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public struct IndexRange
    {
        #region Properties

        /// <summary>
        /// First index.
        /// </summary>
        public IndexSpecification First { get; set; }

        /// <summary>
        /// Last index.
        /// </summary>
        public IndexSpecification Last { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
