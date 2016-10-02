/* PftSyntaxException.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;
using ManagedIrbis.Pft.Infrastructure;

#endregion

namespace ManagedIrbis.Pft
{
    /// <summary>
    /// Исключение, возникающее при разборе PFT-скрипта.
    /// </summary>
    public sealed class PftSyntaxException
        : PftException
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSyntaxException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSyntaxException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSyntaxException
            (
                [NotNull] PftToken token
            )
            : this ("Unexpected token: " + token)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSyntaxException
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

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSyntaxException
            (
                [NotNull] PftToken token,
                Exception innerException
            )
            : this 
                (
                    "Unexpected token: " + token,
                    innerException
                )
        {
        }

        #endregion

        #region Public methods

        #endregion
    }
}
