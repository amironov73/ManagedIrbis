// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ClientSocketUtility.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

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
    public static class ClientSocketUtility
    {
        #region Public methods

        /// <summary>
        /// Create socket of the given type
        /// and add it to the socket chain
        /// of the connection.
        /// </summary>
        [NotNull]
        public static AbstractClientSocket CreateSocket
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string typeName
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(typeName, "typeName");

#if WINMOBILE || PocketPC

            throw new NotImplementedException();
#else

            Type type = Type.GetType(typeName, true);
            AbstractClientSocket result
                = (AbstractClientSocket)Activator.CreateInstance
                (
                    type,
                    connection
                );
            connection.SetSocket(result);

            return result;
#endif
        }

        /// <summary>
        /// Create socket of the given type
        /// and add it to the socket chain
        /// of the connection.
        /// </summary>
        [NotNull]
        public static T CreateSocket<T>
            (
                [NotNull] IrbisConnection connection
            )
            where T: AbstractClientSocket
        {
            Code.NotNull(connection, "connection");

#if WINMOBILE || PocketPC

            throw new NotImplementedException();
#else

            Type type = typeof(T);
            T result
                = (T)Activator.CreateInstance
                (
                    type,
                    connection
                );
            connection.SetSocket(result);

            return result;
#endif
        }

        /// <summary>
        /// Find given socket type in the socket chain.
        /// </summary>
        [CanBeNull]
        public static T FindSocket<T>
            (
                [NotNull] IrbisConnection connection
            )
            where T: AbstractClientSocket
        {
            Code.NotNull(connection, "connection");

            T result = null;
            for (
                    AbstractClientSocket socket = connection.Socket;
                    !ReferenceEquals(socket, null);
                    socket = socket.InnerSocket
                )
            {
                T temp = socket as T;
                if (!ReferenceEquals(temp, null))
                {
                    result = temp;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Find or create (add to socket chain)
        /// given socket type.
        /// </summary>
        [NotNull]
        public static T FindOrCreateSocket<T>
            (
                [NotNull] IrbisConnection connection
            )
            where T: AbstractClientSocket
        {
            Code.NotNull(connection, "connection");

            T result = FindSocket<T>(connection) 
                ?? CreateSocket<T>(connection);

            return result;
        }

        /// <summary>
        /// Remove given socket from the socket chain.
        /// </summary>
        public static void RemoveSocket
            (
                [NotNull] IrbisConnection connection,
                [NotNull] AbstractClientSocket socket
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(socket, "socket");

            AbstractClientSocket inner = socket.InnerSocket;

            if (ReferenceEquals(connection.Socket, socket))
            {
                inner = inner.ThrowIfNull("socket.InnerSocket");
                connection.SetSocket(inner);
            }
            else
            {
                for (
                        AbstractClientSocket current = connection.Socket;
                        !ReferenceEquals(current, null);
                    )
                {
                    inner = current.InnerSocket;

                    if (ReferenceEquals(inner, socket))
                    {
                        current.InnerSocket = inner.InnerSocket;
                        break;
                    }

                    current = inner;
                }
            }
        }

        #endregion
    }
}
