/* UdkException.cs -- исключение, возникающее при работе с УДК
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace ManagedClient.Systematization
{
    /// <summary>
    /// Исключение, возникающее при работе с УДК.
    /// </summary>
    [PublicAPI]
    [Serializable]
    public sealed class UdkException
        : IrbisException
    {
        #region Construciton

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UdkException()
        {
        }

        #endregion

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UdkException
            (
                [CanBeNull] string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UdkException
            (
                [CanBeNull] string message,
                [CanBeNull] Exception innerException
            )
            : base(message, innerException)
        {
        }
    }
}
