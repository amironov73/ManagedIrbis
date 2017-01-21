// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SmartClientSocket.cs -- minimizes memory reallocation
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
using System.Net;

#if !WIN81

using System.Net.Sockets;

#endif

using System.Text;
using System.Threading.Tasks;
using AM;
using AM.IO;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Sockets
{
    /// <summary>
    /// Client socket that minimizes memory reallocation.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SmartClientSocket
        : AbstractClientSocket
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmartClientSocket
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region Private members

#if !WIN81

        private IPAddress _address;

        private void _ResolveHostAddress
            (
                string host
            )
        {
            Code.NotNullNorEmpty(host, "host");

            if (_address == null)
            {
                //try
                //{
                _address = IPAddress.Parse(Connection.Host);
                //}
                //catch
                //{
                //    // Not supported in .NET Core
                //    IPHostEntry ipHostEntry = Dns.GetHostEntry(Connection.Host);
                //    _address = ipHostEntry.AddressList[0];
                //}
            }

            if (_address == null)
            {
                throw new IrbisNetworkException
                    (
                        "Can't resolve host " + host
                    );
            }
        }

#if !SILVERLIGHT && !UAP

        private TcpClient _GetTcpClient()
        {
            TcpClient result = new TcpClient();

            // TODO some setup

#if FW35

            // Not supported in .NET Core
            result.Connect(_address, Connection.Port);

#endif

#if NETCORE

            Task task = result.ConnectAsync(_address, Connection.Port);
            task.Wait();

#else

            // FW40 doesn't contain ConnectAsync
            result.Connect(_address, Connection.Port);

#endif

            return result;
        }

#endif

#endif

        private static byte[] _SmartRead
            (
                [NotNull] NetworkStream stream
            )
        {
            byte[] head = new byte[10 * 1024];
            byte[] body, result;

            int readed1 = stream.Read(head, 0, head.Length);
            if (readed1 == 0)
            {
                throw new IrbisNetworkException("Empty response");
            }

            // Ожидаемый ответ сервера:
            //
            // Команда
            // Идентификатор клиента
            // Порядковый номер
            // Длина ответа
            // Прочие данные

            ByteNavigator navigator = new ByteNavigator(head);
            navigator.SkipLine();
            navigator.SkipLine();
            navigator.SkipLine();

            string text = navigator.ReadLine();
            if (ReferenceEquals(text, null))
            {
                return head;
            }

            int length;
            if (!int.TryParse(text, out length))
            {
                if (readed1 < head.Length)
                {
                    return head.GetSpan(0, readed1);
                }
                body = stream.ReadToEnd();

                result = ArrayUtility.Merge(head, body);

                return result;
            }

            int remaining = length + text.Length - readed1;
            if (remaining <= 0)
            {
                return head.GetSpan(0, readed1);
            }

            body = new byte[remaining];
            int readed2 = stream.Read(body, 0, remaining);
            if (readed2 != remaining)
            {
                throw new IrbisNetworkException();
            }

            result = ArrayUtility.Merge(head, body);

            return result;
        }

        #endregion

        #region Public methods

        #endregion

        #region AbstractClientSocket members

        /// <summary>
        /// Abort the request.
        /// </summary>
        public override void AbortRequest()
        {
            // TODO implement
        }

        /// <summary>
        /// Send request to server and receive answer.
        /// </summary>
        public override byte[] ExecuteRequest
            (
                byte[] request
            )
        {
            Code.NotNull(request, "request");

#if SILVERLIGHT

            throw new NotImplementedException();

#elif UAP

            throw new NotImplementedException();

#elif WIN81

            throw new NotImplementedException();

#else

            _ResolveHostAddress(Connection.Host);

            using (new BusyGuard(Busy))
            {
                using (TcpClient client = _GetTcpClient())
                {
                    NetworkStream stream = client.GetStream();

                    stream.Write
                        (
                            request,
                            0,
                            request.Length
                        );

                    byte[] result = stream.ReadToEnd();

                    return result;
                }
            }

#endif
        }

        #endregion
    }
}
