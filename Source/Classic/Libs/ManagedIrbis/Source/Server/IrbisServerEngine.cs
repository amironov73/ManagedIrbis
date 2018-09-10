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
using AM.Logging;
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
    public sealed class IrbisServerEngine
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Contexts.
        /// </summary>
        [NotNull]
        public NonNullCollection<ServerContext> Contexts { get; private set; }

        /// <summary>
        /// System data directory path.
        /// </summary>
        [NotNull]
        public string DataPath { get; private set; }

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
        /// System root directory path.
        /// </summary>
        [NotNull]
        public string SystemPath { get; private set; }

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

        /// <summary>
        /// System work directory path.
        /// </summary>
        [NotNull]
        public string WorkDir { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisServerEngine
            (
                [NotNull] ServerIniFile iniFile
            )
        {
            Log.Trace("IrbisServerEngine::Constructor enter");

            Code.NotNull(iniFile, "iniFile");

            IniFile = iniFile;
            SystemPath = IniFile.SystemPath.ThrowIfNull("SystemPath");
            Log.Trace("SysPath=" + SystemPath);
            _VerifyDirReadable(SystemPath);
            DataPath = IniFile.DataPath.ThrowIfNull("DataPath");
            Log.Trace("DataPath=" + DataPath);
            _VerifyDirReadable(DataPath);
            WorkDir = IniFile.WorkDir.ThrowIfNull("WorkDir");
            _VerifyDirReadable(WorkDir);
            _VerifyDirWriteable(WorkDir);

            StopSignal = new ManualResetEvent(false);

            IPEndPoint endPoint = new IPEndPoint
                (
                    IPAddress.Any,
                    IniFile.IPPort
                );
            Listener = new TcpListener(endPoint);

            Contexts = new NonNullCollection<ServerContext>();
            Workers = new NonNullCollection<IrbisServerWorker>();

            Log.Trace("IrbisServerEngine::Constructor leave");
        }

        #endregion

        #region Private members

        private void _VerifyDirReadable
            (
                [NotNull] string path
            )
        {
            // TODO Implement
        }

        private void _VerifyDirWriteable
            (
                [NotNull] string path
            )
        {
            // TODO implement
        }

#if DESKTOP

        private void _HandleClient
            (
                IAsyncResult asyncResult
            )
        {
            Log.Trace("IrbisServerEngine::_HandleClient enter");

            TcpListener listener = (TcpListener) asyncResult.AsyncState;
            TcpClient client = listener.EndAcceptTcpClient(asyncResult);
            IrbisServerSocket socket = new IrbisServerSocket(client);
            IrbisServerWorker worker = new IrbisServerWorker(this, socket);
            Workers.Add(worker);
            worker.Task.Start();

            Log.Trace("IrbisServerEngine::_HandleClient leave");
        }

#elif NETCORE

        private void _HandleClient
            (
                Task<TcpClient> task
            )
        {
            TcpClient client = task.Result;
            IrbisServerSocket socket = new IrbisServerSocket(client);
            IrbisServerWorker worker = new IrbisServerWorker(this, socket);
            Workers.Add(worker);
            worker.Task.Start();
        }

#endif

        #endregion

        #region Public methods

        /// <summary>
        /// Process loop.
        /// </summary>
        public void MainLoop()
        {
            Log.Trace("IrbisServerEngine::MainLoop enter");

            Listener.Start();

            while (true)
            {
#if WINMOBILE || PocketPC

                if (StopSignal.WaitOne(0, false))
                {
                    break;
                }

#else

                if (StopSignal.WaitOne(0))
                {
                    Log.Trace("IrbisServerEngine::MainLoop: break signal 1");
                    break;
                }

#endif

#if DESKTOP

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
                    Log.Trace("IrbisServerEngine::MainLoop: break signal 2");
                    break;
                }

#elif NETCORE

                Task<TcpClient> task = Listener.AcceptTcpClientAsync();
                task.ContinueWith (_HandleClient);

#endif

            }

            Log.Trace("IrbisServerEngine::MainLoop leave");
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

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Listener.Stop(); // ???
        }

        #endregion

        #region Object members

        #endregion
    }
}
