/* TriState.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace AM
{
    /// <summary>
    /// Three-state logic.
    /// </summary>
    /// <remarks>See
    /// https://en.wikipedia.org/wiki/Three-state_logic
    /// </remarks>
    public enum TriState
    {
        /// <summary>
        /// False.
        /// </summary>
        False,

        /// <summary>
        /// True.
        /// </summary>
        True,

        /// <summary>
        /// Undefined.
        /// </summary>
        Undefined
    }
}
