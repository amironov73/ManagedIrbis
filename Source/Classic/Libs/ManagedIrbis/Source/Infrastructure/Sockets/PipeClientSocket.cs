// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PipeClientSocket.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !UAP

#region Using directives

using System;
using System.IO;
using System.IO.Pipes;

using AM.Threading;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Sockets
{
    /// <summary>
    /// Client using System.IO.Pipes.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PipeClientSocket
        : AbstractClientSocket
    {
        #region Construction

        /// <summary>
        /// Server name.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Pipe name.
        /// </summary>
        public string PipeName { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PipeClientSocket
            (
                [NotNull] IIrbisConnection connection,
                string serverName,
                string pipeName
            )
            : base(connection)
        {
            ServerName = serverName;
            PipeName = pipeName;
        }

        #endregion

        #region AbstractClientSocket members

        /// <inheritdoc cref="AbstractClientSocket.AbortRequest" />
        public override void AbortRequest()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="AbstractClientSocket.ExecuteRequest" />
        public override byte[] ExecuteRequest
            (
                byte[][] request
            )
        {
            using (new BusyGuard(Busy))
            using (NamedPipeClientStream client = new NamedPipeClientStream
                (
                    ServerName,
                    PipeName,
                    PipeDirection.InOut
                ))
            {
                client.Connect();

                foreach (byte[] buffer in request)
                {
                    client.Write(buffer, 0, buffer.Length);
                }

                MemoryStream memory = Connection.Executive
                    .GetMemoryStream(GetType());
                while (true)
                {
                    byte[] buffer = new byte[50 * 1024];
                    int read = client.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                    {
                        break;
                    }
                    memory.Write(buffer, 0, read);
                }

                byte[] result = memory.ToArray();
                Connection.Executive.ReportMemoryUsage(GetType(), result.Length);

                return result;
            }
        }

        #endregion
    }
}

#endif
