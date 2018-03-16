// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HookProcedure.cs -- callback function used with the SetWindowsHookEx
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// The HookProcedure hook procedure is an application-defined 
    /// or library-defined callback function used with the 
    /// SetWindowsHookEx function.
    /// </summary>
    [PublicAPI]
    public delegate int HookProcedure
        (
            int code,
            int wParam,
            int lParam
        );
}
