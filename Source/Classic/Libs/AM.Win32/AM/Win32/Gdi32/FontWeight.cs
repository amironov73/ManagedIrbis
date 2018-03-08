// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FontWeight.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// The weight of the font in the range 0 through 1000.
    /// For example, 400 is normal and 700 is bold.
    /// If this value is zero, a default weight is used.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum FontWeight
    {
        /// <summary>
        /// Default weight is used.
        /// </summary>
        FW_DONTCARE = 0,

        /// <summary>
        /// Thin.
        /// </summary>
        FW_THIN = 100,

        /// <summary>
        /// Extra-light.
        /// </summary>
        FW_EXTRALIGHT = 200,

        /// <summary>
        /// Light.
        /// </summary>
        FW_LIGHT = 300,

        /// <summary>
        /// Normal.
        /// </summary>
        FW_NORMAL = 400,

        /// <summary>
        /// Medium.
        /// </summary>
        FW_MEDIUM = 500,

        /// <summary>
        /// Semi-bold.
        /// </summary>
        FW_SEMIBOLD = 600,

        /// <summary>
        /// Bold.
        /// </summary>
        FW_BOLD = 700,

        /// <summary>
        /// Extra-bold.
        /// </summary>
        FW_EXTRABOLD = 800,

        /// <summary>
        /// Heavy.
        /// </summary>
        FW_HEAVY = 900,

        /// <summary>
        /// Ultra-light.
        /// </summary>
        FW_ULTRALIGHT = FW_EXTRALIGHT,

        /// <summary>
        /// Regular.
        /// </summary>
        FW_REGULAR = FW_NORMAL,

        /// <summary>
        /// Demi-bold.
        /// </summary>
        FW_DEMIBOLD = FW_SEMIBOLD,

        /// <summary>
        /// Ultra-bold.
        /// </summary>
        FW_ULTRABOLD = FW_EXTRABOLD,

        /// <summary>
        /// Black.
        /// </summary>
        FW_BLACK = FW_HEAVY
    }
}
