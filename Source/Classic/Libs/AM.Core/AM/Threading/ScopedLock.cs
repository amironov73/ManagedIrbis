// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ScopedLock.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Threading;

using CodeJam;

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
    public sealed class ScopedLock
        : IDisposable
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        internal ScopedLock
            (
                [NotNull] Semaphore semaphore
            )
        {
            Code.NotNull(semaphore, "semaphore");

            _semaphore = semaphore;
            _semaphore.WaitOne();
        }

        #endregion

        #region Private members

        [NotNull]
        private readonly Semaphore _semaphore;

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            _semaphore.Release();
        }

        #endregion
    }
}
