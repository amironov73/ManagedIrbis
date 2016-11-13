/* IndexKind.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Kind of index in <see cref="IndexSpecification"/>.
    /// </summary>
    public enum IndexKind
    {
        /// <summary>
        /// Not specified.
        /// </summary>
        None,

        /// <summary>
        /// Ordinary index.
        /// </summary>
        Ordinary,

        /// <summary>
        /// Reverse index.
        /// </summary>
        Reverse,

        /// <summary>
        /// Last repeat.
        /// </summary>
        LastRepeat,

        /// <summary>
        /// New repeat.
        /// </summary>
        NewRepeat
    }
}
