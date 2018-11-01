// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Tcp4CompressSocket.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Net.Sockets;
using System.Threading;

using AM;
using AM.IO;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Sockets
{
    /// <summary>
    /// Сжимающее данные подключение через TCP/IP v4.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class Tcp4CompressSocket
        : Tcp4Socket
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Tcp4CompressSocket
            (
                [NotNull] TcpClient client,
                CancellationToken token
            )
            : base(client, token)
        {
        }

        #endregion

        #region IrbisServerSocket members

        /// <inheritdoc cref="IrbisServerSocket.ReceiveAll" />
        public override MemoryStream ReceiveAll()
        {
            byte[] compressed = base.ReceiveAll().ToArray();
            byte[] decomressed = CompressionUtility.Decompress(compressed);
            MemoryStream result = new MemoryStream();
            result.Write(decomressed, 0, decomressed.Length);
            result.Position = 0;

            return result;
        }

        /// <inheritdoc cref="IrbisServerSocket.Send" />
        public override void Send
            (
                byte[][] data
            )
        {
            byte[][] compressed = new byte[1][];
            byte[] merged = ArrayUtility.Merge(data);
            compressed[0] = CompressionUtility.Compress(merged);
            base.Send(compressed);
        }

        #endregion
    }
}