/* DuplicateKeyException.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// Duplicate key found.
    /// </summary>
    [PublicAPI]
    public sealed class DuplicateKeyException
        : ArsMagnaException
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DuplicateKeyException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DuplicateKeyException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DuplicateKeyException
            (
                string message,
                Exception innerException
            )
            : base(message, innerException)
        {
        }

        #endregion
    }
}
