// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* WIN32_FIND_DATA.cs -- describes a file found by the FindFirstFile and similar functions
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Describes a file found by the FindFirstFile, 
    /// FindFirstFileEx, or FindNextFile function.
    /// </summary>
    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct WIN32_FIND_DATA
    {
        /// <summary>
        /// 
        /// </summary>
        public const int MAX_PATH = 260;

        /// <summary>
        /// File attributes of the file found.
        /// </summary>
        public FileAttributes dwFileAttributes;

        /// <summary>
        /// A FILETIME structure that specifies when the file 
        /// or directory was created. If the underlying file 
        /// system does not support creation time, this member 
        /// is zero.
        /// </summary>
        public FILETIME ftCreationTime;

        /// <summary>
        /// A FILETIME structure. For a file, the structure specifies 
        /// when the file was last read from or written to. For a 
        /// directory, the structure specifies when the directory 
        /// was created. For both files and directories, the specified 
        /// date will be correct, but the time of day will always be set 
        /// to midnight. If the underlying file system does not support 
        /// last access time, this member is zero.
        /// </summary>
        public FILETIME ftLastAccessTime;

        /// <summary>
        /// A FILETIME structure. For a file, the structure specifies 
        /// when the file was last written to. For a directory, the 
        /// structure specifies when the directory was created. If the 
        /// underlying file system does not support last write time, 
        /// this member is zero.
        /// </summary>
        public FILETIME ftLastWriteTime;

        /// <summary>
        /// High-order DWORD value of the file size, in bytes. 
        /// This value is zero unless the file size is greater than 
        /// MAXDWORD. The size of the file is equal to 
        /// (nFileSizeHigh * (MAXDWORD+1)) + nFileSizeLow. 
        /// </summary>
        public int nFileSizeHigh;

        /// <summary>
        /// Low-order DWORD value of the file size, in bytes.
        /// </summary>
        [CLSCompliant(false)]
        public uint nFileSizeLow;

        /// <summary>
        /// If the dwFileAttributes member includes the 
        /// FILE_ATTRIBUTE_REPARSE_POINT attribute, this member 
        /// specifies the reparse tag. Otherwise, this value is 
        /// undefined and should not be used.
        /// </summary>
        public int dwReserved0;

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        public int dwReserved1;

        /// <summary>
        /// A null-terminated string that specifies the name of the file.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
        public string cFileName;

        /// <summary>
        /// A null-terminated string that specifies an alternative name 
        /// for the file. This name is in the classic 8.3 (filename.ext) 
        /// file name format. 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
        public string cAlternateFileName;
    }
}