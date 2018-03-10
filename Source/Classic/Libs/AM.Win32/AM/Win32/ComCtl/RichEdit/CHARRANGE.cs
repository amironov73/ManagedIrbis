// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CHARRANGE.cs -- range of characters in a rich edit control
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// <para>The CHARRANGE structure specifies a range of characters
    /// in a rich edit control. This structure is used with the EM_EXGETSEL
    /// and EM_EXSETSEL messages.
    /// </para>
    /// <para>If the cpMin and cpMax members are equal, the range is empty.
    /// The range includes everything if cpMin is 0 and cpMax is —1.
    /// </para>
    /// </summary>
    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct CHARRANGE
    {
        /// <summary>
        /// Character position index immediately preceding the first
        /// character in the range.
        /// </summary>
        public int cpMin;

        /// <summary>
        /// Character position immediately following the last character
        /// in the range.
        /// </summary>
        public int cpMax;
    }
}
