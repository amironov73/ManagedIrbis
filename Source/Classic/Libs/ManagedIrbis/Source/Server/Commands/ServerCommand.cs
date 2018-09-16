﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ServerCommand.cs --
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
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class ServerCommand
    {
        #region Properties

        /// <summary>
        /// Data.
        /// </summary>
        [NotNull]
        public WorkData Data { get; private set; }

        /// <summary>
        /// Send version to the client.
        /// </summary>
        public virtual bool SendVersion
        {
            get { return false; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ServerCommand
            (
                [NotNull] WorkData data
            )
        {
            Code.NotNull(data, "data");

            Data = data;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Отправка клиенту кода ошибки.
        /// </summary>
        public void SendError
            (
                int errorCode
            )
        {
            if (errorCode >= 0)
            {
                errorCode = -8888;
            }

            // TODO implement
        }

        /// <summary>
        /// Отправка клиенту нормального ответа.
        /// </summary>
        public void SendResponse()
        {
            ServerResponse response = Data.Response.ThrowIfNull();
            string versionString = null;
            if (SendVersion)
            {
                IrbisVersion serverVersion = ServerUtility.GetServerVersion();
                versionString = serverVersion.Version;
            }
            byte[][] packet = response.Encode(versionString);
            TcpClient client = Data.Socket.Client.ThrowIfNull();
            Socket socket = client.Client;
            foreach (byte[] bytes in packet)
            {
                socket.Send(bytes);
            }
        }

        /// <summary>
        /// Update the context.
        /// </summary>
        public void UpdateContext()
        {
            ServerContext context = Data.Context.ThrowIfNull();
            context.LastActivity = DateTime.Now;
            context.LastCommand = Data.Request.CommandCode1;
            context.CommandCount++;
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        public abstract void Execute();

        #endregion

        #region Object members

        #endregion
    }
}
