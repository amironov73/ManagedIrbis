// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StringSplitOptions.cs -- for WinMobile compatibility
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if WINMOBILE

namespace System
{
    /// <summary>
    /// For WinMobile compatibility.
    /// </summary>
    public enum StringSplitOptions
    {
        /// <summary>
        /// The return value includes array elements that contain
        /// an empty string.
        /// </summary>
        None = 0,

        /// <summary>
        /// The return value does not include array elements
        /// that contain an empty string.
        /// </summary>
        RemoveEmptyEntries = 1
    }
}

#endif
