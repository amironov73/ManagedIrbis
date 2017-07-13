// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DirectAccessMode.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Modes for direct access.
    /// </summary>
    public enum DirectAccessMode
    {
        /// <summary>
        /// Exclusive access mode.
        /// </summary>
        Exclusive,

        /// <summary>
        /// Shared access mode.
        /// </summary>
        Shared,

        /// <summary>
        /// Read-only access mode.
        /// </summary>
        ReadOnly
    }
}
