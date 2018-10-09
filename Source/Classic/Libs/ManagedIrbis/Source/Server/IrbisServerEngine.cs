// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisServerEngine.cs --
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
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Threading;
using AM.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Direct;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Server.Commands;
using ManagedIrbis.Server.Sockets;

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
        /// Момент старта движка.
        /// </summary>
        public DateTime StartedAt { get; private set; }

        /// <summary>
        /// Whether the engine paused client request processing.
        /// </summary>
        public bool Paused { get; set; }

        /// <summary>
        /// Contexts.
        /// </summary>
        [NotNull]
        public NonNullCollection<ServerContext> Contexts { get; private set; }

        /// <summary>
        /// Cache.
        /// </summary>
        [NotNull]
        public ServerCache Cache { get; private set; }

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
        public IrbisServerListener[] Listeners { get; private set; }

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

        /// <summary>
        /// IP port number.
        /// </summary>
        public int PortNumber { get; private set; }

        /// <summary>
        /// Ban list.
        /// </summary>
        public BanMaster BanList { get; private set; }

        /// <summary>
        /// Delayed update task.
        /// </summary>
        [CanBeNull]
        public Task DelayedUpdater { get; private set; }

        /// <summary>
        /// Watchdog.
        /// </summary>
        [CanBeNull]
        public ServerWatchdog Watchdog { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public IrbisServerEngine
            (
                [NotNull] ServerSetup setup
            )
        {
            Log.Trace("IrbisServerEngine::Constructor enter");

            Code.NotNull(setup, "setup");

            if (setup.Break)
            {
                Debugger.Launch();
            }

            _cancellation = new CancellationTokenSource();

            SyncRoot = new object();
            Cache = new ServerCache();
            IniFile = setup.IniFile;
            string rootPathOverride = setup.RootPathOverride;
            SystemPath = rootPathOverride
                         ?? IniFile.SystemPath.ThrowIfNull("SystemPath");
            Log.Trace("SysPath=" + SystemPath);
            _VerifyDirectoryReadable(SystemPath);
            DataPath = ReferenceEquals(rootPathOverride, null)
                ? IniFile.DataPath.ThrowIfNull("DataPath")
                : Path.Combine(rootPathOverride, "Datai");
            Log.Trace("DataPath=" + DataPath);
            _VerifyDirectoryReadable(DataPath);
            DepositPath = Path.Combine(DataPath, "Deposit");
            DepositUserPath = Path.Combine(DataPath, "Deposit_USER");
            WorkDir = ReferenceEquals(rootPathOverride, null)
                ? IniFile.WorkDir.ThrowIfNull("WorkDir")
                : Path.Combine(rootPathOverride, "workdir");
            if (!Directory.Exists(WorkDir))
            {
                Directory.CreateDirectory(WorkDir);
            }
            _VerifyDirectoryReadable(WorkDir);
            _VerifyDirectoryWriteable(WorkDir);

            string fileName = Path.Combine(SystemPath, "client_ini.mnu");
            ClientIni = MenuFile.ParseLocalFile(fileName, IrbisEncoding.Ansi);
            string clientList = IniFile.ClientList ?? "client_m.mnu";
            clientList = Path.Combine(DataPath, clientList);
            Users = ServerUtility.LoadClientList(clientList, ClientIni);
            Contexts = new NonNullCollection<ServerContext>();
            Workers = new NonNullCollection<ServerWorker>();
            Mapper = new CommandMapper(this);
            BanList = new BanMaster();

            DelayedUpdater = Task.Factory.StartNew(_DelayedUpdater);
            Watchdog = new ServerWatchdog(this);
            Watchdog.Task.Start();

            _BuildListeners(setup);

            Log.Trace("IrbisServerEngine::Constructor leave");
        }

        #endregion

        #region Private members

        private CancellationTokenSource _cancellation;

        private void _BuildListeners
            (
                [NotNull] ServerSetup setup
            )
        {
            bool usePortOverride = true;
            int portNumber = setup.PortNumberOverride;
            if (portNumber <= 0)
            {
                usePortOverride = false;
                portNumber = IniFile.IPPort;
            }

            List<IrbisServerListener> listeners = new List<IrbisServerListener>();
            if (setup.UseTcpIpV4)
            {
                listeners.Add
                    (
                        Tcp4Listener.ForPort(portNumber, _cancellation.Token)
                    );

                if (!usePortOverride)
                {
                    for (int i = 1; i < 10; i++)
                    {
                        string parameterName = "IP_PORT" + i;
                        portNumber = IniFile.GetValue(parameterName, 0);
                        if (portNumber > 0)
                        {
                            listeners.Add
                                (
                                    Tcp4Listener.ForPort(portNumber, _cancellation.Token)
                                );
                        }
                    }
                }
            }

            if (setup.UseTcpIpV6)
            {
                listeners.Add
                    (
                        Tcp6Listener.ForPort(portNumber, _cancellation.Token)
                    );
            }

            if (setup.HttpPort > 0)
            {
                listeners.Add
                    (
                        HttpServerListener.ForPort(setup.HttpPort, _cancellation.Token)
                    );
            }

#if FW46 || NETCORE

            if (!string.IsNullOrEmpty(setup.PipeName))
            {
                int instanceCount = setup.PipeInstanceCount;
                if (instanceCount <= 0)
                {
                    instanceCount = 3;
                }
                listeners.Add
                    (
                        new PipeListener
                            (
                                setup.PipeName,
                                instanceCount,
                                _cancellation.Token
                            )
                    );
            }

#endif

            Listeners = listeners.ToArray();
            PortNumber = portNumber;
        }

        private string _GetDepositFile
            (
                [NotNull] string fileName
            )
        {
            string result = Path.GetFullPath(Path.Combine(DepositPath, fileName));
            if (!File.Exists(result))
            {
                result = null;
            }

            return result;
        }

        private void _DelayedUpdater()
        {
            while (true)
            {
                if (_cancellation.IsCancellationRequested)
                {
                    return;
                }

                try
                {
                    ThreadUtility.Sleep(100);
                }
                catch (Exception exception)
                {
                    Log.TraceException
                        (
                            "IrbisServerEngine::_DelayedUpdater",
                            exception
                        );
                }
            }
        }

        private void _VerifyDirectoryReadable
            (
                [NotNull] string path
            )
        {
            // TODO Implement
        }

        private void _VerifyDirectoryWriteable
            (
                [NotNull] string path
            )
        {
            // TODO implement
        }

#if DESKTOP || NETCORE || ANDROID || UAP

        private void _HandleClient
            (
                [NotNull] IrbisServerSocket socket
            )
        {
            Log.Trace("IrbisServerEngine::_HandleClient enter");

            if (_cancellation.IsCancellationRequested)
            {
                socket.Dispose();
                return;
            }

            if (BanList.IsBanned(socket))
            {
                socket.Dispose();
                return;
            }

            WorkData data = new WorkData
            {
                Engine = this,
                Socket = socket
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
#endif

        #endregion

        #region Public methods

        /// <summary>
        /// Before execute the command.
        /// </summary>
        public void OnBeforeExecute
            (
                [NotNull] WorkData data
            )
        {
            Code.NotNull(data, "data");

            // TODO implement
        }

        /// <summary>
        /// After command execution.
        /// </summary>
        public void OnAfterExecute
            (
                [NotNull] WorkData data
            )
        {
            Code.NotNull(data, "data");

            // TODO implement
        }

        /// <summary>
        /// Create the context.
        /// </summary>
        [NotNull]
        public ServerContext CreateContext
            (
                [NotNull] string clientId
            )
        {
            Code.NotNullNorEmpty(clientId, "clientId");

            ServerContext result = new ServerContext
            {
                Id = clientId,
                Connected = DateTime.Now
            };
            lock (SyncRoot)
            {
                Contexts.Add(result);
            }

            return result;
        }

        /// <summary>
        /// Destroy the context.
        /// </summary>
        public void DestroyContext
            (
                [NotNull] ServerContext context
            )
        {
            lock (SyncRoot)
            {
                Contexts.Remove(context);
            }
        }

        /// <summary>
        /// Find context for the client.
        /// </summary>
        [CanBeNull]
        public ServerContext FindContext
            (
                [NotNull] string clientId
            )
        {
            Code.NotNullNorEmpty(clientId, "clientId");

            lock (SyncRoot)
            {
                foreach (ServerContext context in Contexts)
                {
                    if (context.Id == clientId)
                    {
                        return context;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Find context for the client.
        /// </summary>
        [NotNull]
        public ServerContext RequireContext
            (
                [NotNull] WorkData data
            )
        {
            Code.NotNull(data, "data");

            ClientRequest request = data.Request.ThrowIfNull();
            string clientId = request.ClientId.ThrowIfNull();
            ServerContext result = FindContext(clientId);
            if (ReferenceEquals(result, null))
            {
                // Клиент не выполнил вход на сервер
                throw new IrbisException(-3334);
            }

            if (result.Username != request.Login
                || result.Password != request.Password
                || result.Workstation != request.Workstation)
            {
                // Неправильный уникальный идентификатор клиента
                throw new IrbisException(-3335);
            }

            return result;
        }

        /// <summary>
        /// Find administrator context for the user.
        /// </summary>
        [NotNull]
        public ServerContext RequireAdministratorContext
            (
                [NotNull] WorkData data
            )
        {
            Code.NotNull(data, "data");

            ServerContext result = RequireContext(data);
            if (string.IsNullOrEmpty(result.Workstation))
            {
                result.Workstation = data.Request.Workstation;
            }
            if (result.Workstation != "A")
            {
                // Требуется вход администратора
                throw new IrbisException(-3338);
            }

            return result;
        }

        /// <summary>
        /// Find the specified user.
        /// </summary>
        [CanBeNull]
        public UserInfo FindUser
            (
                [NotNull] string username
            )
        {
            Code.NotNullNorEmpty(username, "username");

            lock (SyncRoot)
            {
                foreach (UserInfo user in Users)
                {
                    if (user.Name.SameString(username))
                    {
                        return user;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get name of the default INI file for specified client type.
        /// </summary>
        public string GetDefaultIniName
            (
                [CanBeNull] string proposed,
                int index
            )
        {
            if (!string.IsNullOrEmpty(proposed))
            {
                return proposed;
            }

            MenuEntry entry = ClientIni.Entries.GetItem(index);
            return ReferenceEquals(entry, null)
                ? null
                : entry.Code;
        }

        /// <summary>
        /// Раскрываем ссылки на INI-файлы.
        /// </summary>
        public void FixIniFile
            (
                [NotNull] IniFile iniFile
            )
        {
            Code.NotNull(iniFile, "iniFile");

            while (true)
            {
                IniFile.Section found = null;

                foreach (var section in iniFile.GetSections())
                {
                    if (section.Name.SafeStarts("@"))
                    {
                        found = section;
                        break;
                    }
                }

                if (ReferenceEquals(found, null))
                {
                    break;
                }

                string sectionName = found.Name;
                if (!string.IsNullOrEmpty(sectionName))
                {
                    string filename = sectionName.Substring(1);
                    string ext = Path.GetExtension(filename);
                    if (string.IsNullOrEmpty(ext))
                    {
                        filename = filename + ".ini";
                    }
                    filename = Path.Combine(SystemPath, filename);
                    if (File.Exists(filename))
                    {
                        IniFile substitute = new IniFile(filename, IrbisEncoding.Ansi, false);
                        foreach (IniFile.Section section in substitute.GetSections())
                        {
                            iniFile.MergeSection(section);
                        }
                    }

                    iniFile.RemoveSection(sectionName);
                }
            }
        }

        /// <summary>
        /// Get user INI-file.
        /// </summary>
        [NotNull]
        public string GetUserIniFile
            (
                [NotNull] UserInfo user,
                [NotNull] string workstation
            )
        {
            Code.NotNull(user, "user");
            Code.NotNullNorEmpty(workstation, "workstation");

            string filename;
            switch (workstation)
            {
                case "a":
                case "A":
                    filename = GetDefaultIniName(user.Administrator, 5);
                    break;

                case "b":
                case "B":
                    filename = GetDefaultIniName(user.Circulation, 2);
                    break;

                case "c":
                case "C":
                    filename = GetDefaultIniName(user.Cataloger, 0);
                    break;

                case "k":
                case "K":
                    filename = GetDefaultIniName(user.Provision, 4);
                    break;

                case "m":
                case "M":
                    filename = GetDefaultIniName(user.Cataloger, 0);
                    break;

                case "r":
                case "R":
                    filename = GetDefaultIniName(user.Reader, 1);
                    break;

                case "p":
                case "P":
                    filename = GetDefaultIniName(user.Acquisitions, 3);
                    break;

                default:
                    // Недопустимый клиент
                    throw new IrbisException(-3338);
            }

            if (string.IsNullOrEmpty(filename))
            {
                return string.Empty;
            }

            string result;
            try
            {
                string ext = Path.GetExtension(filename);
                if (string.IsNullOrEmpty(ext))
                {
                    filename = filename + ".ini";
                }
                filename = Path.Combine(SystemPath, filename);
                IniFile iniFile = new IniFile(filename, IrbisEncoding.Ansi, false);
                FixIniFile(iniFile);
                StringWriter writer = new StringWriter();
                iniFile.Save(writer);
                result = writer.ToString();
            }
            catch (Exception exception)
            {
                Log.TraceException("IrbisServerEngine::GetUserIniFile", exception);
                return string.Empty;
            }

            return result;
        }

        /// <summary>
        /// Get MST file path for the database.
        /// </summary>
        [NotNull]
        public string GetMstFile
            (
                [NotNull] string database
            )
        {
            Code.NotNullNorEmpty(database, "database");

            // TODO cache

            string parPath = Path.Combine(DataPath, database + ".par");
            if (!File.Exists(parPath))
            {
                throw new IrbisException(-5555);
            }

            string result;
            try
            {
                ParFile parFile = ParFile.ParseFile(parPath);
                string mstFile = parFile.MstPath.ThrowIfNull();
                mstFile = Path.Combine(SystemPath, mstFile);
                mstFile = Path.Combine(mstFile, database + ".mst");
                mstFile = Path.GetFullPath(mstFile);
                if (!File.Exists(mstFile))
                {
                    throw new IrbisException(-5555);
                }
                result = mstFile;
            }
            catch (Exception exception)
            {
                Log.TraceException("IrbisServerEngine::GetMstFile", exception);
                throw;
            }

            return result;
        }

        /// <summary>
        /// Get database access.
        /// </summary>
        [NotNull]
        public DirectAccess64 GetDatabase
            (
                [NotNull] string database
            )
        {
            Code.NotNullNorEmpty(database, "database");

            // TODO cache

            string mstFile = GetMstFile(database);
            DirectAccess64 result;
            try
            {
                result = new DirectAccess64(mstFile, DirectAccessMode.ReadOnly);
            }
            catch (Exception exception)
            {
                Log.TraceException("IrbisServerEngine::GetDatabase", exception);
                throw;
            }

            return result;
        }

        /// <summary>
        /// Get provider for specified database.
        /// </summary>
        [NotNull]
        public LocalProvider GetProvider
            (
                [NotNull] string database
            )
        {
            Code.NotNullNorEmpty(database, "database");

            // TODO cache

            GetMstFile(database);

            LocalProvider result;
            try
            {
                result = new LocalProvider(SystemPath, DirectAccessMode.ReadOnly, false)
                {
                    Database = database
                };
            }
            catch (Exception exception)
            {
                Log.TraceException("IrbisServerEngine::GetProvider", exception);
                throw;
            }

            return result;
        }

        //=====================================================================

        /// <summary>
        /// Process loop.
        /// </summary>
        public void MainLoop()
        {
            Log.Trace("IrbisServerEngine::MainLoop enter");

#if DESKTOP || NETCORE || ANDROID || UAP

            StartedAt = DateTime.Now;

            foreach (IrbisServerListener listener in Listeners)
            {
                listener.Start();
            }

            while (true)
            {
                if (_cancellation.IsCancellationRequested)
                {
                    Log.Trace("IrbisServerEngine::MainLoop: break signal 1");
                    break;
                }

                if (Paused)
                {
                    SpinWait.SpinUntil(() => Paused, 100);
                    continue;
                }

                try
                {
                    int taskCount = Listeners.Length;
                    Task<IrbisServerSocket>[] tasks
                        = new Task<IrbisServerSocket>[taskCount];
                    for (int i = 0; i < taskCount; i++)
                    {
                        tasks[i] = Listeners[i].AcceptClientAsync();
                    }

                    int ready = Task.WaitAny(tasks, _cancellation.Token);
                    if (_cancellation.IsCancellationRequested)
                    {
                        Log.Trace("IrbisServerEngine::MainLoop: break signal 2");
                        break;
                    }

                    IrbisServerSocket socket = tasks[ready].Result;

                    // Do we really need this?
                    for (int i = 0; i < taskCount; i++)
                    {
                        if (i != ready)
                        {
                            TaskUtility.DisposeTask(tasks[i]);
                        }
                    }

                    _HandleClient(socket);
                }
                catch (AggregateException)
                {
                    Log.Trace("IrbisServerEngine::MainLoop: break signal 3");
                    break;
                }
                catch (OperationCanceledException)
                {
                    Log.Trace("IrbisServerEngine::MainLoop: break signal 3");
                    break;
                }
                catch (Exception exception)
                {
                    Log.TraceException("IrbisServerEngine::MainLoop", exception);
                }
            }

            foreach (IrbisServerListener listener in Listeners)
            {
                listener.Stop();
            }

#endif
            Log.Trace("IrbisServerEngine::MainLoop leave");
        }

        //=====================================================================

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
                                Path.Combine(SystemPath, dictionary[path]),
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
        /// Get the cancellation token.
        /// </summary>
        public CancellationToken GetCancellationToken()
        {
            return _cancellation.Token;
        }

        /// <summary>
        /// Get the workers count.
        /// </summary>
        public int GetWorkerCount()
        {
            lock (SyncRoot)
            {
                return Workers.Count;
            }
        }

        /// <summary>
        /// Cancel <see cref="MainLoop"/> processing.
        /// </summary>
        public void CancelProcessing()
        {
            _cancellation.Cancel();
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
            foreach (IrbisServerListener listener in Listeners)
            {
                listener.Stop();
                listener.Dispose();
            }

            Task updater = DelayedUpdater;
            if (!ReferenceEquals(updater, null))
            {
                updater.Wait(3000);
            }

            if (!ReferenceEquals(Watchdog, null))
            {
                Watchdog.Task.Wait(3000);
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
