// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExecutionState.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Thread's execution requirements.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum ExecutionState
    {
        /// <summary>
        /// Informs the system that the thread is performing some 
        /// operation that is not normally detected as activity 
        /// by the system.
        /// </summary>
        ES_SYSTEM_REQUIRED = 0x00000001,

        /// <summary>
        /// Informs the system that the thread is performing some 
        /// operation that is not normally detected as display 
        /// activity by the system.
        /// </summary>
        ES_DISPLAY_REQUIRED = 0x00000002,

        /// <summary>
        /// Informs the system that a user is present. 
        /// If a user is present, the system will use 
        /// the power policy settings set by the user. 
        /// Otherwise, the system does not wake the 
        /// display device and will return to the 
        /// sleeping state as soon as possible.
        /// </summary>
        ES_USER_PRESENT = 0x00000004,

        /// <summary>
        /// Informs the system that the state being set should remain 
        /// in effect until the next call that uses ES_CONTINUOUS and 
        /// one of the other state flags is cleared.
        /// </summary>
        ES_CONTINUOUS = unchecked((int)0x80000000)
    }
}
