/* IrbisNetworkException.exe --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using AM;
using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Network
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public sealed class IrbisNetworkException
        : ArsMagnaException
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="IrbisNetworkException"/> class.
        /// </summary>
        public IrbisNetworkException()
        {
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="IrbisNetworkException" />
        /// class with a specified error message.
        /// </summary>
        /// <param name="message">
        /// A message that describes the error.</param>
        public IrbisNetworkException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="IrbisNetworkException" /> class
        /// with a specified error message and a reference
        /// to the inner exception that is the cause
        /// of this exception.
        /// </summary>
        /// <param name="message">The error message
        /// that explains the reason for the exception.</param>
        /// <param name="innerException">The exception
        /// that is the cause of the current exception.
        /// If the <paramref name="innerException" /> parameter
        /// is not a null reference, the current exception
        /// is raised in a catch block that handles
        /// the inner exception.</param>
        public IrbisNetworkException
            (
                string message,
                Exception innerException
            )
            : base(message, innerException)
        {
        }
    }
}
