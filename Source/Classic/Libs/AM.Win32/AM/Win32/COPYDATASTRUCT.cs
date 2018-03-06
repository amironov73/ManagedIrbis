// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* COPYDATASTRUCT.cs -- contains data to be passed to another application by the WM_COPYDATA message
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
    /// Contains data to be passed to another application
    /// by the WM_COPYDATA message.
    /// </summary>
    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public class COPYDATASTRUCT
    {
        /// <summary>
        /// The data to be passed to the receiving application.
        /// </summary>
        public int dwData;

        /// <summary>
        /// The size, in bytes, of the data pointed
        /// to by the lpData member.
        /// </summary>
        public int cbData;

        /// <summary>
        /// The data to be passed to the receiving application.
        /// This member can be NULL.
        /// </summary>
        public IntPtr lpData;
    }
}
