// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BbkException.cs -- исключение, возникающее при работе с ББК
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Systematization
{
    /// <summary>
    /// Исключение, возникающее при работе с ББК.
    /// </summary>
    [PublicAPI]
    public sealed class BbkException
        : ArsMagnaException
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
    }
}
