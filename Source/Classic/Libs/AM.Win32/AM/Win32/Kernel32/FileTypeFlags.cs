// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FileTypeFlags.cs -- file (device) types
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// File (device) types.
    /// </summary>
    [PublicAPI]
    public enum FileTypeFlags
    {
        /// <summary>
        /// Either the type of the specified file is unknown, 
        /// or the function failed.
        /// </summary>
        FILE_TYPE_UNKNOWN = 0x0000,

        /// <summary>
        /// The specified file is a disk file.
        /// </summary>
        FILE_TYPE_DISK = 0x0001,

        /// <summary>
        /// The specified file is a character file, typically an 
        /// LPT device or a console.
        /// </summary>
        FILE_TYPE_CHAR = 0x0002,

        /// <summary>
        /// The specified file is a socket, a named pipe, or an 
        /// anonymous pipe.
        /// </summary>
        FILE_TYPE_PIPE = 0x0003,

        /// <summary>
        /// Unused.
        /// </summary>
        FILE_TYPE_REMOTE = 0x8000
    }
}