// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IVerifiable.cs -- interface for object state verification
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace UnsafeAM
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
