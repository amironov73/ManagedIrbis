/* IndexSpecification.cs --
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
    /// Index specification (for fields).
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public struct IndexSpecification
    {
        #region Properties

        /// <summary>
        /// Index kind.
        /// </summary>
        public IndexKind Kind { get; set; }

        /// <summary>
        /// Index itself.
        /// </summary>
        public int Index { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
