// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BITMAPINFO.cs -- defines the dimensions and color information for a DIB.
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
    /// The BITMAPINFO structure defines the dimensions and color 
    /// information for a DIB.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPINFO
    {
        /// <summary>
        /// Specifies a <see cref="BITMAPINFOHEADER">BITMAPINFOHEADER</see> 
        /// structure that contains information 
        /// about the dimensions of color format. 
        /// </summary>
        public BITMAPINFOHEADER bmiHeader;

        // Fixed-length Array "bmiColors[1]". Members can be
        // accessed with (&my_BITMAPINFO.bmiColors_1)[index]
        /// <summary>
        /// <para>The bmiColors member contains one of the following:</para>
        /// <para> * An array of RGBQUAD. The elements of the array that
        /// make up the color table.</para>
        /// <para> * An array of 16-bit unsigned integers that specifies 
        /// indexes into the currently realized logical palette. This use 
        /// of bmiColors is allowed for functions that use DIBs. When bmiColors 
        /// elements contain indexes to a realized logical palette, they must 
        /// also call the following bitmap functions: CreateDIBitmap, 
        /// CreateDIBPatternBrush, CreateDIBSection</para> 
        /// <para>The iUsage parameter of CreateDIBSection must be set 
        /// to DIB_PAL_COLORS.</para> 
        /// <para>The number of entries in the array depends on the values 
        /// of the biBitCount and biClrUsed members of the BITMAPINFOHEADER
        /// structure.</para>
        /// </summary>
        public RGBQUAD bmiColors_1;
    }
}
