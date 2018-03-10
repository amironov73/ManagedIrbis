// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ActivateState.cs -- flags for WM_ACTIVATE message
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Flags for WM_ACTIVATE message (low word of wparam).
    /// </summary>
    [PublicAPI]
    [CLSCompliant(false)]
    public enum ActivateState : ushort
    {
        /// <summary>
        /// Window has been deactivated.
        /// </summary>
        WA_INACTIVE = 0,

        /// <summary>
        /// Window activated by other than a mouse click,
        /// like call to SetActiveWindow.
        /// </summary>
        WA_ACTIVE = 1,

        /// <summary>
        /// Window activated by a mouse click.
        /// </summary>
        WA_CLICKACTIVE = 2
    }
}
