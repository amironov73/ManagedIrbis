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
        /// Specified by literal.
        /// </summary>
        Literal,

        /// <summary>
        /// Specified by expression.
        /// </summary>
        Expression,

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
