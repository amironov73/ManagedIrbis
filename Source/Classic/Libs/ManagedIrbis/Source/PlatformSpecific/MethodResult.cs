/* MethodResult.cs --
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

#if CLASSIC || DESKTOP

using System.Diagnostics;

#endif

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.PlatformSpecific
{
    /// <summary>
    /// Result of external method call.
    /// </summary>
    sealed class MethodResult
    {
        #region Properties

        /// <summary>
        /// Return code.
        /// </summary>
        public int ReturnCode { get; set; }

        /// <summary>
        /// Input.
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// Output.
        /// </summary>
        public string Output { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
