/* PftNotConnectedException.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft
{
    /// <summary>
    /// Возникает, когда необходимо обращение к серверу,
    /// а подключение отсутствует.
    /// </summary>
    [PublicAPI]
    public sealed class PftNotConnectedException
        : PftException
    {
        #region Properties

        #endregion

        #region Construciton

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNotConnectedException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNotConnectedException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNotConnectedException
            (
                string message, 
                Exception innerException
            ) 
            : base 
            ( 
                message, 
                innerException
            )
        {
        }

        #endregion

        #region Public methods

        #endregion
    }
}
