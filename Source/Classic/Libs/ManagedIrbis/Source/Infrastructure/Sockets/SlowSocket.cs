// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SlowSocket.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SlowSocket
        : AbstractClientSocket
    {
        #region Constants

        /// <summary>
        /// Default value for <see cref="Delay"/>.
        /// </summary>
        public const int DefaultDelay = 300;

        #endregion

        #region Properties

        /// <summary>
        /// Delay, milliseconds.
        /// </summary>
        public int Delay { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SlowSocket
            (
                [NotNull] IrbisConnection connection,
                [NotNull] AbstractClientSocket innerSocket
            )
            : base(connection)
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(innerSocket, "innerSocket");

            Delay = DefaultDelay;
            InnerSocket = innerSocket;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region AbstractClientSocket members

        /// <inheritdoc cref="AbstractClientSocket.AbortRequest" />
        public override void AbortRequest()
        {
            InnerSocket.ThrowIfNull().AbortRequest();
        }

        /// <inheritdoc cref="AbstractClientSocket.ExecuteRequest" />
        public override byte[] ExecuteRequest
            (
                byte[] request
            )
        {
            Code.NotNull(request, "request");

            int delay = Delay;
            if (delay > 0)
            {
                ThreadUtility.Sleep(delay);
                //Thread.Sleep(delay);
            }

            byte[] result = InnerSocket.ThrowIfNull().ExecuteRequest(request);

            return result;
        }

        #endregion
    }
}
