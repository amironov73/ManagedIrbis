// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DLLVERSIONINFO.cs -- receives DLL-specific version information
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
    /// Receives DLL-specific version information.
    /// It is used with the DllGetVersion function.
    /// </summary>
    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public class DLLVERSIONINFO
    {
        /// <summary>
        /// The size of the structure, in bytes.
        /// This member must be filled in before calling the function.
        /// </summary>
        [CLSCompliant(false)]
        public uint cbSize;

        /// <summary>
        /// The major version of the DLL.
        /// For instance, if the DLL's version is 4.0.950,
        /// this value will be 4.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwMajorVersion;

        /// <summary>
        /// The minor version of the DLL.
        /// For instance, if the DLL's version is 4.0.950,
        /// this value will be 0.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwMinorVersion;

        /// <summary>
        /// The build number of the DLL.
        /// For instance, if the DLL's version is 4.0.950,
        /// this value will be 950.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwBuildNumber;

        /// <summary>
        /// Identifies the platform for which the DLL was built.
        /// </summary>
        [CLSCompliant(false)]
        public uint dwPlatformID;
    }
}
