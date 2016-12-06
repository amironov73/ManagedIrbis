// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisSocketServer.cs -- 
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
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisSocketServer
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Ini file.
        /// </summary>
        [NotNull]
        public ServerIniFile IniFile { get; private set; }

        /// <summary>
        /// TCP listener.
        /// </summary>
        [NotNull]
        public TcpListener Listener { get; private set; }

        /// <summary>
        /// Stop signal.
        /// </summary>
        [NotNull]
        public ManualResetEvent StopSignal { get; private set; }

        /// <summary>
        /// Workers.
        /// </summary>
        [NotNull]
        public NonNullCollection<IrbisServerWorker> Workers { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisSocketServer
            (
                [NotNull] ServerIniFile iniFile
            )
        {
            Code.NotNull(iniFile, "iniFile");

            IniFile = iniFile;

            StopSignal = new ManualResetEvent(false);

            IPEndPoint endPoint = new IPEndPoint
                (
                    IPAddress.Any,
                    IniFile.IPPort
                );
            Listener = new TcpListener(endPoint);

            Workers = new NonNullCollection<IrbisServerWorker>();
        }

        #endregion

        #region Private members

        private void _HandleClient
            (
                IAsyncResult asyncResult
            )
        {
            TcpListener listener = (TcpListener) asyncResult.AsyncState;
            TcpClient client = listener.EndAcceptTcpClient(asyncResult);
            IrbisServerSocket socket = new IrbisServerSocket(client);
            IrbisServerWorker worker = new IrbisServerWorker(this, socket);
            Workers.Add(worker);
            worker.Task.Start();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Process loop.
        /// </summary>
        public void ProcessLoop()
        {
            Listener.Start();

            while (true)
            {
                if (StopSignal.WaitOne(0))
                {
                    break;
                }

                IAsyncResult socketResult = Listener.BeginAcceptTcpClient
                    (
                        _HandleClient,
                        Listener
                    );

                WaitHandle[] handles =
                {
                    socketResult.AsyncWaitHandle,
                    StopSignal
                };
                int index = WaitHandle.WaitAny(handles);
                if (index == 1
                    || index < 0)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Wait for workers (if any).
        /// </summary>
        public void WaitForWorkers()
        {
            Task[] tasks = Workers
                .Select(worker => worker.Task)
                .ToArray();

            if (tasks.Length != 0)
            {
                Task.WaitAll(tasks);
            }
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc/>
        public void Dispose()
        {
            Listener.Stop(); // ???
        }

        #endregion

        #region Object members

        #endregion
    }
}
