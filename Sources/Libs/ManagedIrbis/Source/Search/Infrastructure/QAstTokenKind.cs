/* QAstTokenKind.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// Token kind.
    /// </summary>
    public enum QAstTokenKind
    {
        /// <summary>
        /// No tokens.
        /// </summary>
        None,

        /// <summary>
        /// K=word
        /// </summary>
        Term,

        /// <summary>
        /// (
        /// </summary>
        LeftParenthesis,

        /// <summary>
        /// ,
        /// </summary>
        Comma,

        /// <summary>
        /// )
        /// </summary>
        RightParenthesis,

        /// <summary>
        /// #
        /// </summary>
        Sharp,

        /// <summary>
        /// +
        /// </summary>
        Plus,

        /// <summary>
        /// *
        /// </summary>
        Star,

        /// <summary>
        /// ^
        /// </summary>
        Hat,

        /// <summary>
        /// (G)
        /// </summary>
        G,

        /// <summary>
        /// (F)
        /// </summary>
        F,

        /// <summary>
        /// .
        /// </summary>
        Dot,

        /// <summary>
        /// /
        /// </summary>
        Slash,
    }
}
