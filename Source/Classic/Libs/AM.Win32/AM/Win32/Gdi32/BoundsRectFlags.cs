// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BoundsRectFlags.cs -- flags for bounding rectangle
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Flags for bounding rectangle.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum BoundsRectFlags
    {
        /// <summary>
        /// Error.
        /// </summary>
        ERROR = 0,

        /// <summary>
        /// Clears the bounding rectangle after returning it. 
        /// If this flag is not set, the bounding rectangle will 
        /// not be cleared.
        /// </summary>
        DCB_RESET = 0x0001,

        /// <summary>
        /// 
        /// </summary>
        DCB_ACCUMULATE = 0x0002,

        /// <summary>
        /// 
        /// </summary>
        DCB_DIRTY = DCB_ACCUMULATE,

        /// <summary>
        /// 
        /// </summary>
        DCB_SET = (DCB_RESET | DCB_ACCUMULATE),

        /// <summary>
        /// 
        /// </summary>
        DCB_ENABLE = 0x0004,

        /// <summary>
        /// 
        /// </summary>
        DCB_DISABLE = 0x0008
    }
}
