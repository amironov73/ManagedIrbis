// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OpenFileFlags.cs -- the action to be taken in the OpenFile function
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// The action to be taken in the OpenFile function.
    /// </summary>
    [Flags]
    [PublicAPI]
    [CLSCompliant(false)]
    public enum OpenFileFlags : ushort
    {
        /// <summary>
        /// Opens a file for reading only.
        /// </summary>
        OF_READ = 0x0000,

        /// <summary>
        /// Opens a file for write access only.
        /// </summary>
        OF_WRITE = 0x0001,

        /// <summary>
        /// Opens a file with read/write permissions.
        /// </summary>
        OF_READWRITE = 0x0002,

        /// <summary>
        /// For MS-DOS–based file systems, opens a file
        /// with compatibility mode, allows any process
        /// on a specified computer to open the file any number of times.
        /// Other efforts to open a file with other sharing modes fail.
        /// This flag is mapped to the FILE_SHARE_READ|FILE_SHARE_WRITE
        /// flags of the CreateFile function.
        /// </summary>
        OF_SHARE_COMPAT = 0x0000,

        /// <summary>
        /// Opens a file with exclusive mode, and denies both
        /// read/write access to other processes. If a file has been
        /// opened in any other mode for read/write access,
        /// even by the current process, the function fails.
        /// </summary>
        OF_SHARE_EXCLUSIVE = 0x0010,

        /// <summary>
        /// Opens a file and denies write access to other processes.
        /// On MS-DOS-based file systems, if a file has been opened
        /// in compatibility mode, or for write access by
        /// any other process, the function fails.
        /// This flag is mapped to the FILE_SHARE_READ flag of the CreateFile function.
        /// </summary>
        OF_SHARE_DENY_WRITE = 0x0020,

        /// <summary>
        /// Opens a file and denies read access to other processes.
        /// On MS-DOS-based file systems, if the file has been opened
        /// in compatibility mode, or for read access by any other process,
        /// the function fails.
        /// This flag is mapped to the FILE_SHARE_WRITE flag of the CreateFile function.
        /// </summary>
        OF_SHARE_DENY_READ = 0x0030,

        /// <summary>
        /// Opens a file without denying read or write access to other processes.
        /// On MS-DOS-based file systems, if the file has been opened
        /// in compatibility mode by any other process, the function fails.
        /// This flag is mapped to the FILE_SHARE_READ|FILE_SHARE_WRITE
        /// flags of the CreateFile function.
        /// </summary>
        OF_SHARE_DENY_NONE = 0x0040,

        /// <summary>
        /// Fills the OFSTRUCT structure, but does not do anything else.
        /// </summary>
        OF_PARSE = 0x0100,

        /// <summary>
        /// Deletes a file.
        /// </summary>
        OF_DELETE = 0x0200,

        /// <summary>
        /// Verifies that the date and time of a file are the same as when it was opened previously.
        /// This is useful as an extra check for read-only files.
        /// </summary>
        OF_VERIFY = 0x0400,

        /// <summary>
        /// Ignored. To produce a dialog box containing a Cancel button, use OF_PROMPT.
        /// </summary>
        OF_CANCEL = 0x0800,

        /// <summary>
        /// Creates a new file. If the file exists, it is truncated to zero (0) length.
        /// </summary>
        OF_CREATE = 0x1000,

        /// <summary>
        /// Displays a dialog box if a requested file does not exist.
        /// A dialog box informs a user that the system cannot find
        /// a file, and it contains Retry and Cancel buttons.
        /// The Cancel button directs OpenFile to return
        /// a file-not-found error message.
        /// </summary>
        OF_PROMPT = 0x2000,

        /// <summary>
        /// Opens a file and then closes it. Use this to test for the existence of a file.
        /// </summary>
        OF_EXIST = 0x4000,

        /// <summary>
        /// Opens a file by using information in the reopen buffer.
        /// </summary>
        OF_REOPEN = 0x8000
    }
}