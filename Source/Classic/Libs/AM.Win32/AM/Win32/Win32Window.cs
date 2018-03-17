// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Win32Window.cs -- better than NativeWindow.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Drawing;
using System.Text;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Better than System.Windows.Forms.NativeWindow class.
    /// </summary>
    [PublicAPI]
    public class Win32Window
    {
        #region Properties

        /// <summary>
        /// Gets or sets window bounds.
        /// </summary>
        /// <value>Window bounds.</value>
        public Rectangle Bounds
        {
            get
            {
                Rectangle result;
                User32.GetWindowRect(Handle, out result);

                return result;
            }
            set
            {
                User32.MoveWindow
                    (
                        Handle,
                        value.Left,
                        value.Top,
                        value.Width,
                        value.Height,
                        true
                    );
            }
        }

        /// <summary>
        /// Gets or sets window handle.
        /// </summary>
        public IntPtr Handle { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        public bool IsValid
        {
            get { return User32.IsWindow(Handle); }
        }

        /// <summary>
        /// Gets or sets window location.
        /// </summary>
        public Point Location
        {
            get { return Bounds.Location; }
            set
            {
                Size size = Size;
                User32.MoveWindow
                    (
                        Handle,
                        value.X,
                        value.Y,
                        size.Width,
                        size.Height,
                        true
                    );
            }
        }

        /// <summary>
        /// Gets or sets window size.
        /// </summary>
        public Size Size
        {
            get { return Bounds.Size; }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets window text (caption).
        /// </summary>
        public string Text
        {
            get
            {
                StringBuilder result = new StringBuilder(256);
                User32.GetWindowText(Handle, result, result.Capacity);

                return result.ToString();
            }
            set
            {
                User32.SetWindowText(Handle, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [top most].
        /// </summary>
        public bool TopMost
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:Win32Window"/> is visible.
        /// </summary>
        public bool Visible
        {
            get { return User32.IsWindowVisible(Handle); }
            set
            {
                User32.ShowWindow
                    (
                        Handle,
                        value
                            ? ShowWindowFlags.SW_SHOW
                            : ShowWindowFlags.SW_HIDE
                    );
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Win32Window(IntPtr handle)
        {
            Handle = handle;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Closes the window.
        /// </summary>
        public void Close()
        {
            User32.DestroyWindow(Handle);
        }

        /// <summary>
        /// Gets the DC.
        /// </summary>
        public IntPtr GetDC()
        {
            return User32.GetDC(Handle);
        }

        /// <summary>
        /// Gets the desktop window.
        /// </summary>
        public static Win32Window GetDesktopWindow()
        {
            return new Win32Window(User32.GetDesktopWindow());
        }

        /// <summary>
        /// Gets the foreground window.
        /// </summary>
        public static Win32Window GetForegroundWindow()
        {
            return new Win32Window(User32.GetForegroundWindow());
        }

        /// <summary>
        /// Gets the next window.
        /// </summary>
        public Win32Window GetNextWindow()
        {
            return new Win32Window(User32.GetWindow
                (
                    Handle,
                    GetWindowFlags.GW_HWNDNEXT
                ));
        }

        /// <summary>
        /// Gets parent of the window.
        /// </summary>
        public Win32Window GetParentWindow()
        {
            return new Win32Window(User32.GetParent(Handle));
        }

        /// <summary>
        /// Gets the shell window.
        /// </summary>
        public static Win32Window GetShellWindow()
        {
            return new Win32Window(User32.GetShellWindow());
        }

        /// <summary>
        /// Gets the window DC.
        /// </summary>
        public IntPtr GetWindowDC()
        {
            return User32.GetWindowDC(Handle);
        }

        /// <summary>
        /// Invalidates whole window region.
        /// </summary>
        public void Invalidate
            (
                bool erase
            )
        {
            User32.InvalidateRect(Handle, IntPtr.Zero, erase);
        }

        /// <summary>
        /// Invalidates specified rectangle region of the window.
        /// </summary>
        public void Invalidate
            (
                Rectangle rectangle,
                bool erase
            )
        {
            User32.InvalidateRect(Handle, ref rectangle, erase);
        }

        /// <summary>
        /// Releases the DC.
        /// </summary>
        public void ReleaseDC
            (
                IntPtr context
            )
        {
            User32.ReleaseDC(Handle, context);
        }

        #endregion
    }
}
