// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* POINTL.cs -- coordinates of a point
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
    /// The POINTL structure contains the coordinates of a point.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct POINTL
    {
        /// <summary>
        /// The horizontal (x) coordinate of the point.
        /// </summary>
        public int x;

        /// <summary>
        /// The vertical (y) coordinate of the point.
        /// </summary>
        public int y;
    }
}
