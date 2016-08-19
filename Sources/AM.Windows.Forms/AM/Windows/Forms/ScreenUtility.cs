/* ScreenUtility.cs -- 
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class ScreenUtility
    {
        #region Properties

        /// <summary>
        /// Determines whether secondary screen present.
        /// </summary>
        /// <value><c>true</c> if secondary screen present;
        /// otherwise, <c>false</c>.</value>
        public static bool HaveSecondaryScreen
        {
            [DebuggerStepThrough]
            get
            {
                return SecondaryScreen != null;
            }
        }

        /// <summary>
        /// Gets the secondary screen.
        /// </summary>
        /// <value>The secondary screen or <c>null</c> if none.
        /// </value>
        public static Screen SecondaryScreen
        {
            [DebuggerStepThrough]
            get
            {
                foreach (Screen screen in Screen.AllScreens)
                {
                    if (!screen.Primary)
                    {
                        return screen;
                    }
                }

                return null;
            }
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Moves given form window to the primary screen.
        /// </summary>
        public static void MoveToPrimaryScreen
            (
                [NotNull] Form form
            )
        {
            Code.NotNull(form, "form");

            MoveToScreen(Screen.PrimaryScreen, form);
        }

        /// <summary>
        /// Moves form window to given screen.
        /// </summary>
        public static void MoveToScreen
            (
                [NotNull] Screen screen,
                [NotNull] Form form
            )
        {
            Code.NotNull(screen, "screen");
            Code.NotNull(form, "form");

            form.Location = screen.WorkingArea.Location;
        }

#if NOTDEF

        /// <summary>
        /// Moves the window to given screen.
        /// </summary>
        /// <param name="screen">The screen.</param>
        /// <param name="handle">Window handle.</param>
        public static void MoveToScreen
            (
                [NotNull] Screen screen,
                IntPtr handle
            )
        {
            Code.NotNull(screen, "screen");

            Win32Window window = new Win32Window(handle);
            window.Location = screen.WorkingArea.Location;
        }

#endif

        /// <summary>
        /// Moves given form window to secondary screen (if present).
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns><c>true</c> on success, <c>false</c>
        /// on failure (e.g. no secondary screen present).</returns>
        public static bool MoveToSecondaryScreen
            (
                [NotNull] Form form
            )
        {
            Code.NotNull(form, "form");

            Screen secondaryScreen = SecondaryScreen;
            if (secondaryScreen != null)
            {
                MoveToScreen(secondaryScreen, form);
                return true;
            }

            return false;
        }

#endregion
    }
}
