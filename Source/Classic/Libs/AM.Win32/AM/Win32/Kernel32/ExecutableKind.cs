// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExecutableKind.cs -- binary executable file type
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Binary executable file type.
    /// </summary>
    [PublicAPI]
    public enum ExecutableKind
    {
        /// <summary>
        /// A 32-bit Windows-based application.
        /// </summary>
        SCS_32BIT_BINARY = 0,

        /// <summary>
        /// An MS-DOS – based application.
        /// </summary>
        SCS_DOS_BINARY = 1,

        /// <summary>
        /// A 16-bit Windows-based application.
        /// </summary>
        SCS_WOW_BINARY = 2,

        /// <summary>
        /// A PIF file that executes an MS-DOS – based application.
        /// </summary>
        SCS_PIF_BINARY = 3,

        /// <summary>
        /// A POSIX – based application.
        /// </summary>
        SCS_POSIX_BINARY = 4,

        /// <summary>
        /// A 16-bit OS/2-based application.
        /// </summary>
        SCS_OS216_BINARY = 5,

        /// <summary>
        /// A 64-bit Windows-based application.
        /// </summary>
        SCS_64BIT_BINARY = 6
    }
}
