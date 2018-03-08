// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ROP2.cs -- binary raster operations
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Binary raster operations.
    /// </summary>
    [PublicAPI]
    public enum ROP2
    {
        /// <summary>
        /// Error.
        /// </summary>
        ERROR = 0,

        /// <summary>
        /// Pixel is always 0.
        /// </summary>
        R2_BLACK = 1,

        /// <summary>
        /// Pixel is the inverse of the R2_MERGEPEN color.
        /// </summary>
        R2_NOTMERGEPEN = 2,

        /// <summary>
        /// Pixel is a combination of the colors common to both 
        /// the screen and the inverse of the pen.
        /// </summary>
        R2_MASKNOTPEN = 3,

        /// <summary>
        /// Pixel is the inverse of the pen color.
        /// </summary>
        R2_NOTCOPYPEN = 4,

        /// <summary>
        /// Pixel is a combination of the colors common to both 
        /// the pen and the inverse of the screen.
        /// </summary>
        R2_MASKPENNOT = 5,

        /// <summary>
        /// Pixel is the inverse of the screen color.
        /// </summary>
        R2_NOT = 6,

        /// <summary>
        /// Pixel is a combination of the colors in the pen and 
        /// in the screen, but not in both.
        /// </summary>
        R2_XORPEN = 7,

        /// <summary>
        /// Pixel is the inverse of the R2_MASKPEN color.
        /// </summary>
        R2_NOTMASKPEN = 8,

        /// <summary>
        /// Pixel is a combination of the colors common to both 
        /// the pen and the screen.
        /// </summary>
        R2_MASKPEN = 9,

        /// <summary>
        /// Pixel is the inverse of the R2_XORPEN color.
        /// </summary>
        R2_NOTXORPEN = 10,

        /// <summary>
        /// Pixel remains unchanged.
        /// </summary>
        R2_NOP = 11,

        /// <summary>
        /// Pixel is a combination of the screen color and 
        /// the inverse of the pen color.
        /// </summary>
        R2_MERGENOTPEN = 12,

        /// <summary>
        /// Pixel is the pen color.
        /// </summary>
        R2_COPYPEN = 13,

        /// <summary>
        /// Pixel is a combination of the pen color and 
        /// the inverse of the screen color.
        /// </summary>
        R2_MERGEPENNOT = 14,

        /// <summary>
        /// Pixel is a combination of the pen color and the screen color.
        /// </summary>
        R2_MERGEPEN = 15,

        /// <summary>
        /// Pixel is always 1.
        /// </summary>
        R2_WHITE = 16,

        /// <summary>
        /// ???
        /// </summary>
        R2_LAST = 16
    }
}
