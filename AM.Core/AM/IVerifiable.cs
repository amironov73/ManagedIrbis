/* IVerifiable.cs -- interface for object state verification
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace AM
{
    /// <summary>
    /// Interface for object state verification
    /// </summary>
    public interface IVerifiable
    {
        /// <summary>
        /// Verify object state.
        /// </summary>
        bool Verify
            (
                bool throwOnError
            );
    }
}
