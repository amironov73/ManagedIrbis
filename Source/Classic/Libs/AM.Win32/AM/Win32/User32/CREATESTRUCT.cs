// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CREATESTRUCT.cs -- initialization parameters passed to the window procedure of an application
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Defines the initialization parameters passed to the window
    /// procedure of an application. These members are identical to
    /// the parameters of the CreateWindowEx function.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct CREATESTRUCT
    {
        /// <summary>
        /// <para>Contains additional data which may be used to create
        /// the window. If the window is being created as a result of
        /// a call to the CreateWindow or CreateWindowEx function, this
        /// member contains the value of the lpParam parameter specified
        /// in the function call.</para>
        /// <para>If the window being created is an multiple-document
        /// interface (MDI) window, this member contains a pointer to
        /// an MDICREATESTRUCT structure.</para>
        /// <para>Windows NT/2000/XP: If the window is being created
        /// from a dialog template, this member is the address of a
        /// SHORT value that specifies the size, in bytes, of the window
        /// creation data. The value is immediately followed by the
        /// creation data.</para>
        /// </summary>
        public IntPtr lpCreateParams;

        /// <summary>
        /// Handle to the module that owns the new window.
        /// </summary>
        public IntPtr hInstance;

        /// <summary>
        /// Handle to the menu to be used by the new window.
        /// </summary>
        public IntPtr hMenu;

        /// <summary>
        /// Handle to the parent window, if the window is a child window.
        /// If the window is owned, this member identifies the owner
        /// window. If the window is not a child or owned window, this
        /// member is NULL.
        /// </summary>
        public IntPtr hwndParent;

        /// <summary>
        /// Specifies the height of the new window, in pixels.
        /// </summary>
        public int cy;

        /// <summary>
        /// Specifies the width of the new window, in pixels.
        /// </summary>
        public int cx;

        /// <summary>
        /// Specifies the y-coordinate of the upper left corner of
        /// the new window. If the new window is a child window,
        /// coordinates are relative to the parent window. Otherwise,
        /// the coordinates are relative to the screen origin.
        /// </summary>
        public int y;

        /// <summary>
        /// Specifies the x-coordinate of the upper left corner of the
        /// new window. If the new window is a child window, coordinates
        /// are relative to the parent window. Otherwise, the coordinates
        /// are relative to the screen origin.
        /// </summary>
        public int x;

        /// <summary>
        /// Specifies the style for the new window.
        /// </summary>
        public int style;

        /// <summary>
        /// Pointer to a null-terminated string that specifies the name
        /// of the new window.
        /// </summary>
        public string lpszName;

        /// <summary>
        /// Pointer to a null-terminated string that specifies the class
        /// name of the new window.
        /// </summary>
        public string lpszClass;

        /// <summary>
        /// Specifies the extended window style for the new window.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwExStyle;
    }
}
