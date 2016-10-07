/* ControlUtility.cs -- useful extensions for Control
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Threading;
using System.Windows.Forms;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Useful extension for <see cref="Control"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ControlUtility
    {
        #region Properties

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Invoke specified <paramref name="action"/>
        /// strictly in UI thread for specified
        /// <paramref name="control"/>.
        /// </summary>
        public static void InvokeIfRequired
            (
                [CanBeNull] this Control control,
                [CanBeNull] MethodInvoker action
            )
        {
            if (ReferenceEquals(control, null)
                || ReferenceEquals(action, null))
            {
                return;
            }

            int counter = 0;

            // When the form, thus the control, isn't visible yet,
            // InvokeRequired returns false, resulting still
            // in a cross-thread exception.
            while (!control.Visible
                && counter < 20)
            {
                Thread.Sleep(50);
                counter++;
            }

            if (!control.Visible
                && counter >= 20)
            {
                return;
            }

            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        #endregion
    }
}
