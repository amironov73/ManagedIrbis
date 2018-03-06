// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BackgroundMode.cs -- background mix mode for text, hatch brush drawing on device context
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Background mix mode for text, hatch brush drawing on device context.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum BackgroundMode
    {
        /// <summary>
        /// Error.
        /// </summary>
        ERROR = 0,

        /// <summary>
        /// Transparent.
        /// </summary>
        TRANSPARENT = 1,

        /// <summary>
        /// Opaque.
        /// </summary>
        OPAQUE = 2
    }
}
