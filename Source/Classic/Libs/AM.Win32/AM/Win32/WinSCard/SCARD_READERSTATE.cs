// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* WinSCard.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;
using System.Text;
using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// The SCARD_READERSTATE structure is used by functions
    /// for tracking smart cards within readers.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCARD_READERSTATE
    {
        #region Fields

        int szReader;

        IntPtr pvUserData;

        int  dwCurrentState;

        int  dwEventState;

        int  cbAtr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
        byte[]   rgbAtr;

        #endregion
    }
}
