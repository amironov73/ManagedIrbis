// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PlatformAbstractionLayer.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.PlatformAbstraction
{
    /// <summary>
    /// Platform abstraction level.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PlatformAbstractionLayer
        : IDisposable
    {
        #region Public methods

        /// <summary>
        /// Get random number generator.
        /// </summary>
        public virtual Random GetRandomGenerator()
        {
            return new Random();
        }

        /// <summary>
        /// Get current date and time.
        /// </summary>
        public virtual DateTime Now()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// Get today date.
        /// </summary>
        public virtual DateTime Today()
        {
            return DateTime.Today;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            // Nothing to do here
        }

        #endregion
    }
}
