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
        /// <remarks>
        /// E. g.: 3
        /// </remarks>
        Literal,

        /// <summary>
        /// Specified by expression.
        /// </summary>
        /// <remarks>
        /// E. g.: $x + 1
        /// </remarks>
        Expression,

        /// <summary>
        /// Last repeat.
        /// </summary>
        /// <remarks>
        /// *
        /// </remarks>
        LastRepeat,

        /// <summary>
        /// New repeat.
        /// </summary>
        /// <remarks>
        /// +
        /// </remarks>
        NewRepeat,

        /// <summary>
        /// Current repeat.
        /// </summary>
        /// <remarks>
        /// .
        /// </remarks>
        CurrentRepeat,

        /// <summary>
        /// All repeats.
        /// </summary>
        /// <remarks>
        /// -
        /// </remarks>
        AllRepeats
    }
}
