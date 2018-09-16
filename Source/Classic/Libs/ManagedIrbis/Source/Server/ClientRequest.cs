﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ClientRequest.cs --
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
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

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

            TcpClient connection = data.Socket.Client;
            Memory = new MemoryStream();
            NetworkStream stream = connection.GetStream();
            while (true)
            {
                byte[] buffer = new byte[50 * 1024];
                int read = stream.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                {
                    break;
                }
                Memory.Write(buffer, 0, read);
            }

            Memory.Position = 0;
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
        public string GetAnsiString()
        {
            byte[] bytes = GetString();
            return IrbisEncoding.Ansi.GetString(bytes);
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
        [CanBeNull]
        public string GetUtfString()
        {
            byte[] bytes = GetString();
            return IrbisEncoding.Utf8.GetString(bytes);
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
        public int GetInt32()
        {
            byte[] line = GetString();
            int result = FastNumber.ParseInt32(line, 0, line.Length);

            return result;
        }

        #endregion
    }
}
