// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DeviceOrientation.cs -- the orientation at which images should be presented
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// The orientation at which images should be presented
    /// or orientation of the paper.
    /// </summary>
    [PublicAPI]
    public enum DeviceOrientation
    {
        /// <summary>
        /// Portrait orientation.
        /// </summary>
        DMORIENT_PORTRAIT = 1,

        /// <summary>
        /// Landscape orientation.
        /// </summary>
        DMORIENT_LANDSCAPE = 2,

        /// <summary>
        /// The display orientation is the natural orientation
        /// of the display device; it should be used as the default.
        /// </summary>
        DMDO_DEFAULT = 0,

        /// <summary>
        /// The display orientation is rotated 90 degrees
        /// (measured clockwise) from DMDO_DEFAULT.
        /// </summary>
        DMDO_90 = 1,

        /// <summary>
        /// The display orientation is rotated 180 degrees
        /// (measured clockwise) from DMDO_DEFAULT.
        /// </summary>
        DMDO_180 = 2,

        /// <summary>
        /// The display orientation is rotated 270 degrees
        /// (measured clockwise) from DMDO_DEFAULT.
        /// </summary>
        DMDO_270 = 3
    }
}
