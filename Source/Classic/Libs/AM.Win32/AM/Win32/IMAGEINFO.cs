// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IMAGEINFO.cs -- contains information about an image in an image list
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
    /// Contains information about an image in an image list.
    /// </summary>
    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public class IMAGEINFO
    {
        /// <summary>
        /// A handle to the bitmap that contains the images.
        /// </summary>
        public IntPtr hbmImage;

        /// <summary>
        /// A handle to a monochrome bitmap that contains the masks
        /// for the images. If the image list does not contain
        /// a mask, this member is NULL.
        /// </summary>
        public IntPtr hbmMask;

        /// <summary>
        /// Not used. This member should always be zero.
        /// </summary>
        public int Unused1;

        /// <summary>
        /// Not used. This member should always be zero.
        /// </summary>
        public int Unused2;

        /// <summary>
        /// The bounding rectangle of the specified image within the bitmap specified by hbmImage.
        /// </summary>
        public int rcImage_left;

        /// <summary>
        /// The bounding rectangle of the specified image within the bitmap specified by hbmImage.
        /// </summary>
        public int rcImage_top;

        /// <summary>
        /// The bounding rectangle of the specified image within the bitmap specified by hbmImage.
        /// </summary>
        public int rcImage_right;

        /// <summary>
        /// The bounding rectangle of the specified image within the bitmap specified by hbmImage.
        /// </summary>
        public int rcImage_bottom;
    }

}
