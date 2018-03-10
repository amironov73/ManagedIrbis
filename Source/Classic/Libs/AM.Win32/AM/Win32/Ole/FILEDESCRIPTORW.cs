// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FILEDESCRIPTORW.cs -- describes the properties of a file that is being copied
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Describes the properties of a file that is being
    /// copied by means of the clipboard during an
    /// OLE drag-and-drop operation.
    /// </summary>
    /// <remarks>
    /// <para>If the CFSTR_FILECONTENTS format that
    /// corresponds to this structure contains the file as a
    /// global memory object, nFileSizeHigh and nFileSizeLow
    /// specify the size of the associated memory block.
    /// If they are set, they can also be used if a user-interface
    /// needs to be displayed. For instance, if a file is about to be
    /// overwritten, you would normally use information from this
    /// structure to display a dialog box containing the size, data,
    /// and name of the file.</para>
    /// <para>To create a zero-length file, set the FD_FILESIZE
    /// flag in the dwFlags, and set nFileSizeHigh and nFileSizeLow
    /// to zero. The CFSTR_FILECONTENTS format should represent
    /// the file as either a stream or global memory object
    /// (TYMED_ISTREAM or TYMED_HGLOBAL).</para>
    /// </remarks>
    [PublicAPI]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct FILEDESCRIPTORW
    {
        /// <summary>
        /// Array of flags that indicate which of the other
        /// structure members contain valid data
        /// </summary>
        public FileDescriptorFlags dwFlags;

        /// <summary>
        /// File class identifier.
        /// </summary>
        public Guid clsid;

        /// <summary>
        /// Width and height of the file icon.
        /// </summary>
        public Size sizel;

        /// <summary>
        /// Screen coordinates of the file object.
        /// </summary>
        public Point pointl;

        /// <summary>
        /// File attribute flags. This will be a combination
        /// of the FILE_ATTRIBUTE_ values described
        /// in GetFileAttributes.
        /// </summary>
        public FileAttributes dwFileAttributes;

        /// <summary>
        /// <see cref="System.Runtime.InteropServices.ComTypes.FILETIME"/>
        /// structure that contains the time of file creation.
        /// </summary>
        public FILETIME ftCreationTime;

        /// <summary>
        /// <see cref="FILETIME"/>
        /// structure that contains the time that the
        /// file was last accessed.
        /// </summary>
        public FILETIME ftAccessTime;

        /// <summary>
        /// <see cref="FILETIME"/>
        /// structure that contains the time
        /// of the last write operation.
        /// </summary>
        public FILETIME ftLastWriteTime;

        /// <summary>
        /// High-order DWORD of the file size, in bytes.
        /// </summary>
        public int nFileSizeHigh;

        /// <summary>
        /// Low-order DWORD of the file size, in bytes.
        /// </summary>
        [CLSCompliant(false)]
        public uint nFileSizeLow;

        /// <summary>
        /// Null-terminated string that contains the name of the file.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string cFileName;
    }
}