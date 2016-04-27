/* BbkException.cs -- исключение, возникающее при работе с ББК
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

#endregion

namespace ManagedClient.Systematization
{
    /// <summary>
    /// Исключение, возникающее при работе с ББК.
    /// </summary>
    [PublicAPI]
    [Serializable]
    public sealed class BbkException
        : ApplicationException
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BbkException()
        {
        }

        #endregion

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BbkException
            (
                [CanBeNull] string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BbkException
            (
                [CanBeNull] string message, 
                [CanBeNull] Exception innerException
            )
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        private BbkException
            (
                [NotNull] SerializationInfo info, 
                StreamingContext context
            )
            : base(info, context)
        {
        }
    }
}
