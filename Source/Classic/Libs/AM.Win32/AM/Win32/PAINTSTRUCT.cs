// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PAINTSTRUCT.cs -- used to paint client area of a window
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// The PAINTSTRUCT structure contains information for an application. 
    /// This information can be used to paint the client area of a window 
    /// owned by that application. 
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = 64)]
    // ReSharper disable once InconsistentNaming
    public struct PAINTSTRUCT
    {
        /// <summary>
        /// Handle to the display DC to be used for painting.
        /// </summary>
        [FieldOffset(0)]
        public IntPtr hdc;

        /// <summary>
        /// Specifies whether the background must be erased. This value is 
        /// nonzero if the application should erase the background. The 
        /// application is responsible for erasing the background if a window 
        /// class is created without a background brush.
        /// </summary>
        [FieldOffset(4)]
        public int fErase;

        /// <summary>
        /// Specifies a RECT structure that specifies the upper left and lower 
        /// right corners of the rectangle in which the painting is requested, 
        /// in device units relative to the upper-left corner of the client area.
        /// </summary>
        [FieldOffset(8)]
        public Rectangle rcPaint;

        /// <summary>
        /// Reserved; used internally by the system.
        /// </summary>
        [FieldOffset(24)]
        public int fRestore;

        /// <summary>
        /// Reserved; used internally by the system.
        /// </summary>
        [FieldOffset(28)]
        public int fIncUpdate;

        /// <summary>
        /// Reserved; used internally by the system.
        /// </summary>
        [FieldOffset(32)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] rgbReserved;
    }
}
