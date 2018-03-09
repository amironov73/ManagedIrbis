// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CodePageEnumProc.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public delegate bool CodePageEnumProc 
        (
            string codePage
        );
}
