// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HttpServerSocket.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Threading;

using AM;
using AM.Globalization;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Sockets
{
    /// <summary>
    /// Обслуживание клиентов, подключившихся посредством HTTP.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class HttpServerSocket
        : Tcp4Socket
    {
        #region Properties

        /// <summary>
        /// Два перевода строки. Означают начало данных.
        /// </summary>
        public static byte[] TwoNewLines = {0x0D, 0x0A, 0x0D, 0x0A};

        /// <summary>
        /// Строка "IRBIS_END_REQUEST". Означает конец данных.
        /// </summary>
        public static byte[] EndRequest = { 0x49, 0x52, 0x42, 0x49,
            0x53, 0x5F, 0x45, 0x4E, 0x44, 0x5F, 0x52, 0x45, 0x51,
            0x55, 0x45, 0x53, 0x54 };

        #endregion

        #region Construction

        /// <summary>
        /// Construction.
        /// </summary>
        public HttpServerSocket
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
            //
            // Типичный запрос
            //
            // POST /cgi HTTP/1.1
            // User-Agent: GPNTB/Irbis64
            // Host: 127.0.0.1:6666
            // Accept: *.*
            // Content-length: 66
            //
            // A
            // C
            // A
            // 678232
            // 1
            // password
            // login
            //
            //
            //
            // login
            // password
            // IRBIS_END_REQUEST
            //

            byte[] received = base.ReceiveAll().ToArray();
            int startData = ArrayUtility.IndexOf(received, TwoNewLines);
            int endData = ArrayUtility.IndexOf(received, EndRequest);
            if (startData < 0 || endData < 0)
            {
                throw new IrbisException();
            }

            startData += TwoNewLines.Length;
            int dataLength = endData - startData;
            if (received[endData - 2] == 0x0D && received[endData - 1] == 0x0A)
            {
                // Убираем перевод строки
                dataLength -= 2;
            }
            received = received.GetSpan(startData, dataLength);
            string prefixString = dataLength.ToInvariantString() + "\r\n";
            MemoryStream result = new MemoryStream(dataLength + prefixString.Length);
            byte[] prefix = IrbisEncoding.Ansi.GetBytes(prefixString);
            result.Write(prefix, 0, prefix.Length);
            result.Write(received, 0, received.Length);
            result.Position = 0;

            return result;
        }

        /// <inheritdoc cref="IrbisServerSocket.Send" />
        public override void Send
            (
                byte[][] data
            )
        {
            int dataLength = 0;
            foreach (byte[] bytes in data)
            {
                dataLength += bytes.Length;
            }

            CultureInfo culture = BuiltinCultures.AmericanEnglish;
            string httpHeaders = "HTTP/1.1 200 OK\r\n"
                + "Date: " + DateTime.Now.ToString("F", culture) + "\r\n"
                + "Connection: close\r\n"
                + "Server: IRBIS64\r\n"
                + "Content-Type: application/octet-stream\r\n"
                + "Content-Length: " + dataLength.ToInvariantString()
                + "\r\n\r\n";

            byte[][] newData = new byte[data.Length + 1][];
            newData[0] = IrbisEncoding.Ansi.GetBytes(httpHeaders);
            for (int i = 0; i < data.Length; i++)
            {
                newData[i + 1] = data[i];
            }

            base.Send(newData);
        }

        #endregion
    }
}
