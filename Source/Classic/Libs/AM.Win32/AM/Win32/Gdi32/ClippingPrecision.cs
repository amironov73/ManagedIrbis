// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ClippingPrecision.cs -- the clipping precision
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// The clipping precision. The clipping precision defines
    /// how to clip characters that are partially outside
    /// the clipping region. It can be one or more of the following values.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum ClippingPrecision
    {
        /// <summary>
        /// Specifies default clipping behavior.
        /// </summary>
        CLIP_DEFAULT_PRECIS = 0,

        /// <summary>
        /// Not used.
        /// </summary>
        CLIP_CHARACTER_PRECIS = 1,

        /// <summary>
        /// Not used by the font mapper, but is returned when raster,
        /// vector, or TrueType fonts are enumerated.
        /// For compatibility, this value is always returned
        /// when enumerating fonts.
        /// </summary>
        CLIP_STROKE_PRECIS = 2,

        /// <summary>
        /// Not used.
        /// </summary>
        CLIP_MASK = 0x0F,

        /// <summary>
        /// When this value is used, the rotation for all fonts
        /// depends on whether the orientation of the coordinate
        /// system is left-handed or right-handed.
        /// If not used, device fonts always rotate counterclockwise,
        /// but the rotation of other fonts is dependent
        /// on the orientation of the coordinate system.
        /// For more information about the orientation of coordinate
        /// systems, see the description of the nOrientation parameter.
        /// </summary>
        CLIP_LH_ANGLES = 1 << 4,

        /// <summary>
        /// Not used.
        /// </summary>
        CLIP_TT_ALWAYS = 2 << 4,

        /// <summary>
        /// You must specify this flag to use an embedded read-only font.
        /// </summary>
        CLIP_EMBEDDED = 8 << 4
    }
}
