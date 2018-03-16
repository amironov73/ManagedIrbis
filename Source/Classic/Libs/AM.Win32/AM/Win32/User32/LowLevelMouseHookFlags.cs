// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LowLevelMouseHookFlags.cs -- event-injected flags
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Specifies the event-injected flag.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum LowLevelMouseHookFlags
    {
        /// <summary>
        /// Test the event-injected flag. 
        /// </summary>
        LLMHF_INJECTED = 0x00000001
    }
}
