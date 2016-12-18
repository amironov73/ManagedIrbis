// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SiberianNavigationEventArgs.cs -- 
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI.Grid
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SiberianNavigationEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Cancellation flag.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// New column number.
        /// </summary>
        public int NewColumn { get; set; }

        /// <summary>
        /// New row number.
        /// </summary>
        public int NewRow { get; set; }

        /// <summary>
        /// Old column number.
        /// </summary>
        public int OldColumn { get; set; }

        /// <summary>
        /// Old row number.
        /// </summary>
        public int OldRow { get; set; }

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
