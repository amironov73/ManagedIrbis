// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OPENCARDNAME.cs -- 
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
    /// The OPENCARDNAME structure contains
    /// the information that the GetOpenCardName
    /// function uses to initialize a smart card Select Card dialog box.
    /// Calling SCardUIDlgSelectCard with OPENCARDNAME_EX is recommended
    /// over calling GetOpenCardName with OPENCARDNAME.
    /// OPENCARDNAME is provided for backward compatibility.
    /// </summary>
    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct OPENCARDNAME
    {
        #region Public fields

        // TODO implement

        #endregion
    }
}
