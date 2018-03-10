// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LVCOLUMN.cs -- column in report view
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
    /// Contains information about a column in report view.
    /// This structure is used both for creating and manipulating columns.
    /// This structure supersedes the LVCOLUMN structure.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct LVCOLUMN
    {
        /// <summary>
        /// Variable specifying which members contain valid information.
        /// </summary>
        public ListViewColumnFlags mask;

        /// <summary>
        /// Alignment of the column header and the subitem text
        /// in the column. The alignment of the leftmost column
        /// is always left-justified; it cannot be changed.
        /// </summary>
        public ListViewColumnFormat fmt;

        /// <summary>
        /// Width of the column, in pixels.
        /// </summary>
        public int cx;

        /// <summary>
        /// If column information is being set, this member is the address
        /// of a null-terminated string that contains the column header text.
        /// If the structure is receiving information about a column, this
        /// member specifies the address of the buffer that receives the
        /// column header text.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszText;

        /// <summary>
        /// Size of the buffer pointed to by the pszText member.
        /// If the structure is not receiving information about
        /// a column, this member is ignored.
        /// </summary>
        public int cchTextMax;

        /// <summary>
        /// Index of subitem associated with the column.
        /// </summary>
        public int iSubItem;

        /// <summary>
        /// Version 4.70. Zero-based index of an image within the image list.
        /// The specified image will appear within the column.
        /// </summary>
        public int iImage;

        /// <summary>
        /// Version 4.70. Zero-based column offset. Column offset
        /// is in left-to-right order. For example, zero indicates
        /// the leftmost column.
        /// </summary>
        public int iOrder;
    }
}
