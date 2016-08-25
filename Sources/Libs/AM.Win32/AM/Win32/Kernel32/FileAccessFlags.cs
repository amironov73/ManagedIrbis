/* FileAccessFlags.cs -- file access rights for functions like CreateFile. 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// File access rights for functions like CreateFile.
    /// </summary>
    [Flags]
    public enum FileAccessFlags
    {
        /// <summary>
        /// For a file object, the right to read the corresponding 
        /// file data. For a directory object, the right to read the 
        /// corresponding directory data.
        /// </summary>
        FILE_READ_DATA = 0x0001,

        /// <summary>
        /// For a directory, the right to list the contents of the 
        /// directory.
        /// </summary>
        FILE_LIST_DIRECTORY = 0x0001,

        /// <summary>
        /// For a file object, the right to write data to the file. 
        /// For a directory object, the right to create a file in the 
        /// directory.
        /// </summary>
        FILE_WRITE_DATA = 0x0002,

        /// <summary>
        /// For a directory, the right to create a file in the directory.
        /// </summary>
        FILE_ADD_FILE = 0x0002,

        /// <summary>
        /// For a file object, the right to append data to the file. 
        /// For a directory object, the right to create a subdirectory.
        /// </summary>
        FILE_APPEND_DATA = 0x0004,

        /// <summary>
        /// For a directory, the right to create a subdirectory.
        /// </summary>
        FILE_ADD_SUBDIRECTORY = 0x0004,

        /// <summary>
        /// For a named pipe, the right to create a pipe.
        /// </summary>
        FILE_CREATE_PIPE_INSTANCE = 0x0004,

        /// <summary>
        /// The right to read extended file attributes.
        /// </summary>
        FILE_READ_EA = 0x0008,

        /// <summary>
        /// The right to write extended file attributes.
        /// </summary>
        FILE_WRITE_EA = 0x0010,

        /// <summary>
        /// For a native code file, the right to execute the file. 
        /// This access right given to scripts may cause the script 
        /// to be executable, depending on the script interpreter.
        /// </summary>
        FILE_EXECUTE = 0x0020,

        /// <summary>
        /// For a directory, the right to traverse the directory. 
        /// By default, users are assigned the BYPASS_TRAVERSE_CHECKING 
        /// privilege, which ignores the FILE_TRAVERSE access right. 
        /// </summary>
        FILE_TRAVERSE = 0x0020,

        /// <summary>
        /// For a directory, the right to delete a directory and all 
        /// the files it contains, including read-only files.
        /// </summary>
        FILE_DELETE_CHILD = 0x0040,

        /// <summary>
        /// The right to read file attributes.
        /// </summary>
        FILE_READ_ATTRIBUTES = 0x0080,

        /// <summary>
        /// The right to write file attributes.
        /// </summary>
        FILE_WRITE_ATTRIBUTES = 0x0100,

        /// <summary>
        /// ???
        /// </summary>
        DELETE = 0x00010000,

        /// <summary>
        /// ???
        /// </summary>
        READ_CONTROL = 0x00020000,

        /// <summary>
        /// ???
        /// </summary>
        WRITE_DAC = 0x00040000,

        /// <summary>
        /// ???
        /// </summary>
        WRITE_OWNER = 0x00080000,

        /// <summary>
        /// ???
        /// </summary>
        STANDARD_RIGHTS_REQUIRED = 0x000F0000,

        /// <summary>
        /// Includes READ_CONTROL, which is the right to read the 
        /// information in the file or directory object's security 
        /// descriptor. This does not include the information in the 
        /// SACL.
        /// </summary>
        STANDARD_RIGHTS_READ = READ_CONTROL,

        /// <summary>
        /// Includes WRITE_CONTROL, which is the right to write to the 
        /// directory object's security descriptor. This does not 
        /// include the information in the SACL.
        /// </summary>
        STANDARD_RIGHTS_WRITE = READ_CONTROL,

        /// <summary>
        /// ???
        /// </summary>
        STANDARD_RIGHTS_EXECUTE = READ_CONTROL,

        /// <summary>
        /// ???
        /// </summary>
        STANDARD_RIGHTS_ALL = 0x001F0000,

        /// <summary>
        /// ???
        /// </summary>
        SPECIFIC_RIGHTS_ALL = 0x0000FFFF,

        /// <summary>
        /// The right to specify a file handle in one of the wait 
        /// functions. However, for asynchronous file I/O operations, 
        /// you should wait on the event handle in an OVERLAPPED 
        /// structure rather than using the file handle for 
        /// synchronization.
        /// </summary>
        SYNCHRONIZE = 0x00100000,

        /// <summary>
        /// ???
        /// </summary>
        GENERIC_READ = unchecked((int)0x80000000),

        /// <summary>
        /// ???
        /// </summary>
        GENERIC_WRITE = 0x40000000,

        /// <summary>
        /// ???
        /// </summary>
        GENERIC_EXECUTE = 0x20000000,

        /// <summary>
        /// ???
        /// </summary>
        GENERIC_ALL = 0x10000000,

        /// <summary>
        /// All possible access rights for a file.
        /// </summary>
        FILE_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0x1FF,

        /// <summary>
        /// ???
        /// </summary>
        FILE_GENERIC_READ = STANDARD_RIGHTS_READ
            | FILE_READ_DATA
            | FILE_READ_ATTRIBUTES
            | FILE_READ_EA
            | SYNCHRONIZE,

        /// <summary>
        /// ???
        /// </summary>
        FILE_GENERIC_WRITE = STANDARD_RIGHTS_WRITE
            | FILE_WRITE_DATA
            | FILE_WRITE_ATTRIBUTES
            | FILE_WRITE_EA
            | FILE_APPEND_DATA
            | SYNCHRONIZE,

        /// <summary>
        /// ???
        /// </summary>
        FILE_GENERIC_EXECUTE = STANDARD_RIGHTS_EXECUTE
            | FILE_READ_ATTRIBUTES
            | FILE_EXECUTE
            | SYNCHRONIZE
    }
}
