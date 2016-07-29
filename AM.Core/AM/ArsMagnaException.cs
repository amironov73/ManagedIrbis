/* FileUtility.cs -- file manipulation routines
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
    /// 
    /// </summary>
    [PublicAPI]
    public class ArsMagnaException
        : Exception
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ArsMagnaException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ArsMagnaException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ArsMagnaException
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
