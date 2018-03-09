// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CreateFileFlags.cs -- flags for CreateFile function
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Flags for CreateFile function
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum CreateFileFlags
    {
        #region Attributes

        /// <summary>
        /// The file is read only. Applications can read the file but 
        /// cannot write to it or delete it.
        /// </summary>
        FILE_ATTRIBUTE_READONLY = 0x00000001,

        /// <summary>
        /// The file is hidden. It is not to be included in an ordinary 
        /// directory listing.
        /// </summary>
        FILE_ATTRIBUTE_HIDDEN = 0x00000002,

        /// <summary>
        /// The file is part of or is used exclusively by the operating 
        /// system.
        /// </summary>
        FILE_ATTRIBUTE_SYSTEM = 0x00000004,

        /// <summary>
        /// Directory.
        /// </summary>
        FILE_ATTRIBUTE_DIRECTORY = 0x00000010,

        /// <summary>
        /// The file should be archived. Applications use this 
        /// attribute to mark files for backup or removal.
        /// </summary>
        FILE_ATTRIBUTE_ARCHIVE = 0x00000020,

        /// <summary>
        /// Device.
        /// </summary>
        FILE_ATTRIBUTE_DEVICE = 0x00000040,

        /// <summary>
        /// The file has no other attributes set. This attribute 
        /// is valid only if used alone.
        /// </summary>
        FILE_ATTRIBUTE_NORMAL = 0x00000080,

        /// <summary>
        /// The file is being used for temporary storage. File systems 
        /// avoid writing data back to mass storage if sufficient cache 
        /// memory is available, because often the application deletes 
        /// the temporary file shortly after the handle is closed. In 
        /// that case, the system can entirely avoid writing the data. 
        /// Otherwise, the data will be written after the handle is 
        /// closed.
        /// </summary>
        FILE_ATTRIBUTE_TEMPORARY = 0x00000100,

        /// <summary>
        /// Sparse file.
        /// </summary>
        FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200,

        /// <summary>
        /// Reparse point.
        /// </summary>
        FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400,

        /// <summary>
        /// Compressed.
        /// </summary>
        FILE_ATTRIBUTE_COMPRESSED = 0x00000800,

        /// <summary>
        /// The data of the file is not immediately available. This 
        /// attribute indicates that the file data has been physically 
        /// moved to offline storage. This attribute is used by Remote 
        /// Storage, the hierarchical storage management software. 
        /// Applications should not arbitrarily change this attribute.
        /// </summary>
        FILE_ATTRIBUTE_OFFLINE = 0x00001000,

        /// <summary>
        /// The file will not be indexed by the content indexing 
        /// service. 
        /// </summary>
        FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000,

        /// <summary>
        /// <para>The file or directory is encrypted. For a file, 
        /// this means that all data in the file is encrypted. 
        /// For a directory, this means that encryption is the 
        /// default for newly created files and subdirectories.</para>
        /// <para>This flag has no effect if FILE_ATTRIBUTE_SYSTEM is 
        /// also specified.</para>
        /// </summary>
        FILE_ATTRIBUTE_ENCRYPTED = 0x00004000,

        #endregion

        #region Flags

        /// <summary>
        /// <para>Instructs the system to write through any intermediate 
        /// cache and go directly to disk.</para>
        /// <para>If FILE_FLAG_NO_BUFFERING is not also specified, so 
        /// that system caching is in effect, then the data is written 
        /// to the system cache, but is flushed to disk without delay.
        /// </para>
        /// <para>If FILE_FLAG_NO_BUFFERING is also specified, so that 
        /// system caching is not in effect, then the data is immediately 
        /// flushed to disk without going through the system cache. The 
        /// operating system also requests a write-through the hard disk 
        /// cache to persistent media. However, not all hardware supports 
        /// this write-through capability.</para>
        /// </summary>
        FILE_FLAG_WRITE_THROUGH = unchecked((int)0x80000000),

        /// <summary>
        /// <para>Instructs the system to initialize the object, so that 
        /// operations that take a significant amount of time to process 
        /// return ERROR_IO_PENDING. When the operation is finished, the 
        /// specified event is set to the signaled state.</para>
        /// <para>When you specify FILE_FLAG_OVERLAPPED, the file read 
        /// and write functions must specify an OVERLAPPED structure. 
        /// That is, when FILE_FLAG_OVERLAPPED is specified, an 
        /// application must perform overlapped reading and writing.
        /// </para>
        /// <para>When FILE_FLAG_OVERLAPPED is specified, the system 
        /// does not maintain the file pointer. The file position must 
        /// be passed as part of the lpOverlapped parameter (pointing to 
        /// an OVERLAPPED structure) to the file read and write functions.
        /// </para>
        /// <para>This flag also enables more than one operation to be 
        /// performed simultaneously with the handle (a simultaneous 
        /// read and write operation, for example).</para></summary>
        FILE_FLAG_OVERLAPPED = 0x40000000,

        /// <summary>
        /// <para>Instructs the system to open the file with no system 
        /// caching. This flag has no effect on hard disk caching. 
        /// When combined with FILE_FLAG_OVERLAPPED, the flag gives 
        /// maximum asynchronous performance, because the I/O does not 
        /// rely on the synchronous operations of the memory manager. 
        /// However, some I/O operations will take longer, because data 
        /// is not being held in the cache. Also, the file metadata may 
        /// still be cached. To flush the metadata to disk, use the 
        /// FlushFileBuffers function.</para>
        /// </summary>
        FILE_FLAG_NO_BUFFERING = 0x20000000,

        /// <summary>
        /// Indicates that the file is accessed randomly. The system can 
        /// use this as a hint to optimize file caching.
        /// </summary>
        FILE_FLAG_RANDOM_ACCESS = 0x10000000,

        /// <summary>
        /// <para>Indicates that the file is to be accessed sequentially 
        /// from beginning to end. The system can use this as a hint to 
        /// optimize file caching. If an application moves the file 
        /// pointer for random access, optimum caching may not occur; 
        /// however, correct operation is still guaranteed.</para>
        /// <para>Specifying this flag can increase performance for 
        /// applications that read large files using sequential access. 
        /// Performance gains can be even more noticeable for 
        /// applications that read large files mostly sequentially, but 
        /// occasionally skip over small ranges of bytes.</para>
        /// </summary>
        FILE_FLAG_SEQUENTIAL_SCAN = 0x08000000,

        /// <summary>
        /// <para>Indicates that the operating system is to delete 
        /// the file immediately after all of its handles have been 
        /// closed, not just the handle for which you specified 
        /// FILE_FLAG_DELETE_ON_CLOSE.</para>
        /// <para>If there are existing open handles to the file, the 
        /// call fails unless they were all opened with the 
        /// FILE_SHARE_DELETE share mode.</para>
        /// <para>Subsequent open requests for the file will fail, 
        /// unless they specify the FILE_SHARE_DELETE share mode.</para>
        /// </summary>
        FILE_FLAG_DELETE_ON_CLOSE = 0x04000000,

        /// <summary>
        /// <para>Indicates that the file is being opened or created 
        /// for a backup or restore operation. The system ensures that 
        /// the calling process overrides file security checks, 
        /// provided it has the SE_BACKUP_NAME and SE_RESTORE_NAME 
        /// privileges.</para>
        /// <para>You can also set this flag to obtain a handle to a 
        /// directory. Where indicated, a directory handle can be 
        /// passed to some functions in place of a file handle.</para>
        /// <para>Windows Me/98/95: This flag is not supported.</para>
        /// </summary>
        FILE_FLAG_BACKUP_SEMANTICS = 0x02000000,

        /// <summary>
        /// Indicates that the file is to be accessed according to 
        /// POSIX rules. This includes allowing multiple files with 
        /// names, differing only in case, for file systems that support 
        /// such naming. Use care when using this option because files 
        /// created with this flag may not be accessible by applications 
        /// written for MS-DOS or 16-bit Windows. 
        /// </summary>
        FILE_FLAG_POSIX_SEMANTICS = 0x01000000,

        /// <summary>
        /// Specifying this flag inhibits the reparse behavior of NTFS 
        /// reparse points. When the file is opened, a file handle is 
        /// returned, whether the filter that controls the reparse point 
        /// is operational or not. This flag cannot be used with the 
        /// CREATE_ALWAYS flag. 
        /// </summary>
        FILE_FLAG_OPEN_REPARSE_POINT = 0x00200000,

        /// <summary>
        /// Indicates that the file data is requested, but it should 
        /// continue to reside in remote storage. It should not be 
        /// transported back to local storage. This flag is intended 
        /// for use by remote storage systems.
        /// </summary>
        FILE_FLAG_OPEN_NO_RECALL = 0x00100000,

        /// <summary>
        /// ???
        /// </summary>
        FILE_FLAG_FIRST_PIPE_INSTANCE = 0x00080000

        #endregion
    }
}
