// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisBusyStripe.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class IrbisBusyStripe
        : BusyStripe
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Subscribe to the connection busy state.
        /// </summary>
        public void SubscribeTo
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");
        }

        /// <summary>
        /// Unsubscribe from the connection
        /// busy state.
        /// </summary>
        public void UnsubscribeFrom
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");
        }

        #endregion
    }
}
