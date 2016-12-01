// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CancelableEventArgs.cs -- Event arguments for cancelable handling
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// <see cref="T:System.EventArgs"/> for cancelable
    /// handling.
    /// </summary>
    [PublicAPI]
    public class CancelableEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether
        /// event handling must be canceled.
        /// </summary>
        /// <value><c>true</c> if cancel; 
        /// otherwise, <c>false</c>.</value>
        public bool Cancel { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Handles the event with specified sender and handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="handler">The handler.</param>
        /// <returns><c>true</c> if handling was canceled,
        /// <c>false</c> otherwise.</returns>
        public bool Handle
            (
                object sender,
                CancelableEventHandler handler
            )
        {
            if (handler == null)
            {
                return false;
            }

            Delegate[] list = handler.GetInvocationList();
            foreach (CancelableEventHandler eventHandler in list)
            {
                eventHandler(sender, this);
                if (Cancel)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
