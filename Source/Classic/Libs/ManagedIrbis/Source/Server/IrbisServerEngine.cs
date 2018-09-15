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
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Server.Commands;

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
        /// Object for synchronization.
        /// </summary>
        [NotNull]
        public object SyncRoot { get; private set; }

        /// <summary>
        /// Contexts.
        /// </summary>
        [NotNull]
        public NonNullCollection<ServerContext> Contexts { get; private set; }

        /// <summary>
        /// MNU file with standard _server_ INI file names.
        /// </summary>
        [NotNull]
        public MenuFile ClientIni { get; private set; }

        /// <summary>
        /// System data directory path.
        /// </summary>
        [NotNull]
        public string DataPath { get; private set; }

        /// <summary>
        /// Path for Deposit directory.
        /// </summary>
        [NotNull]
        public string DepositPath { get; private set; }

        /// <summary>
        /// Path for Deposit_USER directory.
        /// </summary>
        [NotNull]
        public string DepositUserPath { get; private set; }

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
        /// Command mapper.
        /// </summary>
        [NotNull]
        public CommandMapper Mapper { get; private set; }

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
        /// Known users.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public UserInfo[] Users { get; private set; }

        /// <summary>
        /// Workers.
        /// </summary>
        [NotNull]
        public NonNullCollection<ServerWorker> Workers { get; private set; }

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
            : this(iniFile, null)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisServerEngine
            (
                [NotNull] ServerIniFile iniFile,
                [CanBeNull] string rootPathOverride
            )
        {
            Log.Trace("IrbisServerEngine::Constructor enter");

            Code.NotNull(iniFile, "iniFile");

            SyncRoot = new object();
            IniFile = iniFile;
            SystemPath = rootPathOverride
                         ?? IniFile.SystemPath.ThrowIfNull("SystemPath");
            Log.Trace("SysPath=" + SystemPath);
            _VerifyDirReadable(SystemPath);
            DataPath = ReferenceEquals(rootPathOverride, null)
                ? IniFile.DataPath.ThrowIfNull("DataPath")
                : Path.Combine(rootPathOverride, "Datai");
            Log.Trace("DataPath=" + DataPath);
            _VerifyDirReadable(DataPath);
            DepositPath = Path.Combine(DataPath, "Deposit");
            DepositUserPath = Path.Combine(DataPath, "Deposit_USER");
            WorkDir = ReferenceEquals(rootPathOverride, null)
                ? IniFile.WorkDir.ThrowIfNull("WorkDir")
                : Path.Combine(rootPathOverride, "workdir");
            if (!Directory.Exists(WorkDir))
            {
                Directory.CreateDirectory(WorkDir);
            }
            _VerifyDirReadable(WorkDir);
            _VerifyDirWriteable(WorkDir);

            string fileName = Path.Combine(SystemPath, "client_ini.mnu");
            ClientIni = MenuFile.ParseLocalFile(fileName, IrbisEncoding.Ansi);
            string clientList = IniFile.ClientList ?? "client_m.mnu";
            clientList = Path.Combine(DataPath, clientList);
            Users = UserInfo.ParseFile(clientList, ClientIni);

            StopSignal = new ManualResetEvent(false);

            IPEndPoint endPoint = new IPEndPoint
                (
                    IPAddress.Any,
                    IniFile.IPPort
                );
            Listener = new TcpListener(endPoint);

            Contexts = new NonNullCollection<ServerContext>();
            Workers = new NonNullCollection<ServerWorker>();
            Mapper = new CommandMapper(this);

            Log.Trace("IrbisServerEngine::Constructor leave");
        }

        #endregion

        #region Private members

        private string _GetDepositFile(string fileName)
        {
            string result = Path.GetFullPath(Path.Combine(DepositPath, fileName));
            if (!File.Exists(result))
            {
                result = null;
            }

            return result;
        }

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

        private void _HandleClient
            (
#if DESKTOP
                IAsyncResult asyncResult
#elif NETCORE || ANDROID || UAP
                Task<TcpClient> task
#endif
            )
        {
            Log.Trace("IrbisServerEngine::_HandleClient enter");

#if DESKTOP

            TcpListener listener = (TcpListener)asyncResult.AsyncState;
            TcpClient client = listener.EndAcceptTcpClient(asyncResult);

#elif NETCORE || ANDROID || UAP

            TcpClient client = task.Result;

#endif
            WorkData data = new WorkData
            {
                Engine = this,
                Socket = new IrbisServerSocket(client),
            };

            ServerWorker worker = new ServerWorker(data);
            data.Worker = worker;

            lock (SyncRoot)
            {
                Workers.Add(worker);
            }

            data.Task.Start();

            Log.Trace("IrbisServerEngine::_HandleClient leave");
        }

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

#elif NETCORE || ANDROID || UAP

                Task<TcpClient> task = Listener.AcceptTcpClientAsync();
                task.ContinueWith (_HandleClient);

#endif

            }

            Log.Trace("IrbisServerEngine::MainLoop leave");
        }

        /// <summary>
        /// Resolve the file path.
        /// </summary>
        [CanBeNull]
        public string ResolveFile
            (
                [NotNull] FileSpecification specification
            )
        {
            Code.NotNull(specification, "specification");

            string fileName = specification.FileName;
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            string result;
            string database = specification.Database;
            int path = (int)specification.Path;
            if (path == 0)
            {
                result = Path.Combine(SystemPath, fileName);
            }
            else if (path == 1)
            {
                result = Path.Combine(DataPath, fileName);
            }
            else
            {
                result = Path.GetFullPath(Path.Combine(DepositUserPath, fileName));
                if (File.Exists(result))
                {
                    return result;
                }

                if (string.IsNullOrEmpty(database))
                {
                    return _GetDepositFile(fileName);
                }

                string parPath = Path.Combine(DataPath, database + ".par");
                if (!File.Exists(parPath))
                {
                    result = _GetDepositFile(fileName);
                }
                else
                {
                    Dictionary<int, string> dictionary;
                    using (StreamReader reader
                        = TextReaderUtility.OpenRead(parPath, IrbisEncoding.Ansi))
                    {
                        dictionary = ParFile.ReadDictionary(reader);
                    }

                    if (!dictionary.ContainsKey(path))
                    {
                        result = _GetDepositFile(fileName);
                    }
                    else
                    {
                        result = Path.GetFullPath(Path.Combine
                            (
                                Path.Combine(DataPath, dictionary[path]),
                                fileName
                            ));
                        if (!File.Exists(result))
                        {
                            result = _GetDepositFile(fileName);
                        }
                    }
                }
            }

            return result;

        }

        /// <summary>
        /// Wait for workers (if any).
        /// </summary>
        public void WaitForWorkers()
        {
            Task[] tasks = Workers
                .Select(worker => worker.Data.Task)
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
