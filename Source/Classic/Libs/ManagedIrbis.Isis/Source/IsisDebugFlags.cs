// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IsisDebugFlags.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

#endregion

namespace ManagedIrbis.Isis
{
    /// <summary>
    /// Флаги отладки ISIS32.DLL.
    /// </summary>
    [Flags]
    public enum IsisDebugFlags
    {
        /// <summary>
        /// 
        /// </summary>
        SHOW_NEVER = 0,

        /// <summary>
        /// 
        /// </summary>
        SHOW_FATAL = 1,

        /// <summary>
        /// 
        /// </summary>
        SHOW_ALWAYS = 3,

        /// <summary>
        /// 
        /// </summary>
        EXIT_NEVER = 0,

        /// <summary>
        /// 
        /// </summary>
        EXIT_FATAL = 16,

        /// <summary>
        /// 
        /// </summary>
        EXIT_ALWAYS = 48,

        /// <summary>
        /// 
        /// </summary>
        DEBUG_VERY_LIGHT = 0,   /* SHOW_NEVER   | EXIT_NEVER  */

        /// <summary>
        /// 
        /// </summary>
        DEBUG_LIGHT = 17,   /* SHOW_FATAL   | EXIT_FATAL  */

        /// <summary>
        /// 
        /// </summary>
        DEBUG_HARD = 19,   /* SHOW_ALWAYS  | EXIT_FATAL  */

        /// <summary>
        /// 
        /// </summary>
        DEBUG_VERY_HARD = 51   /* SHOW_ALWAYS  | EXIT_ALWAYS */
    }
}
