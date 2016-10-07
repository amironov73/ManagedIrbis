/* Win32FormUtility.cs -- Win32-specific extension methods for System.Windows.Forms
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM.Win32;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Win32-specific extension methods for <see cref="Form"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class Win32FormUtility
    {
        #region Public methods

        /// <summary>
        /// Shows the form without activation.
        /// </summary>
        public static void ShowNoActivate
            (
                [NotNull] Form form
            )
        {
            Code.NotNull(form, "form");

            User32.SetWindowPos
                (
                    form.Handle,
                    User32.HWND_TOPMOST,
                    0,
                    0,
                    0,
                    0,
                    SetWindowPosFlags.SWP_SHOWWINDOW
                    | SetWindowPosFlags.SWP_NOACTIVATE
                    | SetWindowPosFlags.SWP_NOSIZE
                    | SetWindowPosFlags.SWP_NOMOVE
                  );
            form.Show();
        }

        #endregion
    }
}
