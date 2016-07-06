/* ReadOnlyException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ReadOnlyException
        : ArsMagnaException
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReadOnlyException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReadOnlyException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReadOnlyException
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
