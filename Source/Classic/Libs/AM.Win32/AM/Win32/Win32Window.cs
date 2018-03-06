// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Win32Window.cs -- better than NativeWindow.
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Better than <see cref="System.Windows.Forms.NativeWindow"/>
    /// class.
    /// </summary>
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

        private IntPtr _handle;

        /// <summary>
        /// Gets or sets window handle.
        /// </summary>
        /// <value>Window handle.</value>
        public IntPtr Handle
        {
            [DebuggerStepThrough]
            get
            {
                return _handle;
            }
            [DebuggerStepThrough]
            set
            {
                _handle = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; 
        /// otherwise, <c>false</c>.</value>
        public bool IsValid
        {
            get
            {
                return User32.IsWindow(Handle);
            }
        }

        /// <summary>
        /// Gets or sets window location.
        /// </summary>
        /// <value>The location.</value>
        public Point Location
        {
            get
            {
                return this.Bounds.Location;
            }
            set
            {
                Size size = this.Size;
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
        /// <value>Window size.</value>
        public Size Size
        {
            get
            {
                return this.Bounds.Size;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets or sets window text (caption).
        /// </summary>
        /// <value>Window text (caption).</value>
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
        /// <value><c>true</c> if [top most]; otherwise, <c>false</c>.
        /// </value>
        public bool TopMost
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this 
        /// <see cref="T:Win32Window"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.
        /// </value>
        public bool Visible
        {
            get
            {
                return User32.IsWindowVisible(Handle);
            }
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

        #region Private members
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
        /// <returns></returns>
        public IntPtr GetDC()
        {
            return User32.GetDC(Handle);
        }

        /// <summary>
        /// Gets the desktop window.
        /// </summary>
        /// <returns></returns>
        public static Win32Window GetDesktopWindow()
        {
            return new Win32Window(User32.GetDesktopWindow());
        }

        /// <summary>
        /// Gets the foreground window.
        /// </summary>
        /// <returns></returns>
        public static Win32Window GetForegroundWindow()
        {
            return new Win32Window(User32.GetForegroundWindow());
        }

        /// <summary>
        /// Gets the next window.
        /// </summary>
        /// <returns></returns>
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
        /// <returns></returns>
        public Win32Window GetParentWindow()
        {
            return new Win32Window(User32.GetParent(Handle));
        }

        /// <summary>
        /// Gets the shell window.
        /// </summary>
        /// <returns></returns>
        public static Win32Window GetShellWindow()
        {
            return new Win32Window(User32.GetShellWindow());
        }

        /// <summary>
        /// Gets the window DC.
        /// </summary>
        /// <returns></returns>
        public IntPtr GetWindowDC()
        {
            return User32.GetWindowDC(Handle);
        }

        /// <summary>
        /// Invalidates whole window region.
        /// </summary>
        /// <param name="erase">If set to <c>true</c> 
        /// background of the window will be erased.</param>
        public void Invalidate(bool erase)
        {
            User32.InvalidateRect(Handle, IntPtr.Zero, erase);
        }

        /// <summary>
        /// Invalidates specified rectangle region of the window.
        /// </summary>
        /// <param name="rectangle">Region to invalidate.</param>
        /// <param name="erase">If set to <c>true</c> 
        /// background of the window will be erased.</param>
        public void Invalidate(Rectangle rectangle, bool erase)
        {
            User32.InvalidateRect(Handle, ref rectangle, erase);
        }

        /// <summary>
        /// Releases the DC.
        /// </summary>
        /// <param name="context">The context.</param>
        public void ReleaseDC(IntPtr context)
        {
            User32.ReleaseDC(Handle, context);
        }

        #endregion
    }
}
