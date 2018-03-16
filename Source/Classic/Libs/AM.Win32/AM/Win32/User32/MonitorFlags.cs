// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MonitorFlags.cs -- Determines the function's return value if the window does not intersect any display monitor
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Determines the function's return value if the window does not 
    /// intersect any display monitor.
    /// </summary>
    [PublicAPI]
    public enum MonitorFlags
    {
        /// <summary>
        /// Returns NULL.
        /// </summary>
        MONITOR_DEFAULTTONULL = 0x00000000,

        /// <summary>
        /// Returns a handle to the primary display monitor. 
        /// </summary>
        MONITOR_DEFAULTTOPRIMARY = 0x00000001,

        /// <summary>
        /// Returns a handle to the display monitor that 
        /// is nearest to the window.
        /// </summary>
        MONITOR_DEFAULTTONEAREST = 0x00000002
    }
}
