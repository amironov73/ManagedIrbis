// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BrushStyle.cs -- specifies the brush style.
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Specifies the brush style.
    /// </summary>
    [PublicAPI]
    public enum BrushStyle
    {
        /// <summary>
        /// Solid brush.
        /// </summary>
        BS_SOLID = 0,

        /// <summary>
        /// Same as BS_HOLLOW.
        /// </summary>
        BS_NULL = 1,

        /// <summary>
        /// Hollow brush.
        /// </summary>
        BS_HOLLOW = 0,

        /// <summary>
        /// Hatched brush.
        /// </summary>
        BS_HATCHED = 2,

        /// <summary>
        /// Pattern brush defined by a memory bitmap.
        /// </summary>
        BS_PATTERN = 3,

        /// <summary>
        /// ???
        /// </summary>
        BS_INDEXED = 4,

        /// <summary>
        /// <para>A pattern brush defined by a device-independent bitmap 
        /// (DIB) specification. If lbStyle is BS_DIBPATTERN, the lbHatch 
        /// member contains a handle to a packed DIB.</para>
        /// <para>Windows 95: Creating brushes from bitmaps or DIBs larger 
        /// than 8 by 8 pixels is not supported. If a larger bitmap is specified, 
        /// only a portion of the bitmap is used.</para>
        /// </summary>
        BS_DIBPATTERN = 5,

        /// <summary>
        /// A pattern brush defined by a device-independent bitmap (DIB) 
        /// specification. If lbStyle is BS_DIBPATTERNPT, the lbHatch member 
        /// contains a pointer to a packed DIB.
        /// </summary>
        BS_DIBPATTERNPT = 6,

        /// <summary>
        /// Same as BS_PATTERN.
        /// </summary>
        BS_PATTERN8X8 = 7,

        /// <summary>
        /// Same as BS_DIBPATTERN.
        /// </summary>
        BS_DIBPATTERN8X8 = 8,

        /// <summary>
        /// ???
        /// </summary>
        BS_MONOPATTERN = 9
    }
}
