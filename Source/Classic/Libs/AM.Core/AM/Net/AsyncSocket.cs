﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AsyncSocket.cs -- 
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
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Net
{
#if NOTDEF

    /// <summary>
    /// Thin <see cref="Task"/>-oriented wrapper over
    /// <see cref="Socket"/> class.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class AsyncSocket
    {
        #region Properties

        /// <summary>
        /// Underlying socket.
        /// </summary>
        [NotNull]
        public Socket BaseSocket { get { return _socket; } }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public AsyncSocket ()
            : this 
                (
                    new Socket
                        (
                            AddressFamily.InterNetwork,
                            SocketType.Stream,
                            ProtocolType.Tcp
                        )
                )
        {
            
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public AsyncSocket
            (
                [NotNull] Socket socket
            )
        {
            Code.NotNull(socket, "socket");

            _socket = socket;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public AsyncSocket
            (
                AddressFamily addressFamily,
                SocketType socketType,
                ProtocolType protocolType
            )
            : this 
                (
                    new Socket
                        (
                            addressFamily,
                            socketType,
                            protocolType
                        )
                )
        {
            
        }

        #endregion

        #region Private members

        private readonly Socket _socket;

        #endregion

        #region Public methods

        public AsyncSocketResult ConnectAsync
            (
                [NotNull] EndPoint endPoint
            )
        {
            TaskCompletionSource<bool> taskCompletionSource
                = new TaskCompletionSource<bool>(this);
            
            BaseSocket.BeginConnect
                (
                    endPoint,
                    iar =>
                    {
                        var t = (TaskCompletionSource<bool>)iar.AsyncState;
                        var s = (Socket)t.Task.AsyncState;
                        try
                        {
                            s.EndConnect(iar);
                            t.TrySetResult(true);
                        }
                        catch (Exception ex)
                        {
                            t.TrySetException(ex);
                        }

                    },
                    taskCompletionSource
                );

            AsyncSocketResult result = new AsyncSocketResult
                (
                    () => taskCompletionSource.Task.Wait()
                );
            return result;
        }


        #endregion

        #region Object members

        #endregion

    }

#endif
}
