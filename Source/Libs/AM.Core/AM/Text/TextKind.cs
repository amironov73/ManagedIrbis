/* TextKind.cs -- kind of the text
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace AM.Text
{
    /// <summary>
    /// Kind of the text.
    /// </summary>
    public enum TextKind
    {
        /// <summary>
        /// Plain text.
        /// </summary>
        PlainText,

        /// <summary>
        /// Rich text (RTF).
        /// </summary>
        RichText,

        /// <summary>
        /// HTML text.
        /// </summary>
        Html
    }
}
