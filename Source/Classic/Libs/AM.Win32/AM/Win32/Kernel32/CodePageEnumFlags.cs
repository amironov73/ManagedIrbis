// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CodePageEnumFlags.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    ///  Specifies the code pages to enumerate.
    /// </summary>
    [Flags]
    public enum CodePageEnumFlags
    {
        /// <summary>
        /// Enumerate only installed code pages.
        /// </summary>
        CP_INSTALLED = 0x00000001,

        /// <summary>
        /// Enumerate all supported code pages.
        /// </summary>
        CP_SUPPORTED = 0x00000002
    }
}
