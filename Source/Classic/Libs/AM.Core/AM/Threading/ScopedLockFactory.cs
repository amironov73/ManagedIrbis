// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ScopedLockFactory.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Threading;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Threading
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ScopedLockFactory
        : IDisposable
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ScopedLockFactory()
        {
            _semaphore = new Semaphore(1, 1);
        }

        #endregion

        #region Private members

        private readonly Semaphore _semaphore;

        #endregion

        #region Public methods

        /// <summary>
        /// Create lock.
        /// </summary>
        [NotNull]
        public ScopedLock CreateLock()
        {
            return new ScopedLock(_semaphore);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
#if NETCORE

            _semaphore.Dispose();

#else

            _semaphore.Close();

#endif
        }

            #endregion
    }
}
