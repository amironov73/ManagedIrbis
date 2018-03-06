// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BITMAPINFOHEADER.cs -- contains information about the dimensions and color format of a DIB
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
    /// <para>The BITMAPINFOHEADER structure contains information 
    /// about the dimensions and color format of a DIB.</para>
    /// <para>Windows 95, Windows NT 4.0: Applications can use the 
    /// BITMAPV4HEADER structure for added functionality.</para>
    /// <para>Windows 98/Me, Windows 2000/XP: Applications can use 
    /// the BITMAPV5HEADER structure for added functionality. However, 
    /// these are used only in the CreateDIBitmap function.</para>
    /// </summary>
    [PublicAPI]
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPINFOHEADER
    {
        /// <summary>
        /// Specifies the number of bytes required by the structure.
        /// </summary>
        [CLSCompliant(false)]
        public uint biSize;

        /// <summary>
        /// <para>Specifies the width of the bitmap, in pixels.</para> 
        /// <para>Windows 98/Me, Windows 2000/XP>: If biCompression 
        /// is BI_JPEG or BI_PNG, the biWidth member specifies the 
        /// width of the decompressed JPEG or PNG image file, respectively.
        /// </para> 
        /// </summary>
        public int biWidth;

        /// <summary>
        /// <para>Specifies the height of the bitmap, in pixels.</para> 
        /// <para>If biHeight is positive, the bitmap is a bottom-up 
        /// DIB and its origin is the lower-left corner. If biHeight is 
        /// negative, the bitmap is a top-down DIB and its origin is the 
        /// upper-left corner. If biHeight is negative, indicating a 
        /// top-down DIB, biCompression must be either BI_RGB or BI_BITFIELDS. 
        /// Top-down DIBs cannot be compressed.</para> 
        /// <para>Windows 98/Me, Windows 2000/XP: If biCompression is BI_JPEG 
        /// or BI_PNG, the biHeight member specifies the height of the 
        /// decompressed JPEG or PNG image file, respectively.</para>
        /// </summary>
        public int biHeight;

        /// <summary>
        /// Specifies the number of planes for the target device. This value 
        /// must be set to 1.
        /// </summary>
        [CLSCompliant(false)]
        public ushort biPlanes;

        /// <summary>
        /// Specifies the number of bits-per-pixel. The biBitCount member 
        /// of the BITMAPINFOHEADER structure determines the number of bits 
        /// that define each pixel and the maximum number of colors in the 
        /// bitmap. This member must be one of the following values: 
        /// 0, 1, 4, 8, 16, 24, 32.
        /// </summary>
        [CLSCompliant(false)]
        public ushort biBitCount;

        /// <summary>
        /// Specifies the type of compression for a compressed bottom-up bitmap 
        /// (top-down DIBs cannot be compressed). See <see cref="BitmapCompression">
        /// BitmapCompression enum.</see>. 
        /// </summary>
        [CLSCompliant(false)]
        public uint biCompression;

        /// <summary>
        /// <para>Specifies the size, in bytes, of the image. This may be set to 
        /// zero for BI_RGB bitmaps.</para> 
        /// <para>Windows 98/Me, Windows 2000/XP: If biCompression is BI_JPEG 
        /// or BI_PNG, biSizeImage indicates the size of the JPEG or PNG image
        /// buffer, respectively.</para>
        /// </summary>
        [CLSCompliant(false)]
        public uint biSizeImage;

        /// <summary>
        /// Specifies the horizontal resolution, in pixels-per-meter, of the 
        /// target device for the bitmap. An application can use this value 
        /// to select a bitmap from a resource group that best matches the 
        /// characteristics of the current device. 
        /// </summary>
        public int biXPelsPerMeter;

        /// <summary>
        /// Specifies the vertical resolution, in pixels-per-meter, of the 
        /// target device for the bitmap. 
        /// </summary>
        public int biYPelsPerMeter;

        /// <summary>
        /// <para>Specifies the number of color indexes in the color table 
        /// that are actually used by the bitmap. If this value is zero, 
        /// the bitmap uses the maximum number of colors corresponding to 
        /// the value of the biBitCount member for the compression mode 
        /// specified by biCompression.</para>
        /// <para>If biClrUsed is nonzero and the biBitCount member is 
        /// less than 16, the biClrUsed member specifies the actual number 
        /// of colors the graphics engine or device driver accesses. 
        /// If biBitCount is 16 or greater, the biClrUsed member specifies 
        /// the size of the color table used to optimize performance of the 
        /// system color palettes. If biBitCount equals 16 or 32, the optimal 
        /// color palette starts immediately following the three DWORD masks.
        /// </para> 
        /// <para>When the bitmap array immediately follows the 
        /// <see cref="BITMAPINFO">BITMAPINFO structure</see>, it is a packed 
        /// bitmap. Packed bitmaps are referenced by a single pointer. Packed 
        /// bitmaps require that the biClrUsed member must be either zero or 
        /// the actual size of the color table.</para>
        /// </summary>
        [CLSCompliant(false)]
        public uint biClrUsed;

        /// <summary>
        /// Specifies the number of color indexes that are required for displaying the bitmap. 
        /// If this value is zero, all colors are required. 
        /// </summary>
        [CLSCompliant(false)]
        public uint biClrImportant;
    }
}
