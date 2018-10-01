// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ClientRequest.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Server.Sockets;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server
{
    /// <summary>
    /// Client request.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ClientRequest
    {
        #region Properties

        /// <summary>
        /// Общая длина запроса в байтах.
        /// </summary>
        public int RequestLength { get; set; }

        /// <summary>
        /// Код команды (первая копия).
        /// </summary>
        public string CommandCode1 { get; set; }

        /// <summary>
        /// Код АРМ.
        /// </summary>
        public string Workstation { get; set; }

        /// <summary>
        /// Код команды (вторая копия).
        /// </summary>
        public string CommandCode2 { get; set; }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Номер команды.
        /// </summary>
        public string CommandNumber { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пакет с клиентским запросом.
        /// </summary>
        [NotNull]
        public MemoryStream Memory { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor for mocking.
        /// </summary>
        public ClientRequest()
        {
            // To make Resharper happy
            Memory = new MemoryStream();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ClientRequest
            (
                [NotNull] WorkData data
            )
        {
            Code.NotNull(data, "data");

            IrbisServerSocket socket = data.Socket;
            Memory = socket.ReceiveAll();
            RequestLength = GetInt32();
            CommandCode1 = RequireAnsiString();
            Workstation = RequireAnsiString();
            CommandCode2 = RequireAnsiString();
            ClientId = RequireAnsiString();
            CommandNumber = RequireAnsiString();
            Password = GetAnsiString();
            Login = GetAnsiString();
            GetAnsiString();
            GetAnsiString();
            GetAnsiString();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get string without encoding.
        /// </summary>
        [NotNull]
        public byte[] GetString()
        {
            MemoryStream result = new MemoryStream();

            while (true)
            {
                int next = Memory.ReadByte();
                if (next < 0 || next == 0x0A)
                {
                    break;
                }
                result.WriteByte((byte)next);
            }

            return result.ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        [CanBeNull]
        public string GetAutoString()
        {
            byte[] bytes = GetString();
            int index = 0, count = bytes.Length;
            Encoding encoding = IrbisEncoding.Ansi;

            if (count != 0)
            {
                if (bytes[0] == (byte) '!')
                {
                    encoding = IrbisEncoding.Utf8;
                    index = 1;
                    count--;
                }
            }

            return encoding.GetString(bytes, index, count);
        }

        /// <summary>
        ///
        /// </summary>
        [NotNull]
        public string RequireAutoString()
        {
            string result = GetAutoString();
            if (string.IsNullOrEmpty(result))
            {
                throw new IrbisException();
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        [CanBeNull]
        public string GetAnsiString()
        {
            byte[] bytes = GetString();
            return EncodingUtility.GetString(IrbisEncoding.Ansi, bytes);
        }

        /// <summary>
        ///
        /// </summary>
        [NotNull]
        public string RequireAnsiString()
        {
            string result = GetAnsiString();
            if (string.IsNullOrEmpty(result))
            {
                throw new IrbisException();
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        [NotNull]
        public string[] RemainingAnsiStrings()
        {
            List<string> result = new List<string>();

            while (Memory.Position < Memory.Length)
            {
                string line = GetAnsiString();
                result.Add(line);
            }

            return result.ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        [NotNull]
        public string RemainingAnsiText()
        {
            int remaining = (int) (Memory.Length - Memory.Position);
            byte[] bytes = new byte[remaining];
            Memory.Read(bytes, 0, remaining);

            return EncodingUtility.GetString(IrbisEncoding.Ansi, bytes);
        }

        /// <summary>
        ///
        /// </summary>
        [CanBeNull]
        public string GetUtfString()
        {
            byte[] bytes = GetString();
            return EncodingUtility.GetString(IrbisEncoding.Utf8, bytes);
        }

        /// <summary>
        ///
        /// </summary>
        [NotNull]
        public string RequireUtfString()
        {
            string result = GetUtfString();
            if (string.IsNullOrEmpty(result))
            {
                throw new IrbisException();
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        [NotNull]
        public string[] RemainingUtfStrings()
        {
            List<string> result = new List<string>();

            while (Memory.Position < Memory.Length)
            {
                string line = GetUtfString();
                result.Add(line);
            }

            return result.ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        [NotNull]
        public string RemainingUtfText()
        {
            int remaining = (int) (Memory.Length - Memory.Position);
            byte[] bytes = new byte[remaining];
            Memory.Read(bytes, 0, remaining);

            return EncodingUtility.GetString(IrbisEncoding.Utf8, bytes);
        }

        /// <summary>
        ///
        /// </summary>
        public int GetInt32()
        {
            byte[] line = GetString();
            int result = FastNumber.ParseInt32(line, 0, line.Length);

            return result;
        }

        #endregion
    }
}
