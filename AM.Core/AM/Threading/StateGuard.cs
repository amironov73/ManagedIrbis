/* StateGuard.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

using AM.IO;
using AM.Runtime;

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
    public sealed class StateGuard<T>
        : IDisposable
    {
        #region Properties

        #endregion

        #region Construction

        public StateGuard
            (
                ref T value
            )
        {
        }

        #endregion

        #region Private members

        private T _savedValue;

        #endregion

        #region Public methods

        #endregion

        #region IDisposable members

        /// <summary>
        /// Performs application-defined tasks associated
        /// with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            
        }

        #endregion
    }
}
