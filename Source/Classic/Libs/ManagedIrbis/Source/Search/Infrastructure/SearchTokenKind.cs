// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchTokenKind.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// Token kind.
    /// </summary>
    internal enum SearchTokenKind
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
        Hash,

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
