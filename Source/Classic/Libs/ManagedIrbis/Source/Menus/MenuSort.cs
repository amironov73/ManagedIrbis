/* Sort.cs -- MNU entries sorting
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Menus
{
    /// <summary>
    /// Menu sorting.
    /// </summary>
    [PublicAPI]
    public enum MenuSort
    {
        /// <summary>
        /// No sorting.
        /// </summary>
        None,

        /// <summary>
        /// Sort by code.
        /// </summary>
        ByCode,

        /// <summary>
        /// Sort by comment.
        /// </summary>
        ByComment
    }
}
