// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DosDeviceFlags.cs -- DefineDosDevice function options
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Controls some aspects of the DefineDosDevice function.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum DosDeviceFlags
    {
        /// <summary>
        /// If this value is specified, the function does not convert 
        /// the lpTargetPath string from an MS-DOS path to a path, 
        /// but takes it as is.
        /// </summary>
        DDD_RAW_TARGET_PATH = 0x00000001,

        /// <summary>
        /// <para>If this value is specified, the function removes 
        /// the specified definition for the specified device. 
        /// To determine which definition to remove, the function 
        /// walks the list of mappings for the device, looking for 
        /// a match of lpTargetPath against a prefix of each mapping 
        /// associated with this device. The first mapping that matches 
        /// is the one removed, and then the function returns.</para>
        /// <para>If lpTargetPath is NULL or a pointer to a NULL string, 
        /// the function will remove the first mapping associated with 
        /// the device and pop the most recent one pushed. If there is 
        /// nothing left to pop, the device name will be removed.</para>
        /// <para>If this value is not specified, the string pointed to 
        /// by the lpTargetPath parameter will become the new mapping 
        /// for this device.</para>
        /// </summary>
        DDD_REMOVE_DEFINITION = 0x00000002,

        /// <summary>
        /// If this value is specified along with DDD_REMOVE_DEFINITION, 
        /// the function will use an exact match to determine which mapping 
        /// to remove. Use this value to insure that you do not delete 
        /// something that you did not define.
        /// </summary>
        DDD_EXACT_MATCH_ON_REMOVE = 0x00000004,

        /// <summary>
        /// ???
        /// </summary>
        DDD_NO_BROADCAST_SYSTEM = 0x00000008,

        /// <summary>
        /// ???
        /// </summary>
        DDD_LUID_BROADCAST_DRIVE = 0x00000010
    }
}