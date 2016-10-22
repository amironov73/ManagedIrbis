/* AsyncSocketResult.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Net
{
#if NOTDEF

    [PublicAPI]
    [MoonSharpUserData]
    public sealed class AsyncSocketResult
        : Task
    {
        #region Properties

        public AsyncSocket Socket { get { return _socket; } }

        public CancellationTokenSource CancellationTokenSource
        {
            get { return _cancellationTokenSource; }
        }

        public CancellationToken CancellationToken
        {
            get { return _cancellationToken; }
        }

        #endregion

        #region Construction

        internal AsyncSocketResult(Action action)
            : base(action)
        {
        }

        #endregion

        #region Private members

        internal readonly AsyncSocket _socket;

        internal CancellationTokenSource _cancellationTokenSource;

        internal CancellationToken _cancellationToken;

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }

#endif
}
