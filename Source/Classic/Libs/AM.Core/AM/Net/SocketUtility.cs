// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SocketUtility.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Net
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class SocketUtility
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Resolve IPv4 address
        /// </summary>
        /// <returns>Resolved IP address of the host.</returns>
        [NotNull]
        public static IPAddress ResolveAddressIPv4
            (
                [NotNull] string address
            )
        {
            Code.NotNull(address, "address");

            if (address.OneOf("localhost", "local", "(local)"))
            {
                return IPAddress.Loopback;
            }

            IPAddress result = null;

            try
            {
                result = IPAddress.Parse(address);
                if (result.AddressFamily
                    == AddressFamily.InterNetwork)
                {
                    throw new Exception("Address must be IPv4");
                }
            }
            catch 
            {
                IPHostEntry entry;

#if NETCORE || UAP

                entry = Dns.GetHostEntryAsync(address).Result;

#else

                entry = Dns.GetHostEntry(address);

#endif

                if (!ReferenceEquals(entry, null)
                    && !ReferenceEquals(entry.AddressList, null)
                    && entry.AddressList.Length != 0)
                {
                    IPAddress[] addresses = entry.AddressList
                        .Where
                        (
                            item => item.AddressFamily
                                    == AddressFamily.InterNetwork
                        )
                        .ToArray();

                    if (addresses.Length == 0)
                    {
                        throw new Exception("Address must be IPv4 only");
                    }

                    result = addresses.Length == 1
                        ? addresses[0]
                        : addresses[new Random().Next(addresses.Length)];
                }
            }

            if (ReferenceEquals(result, null))
            {
                throw new ArsMagnaException("Can't resolve address");
            }

            return result;
        }

        /// <summary>
        /// Resolve IPv6 address
        /// </summary>
        /// <returns>Resolved IP address of the host.</returns>
        [NotNull]
        public static IPAddress ResolveAddressIPv6
            (
                [NotNull] string address
            )
        {
            Code.NotNull(address, "address");

            if (address.OneOf("localhost", "local", "(local)"))
            {
                return IPAddress.IPv6Loopback;
            }

            IPAddress result = null;

            try
            {
                result = IPAddress.Parse(address);
                if (result.AddressFamily
                    == AddressFamily.InterNetworkV6)
                {
                    throw new Exception("Address must be IPv6");
                }
            }
            catch
            {
                IPHostEntry entry;

#if NETCORE || UAP

                entry = Dns.GetHostEntryAsync(address).Result;

#else

                entry = Dns.GetHostEntry(address);

#endif

                if (!ReferenceEquals(entry, null)
                    && !ReferenceEquals(entry.AddressList, null)
                    && entry.AddressList.Length != 0)
                {
                    IPAddress[] addresses = entry.AddressList
                        .Where
                        (
                            item => item.AddressFamily
                                    == AddressFamily.InterNetworkV6
                        )
                        .ToArray();

                    if (addresses.Length == 0)
                    {
                        throw new Exception("Address must be IPv6 only");
                    }

                    result = addresses.Length == 1
                        ? addresses[0]
                        : addresses[new Random().Next(addresses.Length)];
                }
            }

            if (ReferenceEquals(result, null))
            {
                throw new ArsMagnaException("Can't resolve address");
            }

            return result;
        }

#if NOTDEF

        [NotNull]
        public static Task<Socket> AcceptAsync
            (
                [NotNull] this Socket socket
            )
        {
            var tcs = new TaskCompletionSource<Socket>(socket);

            socket.BeginAccept
                (
                    iar =>
                    {
                        var t = (TaskCompletionSource<Socket>) iar.AsyncState;
                        var s = (Socket) t.Task.AsyncState;
                        try
                        {
                            t.TrySetResult(s.EndAccept(iar));
                        }
                        catch (Exception ex)
                        {
                            t.TrySetException(ex);
                        }
                    },
                    tcs
                );

            return tcs.Task;
        }

        [NotNull]
        public static Task ConnectAsync
            (
                [NotNull] this Socket socket,
                [NotNull] IPAddress address,
                int port
            )
        {
            EndPoint endPoint = new IPEndPoint(address, port);
            return ConnectAsync(socket, endPoint);
        }

        [NotNull]
        public static Task ConnectAsync
            (
                [NotNull] this Socket socket,
                [NotNull] EndPoint endPoint
            )
        {
            var tcs = new TaskCompletionSource<bool>(socket);

            socket.BeginConnect
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
                    tcs
                );

            return tcs.Task;
        }

        [NotNull]
        public static Task DisconnectAsync
            (
                [NotNull] this Socket socket,
                bool reuseSocket
            )
        {
            var tcs = new TaskCompletionSource<bool>(socket);

            socket.BeginDisconnect
                (
                    reuseSocket,
                    iar =>
                    {
                        var t = (TaskCompletionSource<bool>) iar.AsyncState;
                        var s = (Socket) t.Task.AsyncState;
                        try
                        {
                            s.EndDisconnect(iar);
                            t.TrySetResult(true);
                        }
                        catch (Exception ex)
                        {
                            t.TrySetException(ex);
                        }
                    },
                    tcs
                );

            return tcs.Task;
        }

        [NotNull]
        public static Task DisconnectAsync
            (
                [NotNull] this Socket socket
            )
        {
            return DisconnectAsync(socket, false);
        }

        [NotNull]
        public static Task<int> ReceiveAsync
            (
                [NotNull] this Socket socket,
                [NotNull] byte[] buffer,
                int offset,
                int size,
                SocketFlags socketFlags
            )
        {
            var tcs = new TaskCompletionSource<int>(socket);

            socket.BeginReceive
                (
                    buffer,
                    offset,
                    size,
                    socketFlags,
                    iar =>
                    {
                        var t = (TaskCompletionSource<int>)iar.AsyncState;
                        var s = (Socket)t.Task.AsyncState;
                        try
                        {
                            t.TrySetResult(s.EndReceive(iar));
                        }
                        catch (Exception exc)
                        {
                            t.TrySetException(exc);
                        }
                    },
                    tcs
                );

            return tcs.Task;
        }

        [NotNull]
        public static Task<int> ReceiveAsync
            (
                [NotNull] this Socket socket,
                [NotNull] byte[] buffer
            )
        {
            return ReceiveAsync
                (
                    socket,
                    buffer,
                    0,
                    buffer.Length,
                    SocketFlags.None
                );
        }
            
        [NotNull]
        public static Task<int> SendAsync
            (
                [NotNull] this Socket socket,
                [NotNull] byte[] buffer,
                int offset,
                int size,
                SocketFlags socketFlags
            )
        {
            var tcs = new TaskCompletionSource<int>(socket);

            socket.BeginSend
                (
                    buffer,
                    offset,
                    size,
                    socketFlags,
                    iar =>
                    {
                        var t = (TaskCompletionSource<int>)iar.AsyncState;
                        var s = (Socket)t.Task.AsyncState;
                        try
                        {
                            t.TrySetResult(s.EndReceive(iar));
                        }
                        catch (Exception exc)
                        {
                            t.TrySetException(exc);
                        }
                    },
                    tcs
                );

            return tcs.Task;
        }

        [NotNull]
        public static Task<int> SendAsync
            (
                [NotNull] this Socket socket,
                [NotNull] byte[] buffer,
                int length
            )
        {
            return SendAsync
                (
                    socket,
                    buffer,
                    0,
                    length,
                    SocketFlags.None
                );
        }

        [NotNull]
        public static Task<int> SendAsync
            (
                [NotNull] this Socket socket,
                [NotNull] byte[] buffer
            )
        {
            return SendAsync
                (
                    socket,
                    buffer,
                    0,
                    buffer.Length,
                    SocketFlags.None
                );
        }

#endif

        /// <summary>
        /// Receive specified amount of data from the socket.
        /// </summary>
        [NotNull]
        public static byte[] ReceiveExact
            (
                [NotNull] this Socket socket,
                int dataLength
            )
        {
            Code.NotNull(socket, "socket");
            Code.Nonnegative(dataLength, "dataLength");

            using (MemoryStream result = new MemoryStream(dataLength))
            {
                byte[] buffer = new byte[32 * 1024];

                while (dataLength > 0)
                {
                    int readed = socket.Receive(buffer);

                    if (readed <= 0)
                    {
                        throw new ArsMagnaException
                        (
                            "Socket reading error"
                        );
                    }

                    result.Write(buffer, 0, readed);

                    dataLength -= readed;
                }

                return result.ToArray();
            }
        }

        /// <summary>
        /// Read from the socket as many data as possible.
        /// </summary>
        [NotNull]
        public static byte[] ReceiveToEnd
            (
                [NotNull] this Socket socket
            )
        {
            Code.NotNull(socket, "socket");

            using (MemoryStream result = new MemoryStream())
            {
                byte[] buffer = new byte[32 * 1024];

                while (true)
                {
                    int readed = socket.Receive(buffer);

                    if (readed < 0)
                    {
                        throw new ArsMagnaException
                        (
                            "Socket reading error"
                        );
                    }

                    if (readed == 0)
                    {
                        break;
                    }

                    result.Write(buffer, 0, readed);
                }

                return result.ToArray();
            }
        }

#endregion
    }

}
