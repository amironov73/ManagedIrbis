// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DocInfoFlags.cs -- specifies additional information about the print job
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Specifies additional information about the print job.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum DocInfoFlags
    {
        /// <summary>
        /// Applications that use banding should set this flag for optimal 
        /// performance during printing.
        /// </summary>
        DI_APPBANDING = 0x00000001,

        /// <summary>
        /// The application will use raster operations that involve reading 
        /// from the destination surface.
        /// </summary>
        DI_ROPS_READ_DESTINATION = 0x00000002
    }
}
