// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DRIVETRYPE.cs -- describes available types for local drives
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Describes available types for local drives.
    /// </summary>
    [PublicAPI]
    public enum DRIVETYPE
    {
        /// <summary>
        /// The drive type cannot be determined.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The root path is invalid. For example, no volume 
        /// is mounted at the path.
        /// </summary>
        NotRootDir = 1,

        /// <summary>
        /// The disk can be removed from the drive.
        /// </summary>
        Removable = 2,

        /// <summary>
        /// The disk cannot be removed from the drive.
        /// </summary>
        Fixed = 3,

        /// <summary>
        /// The drive is a remote (network) drive.
        /// </summary>
        Remote = 4,

        /// <summary>
        /// The drive is a CD-ROM drive.
        /// </summary>
        CDROM = 5,

        /// <summary>
        /// The drive is a RAM disk.
        /// </summary>
        RamDisk = 6,
    }
}
