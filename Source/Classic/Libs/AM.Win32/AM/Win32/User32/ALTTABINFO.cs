// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ALTTABINFO.cs -- status information for the application-switching (ALT+TAB) window
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Contains status information for the application-switching
    /// (ALT+TAB) window.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ALTTABINFO
    {
        /// <summary>
        /// Size of the structure in bytes.
        /// </summary>
        public const int SIZE = 40;

        /// <summary>
        /// Specifies the size, in bytes, of the structure.
        /// The caller must set this to sizeof(ALTTABINFO).
        /// </summary>
        public int cbSize;

        /// <summary>
        /// Specifies the number of items in the window.
        /// </summary>
        public int cItems;

        /// <summary>
        /// Specifies the number of columns in the window.
        /// </summary>
        public int cColumns;

        /// <summary>
        /// Specifies the number of rows in the window.
        /// </summary>
        public int cRows;

        /// <summary>
        /// Specifies the column of the item that has the focus.
        /// </summary>
        public int iColFocus;

        /// <summary>
        /// Specifies the row of the item that has the focus.
        /// </summary>
        public int iRowFocus;

        /// <summary>
        /// Specifies the width of each icon in the
        /// application-switching window.
        /// </summary>
        public int cxItem;

        /// <summary>
        /// Specifies the height of each icon in the
        /// application-switching window.
        /// </summary>
        public int cyItem;

        /// <summary>
        /// Specifies the top-left corner of the first icon.
        /// </summary>
        public Point ptStart;
    }
}
