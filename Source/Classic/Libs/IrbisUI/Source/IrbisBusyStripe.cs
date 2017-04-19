// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisBusyStripe.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM.Threading;
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

        private void Busy_StateChanged
            (
                object sender,
                EventArgs e
            )
        {
            BusyState state = (BusyState)sender;

            this.InvokeIfRequired
                (
                    () =>
                    {
                        Moving = state;
                        Invalidate();
                    }
                );
        }

        private void Connection_Disposing
            (
                object sender,
                EventArgs e
            )
        {
            IrbisConnection connection = (IrbisConnection)sender;

            UnsubscribeFrom(connection);
        }

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

            connection.Busy.StateChanged += Busy_StateChanged;
            connection.Disposing += Connection_Disposing;
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

            connection.Busy.StateChanged -= Busy_StateChanged;
            connection.Disposing -= Connection_Disposing;
        }

        #endregion
    }
}
