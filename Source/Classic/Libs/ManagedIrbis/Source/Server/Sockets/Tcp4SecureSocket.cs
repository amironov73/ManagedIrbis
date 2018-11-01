// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Tcp4SecureSocket.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !UAP

#region Using directives

using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

using AM.Security;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Sockets
{
    /// <summary>
    /// Подключение через TCP/IP v4 с поддержкой SSL/TLS.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class Tcp4SecureSocket
        : Tcp4Socket
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Tcp4SecureSocket
            (
                [NotNull] TcpClient client,
                CancellationToken token
            )
            : base(client, token)
        {
            _certificate = SecurityUtility.GetSslCertificate();
            _sslStream = new SslStream(client.GetStream(), false);
            _sslStream.AuthenticateAsServer(_certificate);
        }

        #endregion

        #region Private member

        private X509Certificate _certificate;

        private SslStream _sslStream;

        private bool _ValidateServerCertificate
            (
                object sender,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors
            )
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            // Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            return false;
        }

        #endregion

        #region IrbisServerSocket members

        /// <inheritdoc cref="IrbisServerSocket.ReceiveAll" />
        public override MemoryStream ReceiveAll()
        {
            MemoryStream result = new MemoryStream();

            while (true)
            {
                byte[] buffer = new byte[50 * 1024];
                int read = _sslStream.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                {
                    break;
                }
                result.Write(buffer, 0, read);
            }

            result.Position = 0;

            return result;
        }

        /// <inheritdoc cref="IrbisServerSocket.Send" />
        public override void Send
            (
                byte[][] data
            )
        {
            foreach (byte[] bytes in data)
            {
                _sslStream.Write(bytes);
            }
        }

        /// <inheritdoc cref="IrbisServerSocket.Dispose" />
        public override void Dispose()
        {
            _sslStream.Dispose();
            base.Dispose();
        }

        #endregion
    }
}

#endif
