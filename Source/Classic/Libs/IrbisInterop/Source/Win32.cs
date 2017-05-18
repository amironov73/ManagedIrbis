// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Win32.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace IrbisInterop.Source
{
    /// <summary>
    /// Some Win32 interop.
    /// </summary>
    internal static class Win32
    {
        #region Constants

        private const string Kernel32 = "Kernel32";

        #endregion

        #region Interop methods

        /// <summary>
        /// Verifies that the calling process has read access
        /// to the specified range of memory.
        /// </summary>
        /// <param name="address">A pointer to the first byte
        /// of the memory block.</param>
        /// <param name="size">The size of the memory block,
        /// in bytes. If this parameter is zero,
        /// the return value is zero.</param>
        /// <returns>If the calling process has read access
        /// to all bytes in the specified memory range,
        /// the return value is zero.</returns>
        [DllImport(Kernel32)]
        public static extern bool IsBadReadPtr
            (
                int address,
                int size
            );

        /// <summary>
        /// Verifies that the calling process has write access
        /// to the specified range of memory.
        /// </summary>
        /// <param name="address">A pointer to the first byte
        /// of the memory block.</param>
        /// <param name="size">The size of the memory block,
        /// in bytes. If this parameter is zero,
        /// the return value is zero.</param>
        /// <returns>If the calling process has write access
        /// to all bytes in the specified memory range,
        /// the return value is zero.</returns>
        [DllImport(Kernel32)]
        public static extern bool IsBadWritePtr
            (
                int address,
                int size
            );

        #endregion

        #region Public methods


        #endregion
    }
}
