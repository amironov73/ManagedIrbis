// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FileSystemFlags.cs -- file system options
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// File system options.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum FileSystemFlags
    {
        /// <summary>
        /// The file system supports case-sensitive file names.
        /// </summary>
        FILE_CASE_SENSITIVE_SEARCH = 0x00000001,

        /// <summary>
        /// The file system preserves the case of file names 
        /// when it places a name on disk.
        /// </summary>
        FILE_CASE_PRESERVED_NAMES = 0x00000002,

        /// <summary>
        /// The file system supports Unicode in file names 
        /// as they appear on disk.
        /// </summary>
        FILE_UNICODE_ON_DISK = 0x00000004,

        /// <summary>
        /// The file system preserves and enforces ACLs. 
        /// For example, NTFS preserves and enforces ACLs, and FAT does not.
        /// </summary>
        FILE_PERSISTENT_ACLS = 0x00000008,

        /// <summary>
        /// The file system supports file-based compression.
        /// </summary>
        FILE_FILE_COMPRESSION = 0x00000010,

        /// <summary>
        /// The file system supports disk quotas.
        /// </summary>
        FILE_VOLUME_QUOTAS = 0x00000020,

        /// <summary>
        /// The file system supports sparse files.
        /// </summary>
        FILE_SUPPORTS_SPARSE_FILES = 0x00000040,

        /// <summary>
        /// The file system supports reparse points.
        /// </summary>
        FILE_SUPPORTS_REPARSE_POINTS = 0x00000080,

        /// <summary>
        /// ???
        /// </summary>
        FILE_SUPPORTS_REMOTE_STORAGE = 0x00000100,

        /// <summary>
        /// The specified volume is a compressed volume; 
        /// for example, a DoubleSpace volume.
        /// </summary>
        FILE_VOLUME_IS_COMPRESSED = 0x00008000,

        /// <summary>
        /// The file system supports object identifiers.
        /// </summary>
        FILE_SUPPORTS_OBJECT_IDS = 0x00010000,

        /// <summary>
        /// The file system supports the Encrypted File System (EFS).
        /// </summary>
        FILE_SUPPORTS_ENCRYPTION = 0x00020000,

        /// <summary>
        /// The file system supports named streams.
        /// </summary>
        FILE_NAMED_STREAMS = 0x00040000,

        /// <summary>
        /// <para>The specified volume is read-only.</para>
        /// <para>Windows 2000/NT and Windows Me/98/95:
        /// This value is not supported.</para>
        /// </summary>
        FILE_READ_ONLY_VOLUME = 0x00080000,

        /// <summary>
        /// The file system preserves the case of file names 
        /// when it places a name on disk.
        /// </summary>
        FS_CASE_IS_PRESERVED = FILE_CASE_PRESERVED_NAMES,

        /// <summary>
        /// The file system supports case-sensitive file names.
        /// </summary>
        FS_CASE_SENSITIVE = FILE_CASE_SENSITIVE_SEARCH,

        /// <summary>
        /// The file system supports Unicode in file names 
        /// as they appear on disk.
        /// </summary>
        FS_UNICODE_STORED_ON_DISK = FILE_UNICODE_ON_DISK,

        /// <summary>
        /// The file system preserves and enforces ACLs. 
        /// For example, NTFS preserves and enforces ACLs, and FAT does not.
        /// </summary>
        FS_PERSISTENT_ACLS = FILE_PERSISTENT_ACLS,

        /// <summary>
        /// The specified volume is a compressed volume; 
        /// for example, a DoubleSpace volume.
        /// </summary>
        FS_VOL_IS_COMPRESSED = FILE_VOLUME_IS_COMPRESSED,

        /// <summary>
        /// The file system supports file-based compression.
        /// </summary>
        FS_FILE_COMPRESSION = FILE_FILE_COMPRESSION,

        /// <summary>
        /// The file system supports the Encrypted File System (EFS).
        /// </summary>
        FS_FILE_ENCRYPTION = FILE_SUPPORTS_ENCRYPTION
    }
}