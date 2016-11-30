// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UdkException.cs -- исключение, возникающее при работе с УДК
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Systematization
{
    /// <summary>
    /// Исключение, возникающее при работе с УДК.
    /// </summary>
    [PublicAPI]
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
