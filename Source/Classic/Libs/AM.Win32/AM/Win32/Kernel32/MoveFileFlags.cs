// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MoveFileFlags.cs -- options for MoveFileEx
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Options for MoveFileEx and other functions.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum MoveFileFlags
    {
        /// <summary><para>If a file named lpNewFileName exists, 
        /// the function replaces its contents with the contents 
        /// of the lpExistingFileName file.</para>
        /// <para>This value cannot be used if lpNewFileName or 
        /// lpExistingFileName names a directory.</para></summary>
        MOVEFILE_REPLACE_EXISTING = 0x00000001,

        /// <summary><para>If the file is to be moved to a different volume, 
        /// the function simulates the move by using the CopyFile 
        /// and DeleteFile functions.</para>
        /// <para>This value cannot be used with MOVEFILE_DELAY_UNTIL_REBOOT.
        /// </para></summary>
        MOVEFILE_COPY_ALLOWED = 0x00000002,

        /// <summary><para>The system does not move the file until the 
        /// operating system is restarted. The system moves the file 
        /// immediately after AUTOCHK is executed, but before creating 
        /// any paging files. Consequently, this parameter enables the 
        /// function to delete paging files from previous startups.
        /// </para>
        /// <para>This value can only be used if the process is in the 
        /// context of a user who belongs to the administrator group 
        /// or the LocalSystem account.</para>
        /// <para>This value cannot be used with MOVEFILE_COPY_ALLOWED.
        /// </para></summary>
        MOVEFILE_DELAY_UNTIL_REBOOT = 0x00000004,

        /// <summary><para>The function does not return until the file 
        /// has actually been moved on the disk.</para>
        /// <para>Setting this value guarantees that a move performed 
        /// as a copy and delete operation is flushed to disk before 
        /// the function returns. The flush occurs at the end of the 
        /// copy operation.</para>
        /// <para>This value has no effect if MOVEFILE_DELAY_UNTIL_REBOOT 
        /// is set.</para></summary>
        MOVEFILE_WRITE_THROUGH = 0x00000008,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        MOVEFILE_CREATE_HARDLINK = 0x00000010,

        /// <summary>
        /// The function fails if the source file is a link source, 
        /// but the file cannot be tracked after the move. 
        /// This situation can occur if the destination is a volume 
        /// formatted with the FAT file system.
        /// </summary>
        MOVEFILE_FAIL_IF_NOT_TRACKABLE = 0x00000020
    }
}
