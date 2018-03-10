// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FileDescriptorFlags.cs -- flags for FILEDESCRIPTOR structure
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Flags that indicate which of the other
    /// structure members of <see cref="FILEDESCRIPTORA"/>
    /// or <see cref="FILEDESCRIPTORW"/> contain valid data.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum FileDescriptorFlags
    {
        /// <summary>
        /// The clsid member is valid.
        /// </summary>
        FD_CLSID = 0x0001,

        /// <summary>
        /// The sizel and pointl members are valid.
        /// </summary>
        FD_SIZEPOINT = 0x0002,

        /// <summary>
        /// The dwFileAttributes member is valid.
        /// </summary>
        FD_ATTRIBUTES = 0x0004,

        /// <summary>
        /// The ftCreationTime member is valid.
        /// </summary>
        FD_CREATETIME = 0x0008,

        /// <summary>
        /// The ftLastAccessTime member is valid.
        /// </summary>
        FD_ACCESSTIME = 0x0010,

        /// <summary>
        /// The ftLastWriteTime member is valid.
        /// </summary>
        FD_WRITESTIME = 0x0020,

        /// <summary>
        /// The nFileSizeHigh and nFileSizeLow members are valid.
        /// </summary>
        FD_FILESIZE = 0x0040,

        /// <summary>
        /// A progress indicator is shown with drag-and-drop operations.
        /// </summary>
        FD_PROGRESSUI = 0x4000,

        /// <summary>
        /// Treat the operation as "Link."
        /// </summary>
        FD_LINKUI = 0x8000
    }
}