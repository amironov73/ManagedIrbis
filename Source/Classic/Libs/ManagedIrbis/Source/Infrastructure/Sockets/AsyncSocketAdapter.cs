// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AsyncSocketAdapter.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if (CLASSIC && (FW45 || FW46)) || NETCORE || ANDROID || UAP

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class AsyncSocketAdapter
        : AbstractClientSocket
    {
        #region Properties

        /// <summary>
        /// Delay, milliseconds
        /// (for debug purposes).
        /// </summary>
        public int Delay { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsyncSocketAdapter
            (
                [NotNull] IrbisConnection connection,
                [NotNull] AbstractClientSocket innerSocket
            )
            : base(connection)
        {
            Code.NotNull(innerSocket, "innerSocket");

            InnerSocket = innerSocket;
            Delay = 0;
        }

        #endregion

        #region Private members

        private byte[] _result;

        // Must be async Task method!
        private async Task __Execute
            (
                byte[][] request
            )
        {
            AbstractClientSocket innerSocket = InnerSocket
                .ThrowIfNull("InnerSocket not set");

            innerSocket.ExecuteRequest(request);

            if (Delay > 0)
            {
                await Task.Delay(Delay);
            }
        }

        // ExecuteRequest can't use await,
        // so we must create intemediate method
        private async void _Execute
            (
                byte[][] request
            )
        {
            try
            {
                await __Execute(request);
            }
            catch (AggregateException exception)
            {
                Log.TraceException
                    (
                        "AsyncSocketAdapter::_Execute",
                        exception
                    );

                // TODO: intelligent handling!
                exception.Handle(ex => true);
            }
        }

        #endregion

        #region Public methods

        #endregion

        #region AbstractClientSocket members

        /// <inheritdoc cref="AbstractClientSocket.AbortRequest"/>
        public override void AbortRequest()
        {

        }

        /// <inheritdoc cref="AbstractClientSocket.ExecuteRequest"/>
        public override byte[] ExecuteRequest
            (
                byte[][] request
            )
        {
            Code.NotNull(request, "request");

            _result = null;
            _Execute(request);

            return _result;
        }

        #endregion
    }
}

#endif
