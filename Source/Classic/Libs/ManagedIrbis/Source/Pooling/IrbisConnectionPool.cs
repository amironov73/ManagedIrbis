// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisConnectionPool.cs -- пул соединений с сервером.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable ConvertClosureToMethodGroup

namespace ManagedIrbis.Pooling
{
    /// <summary>
    /// Пул соединений с сервером.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class IrbisConnectionPool
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Строка подключения по умолчанию.
        /// </summary>
        public static string DefaultConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// Количество одновременных подключений по умолчанию.
        /// </summary>
        public static int DefaultCapacity
        {
            get { return _defaultCapacity; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                _defaultCapacity = value;
            }
        }

        /// <summary>
        /// Количество одновременных подключений.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Строка подключения к серверу.
        /// </summary>
        public string ConnectionString { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisConnectionPool()
        {
            Log.Trace
                (
                    "IrbisConnectionPool::Constructor"
                );

            Capacity = DefaultCapacity;
            ConnectionString = DefaultConnectionString;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisConnectionPool
            (
                int capacity
            )
            : this()
        {
            if (capacity < 1)
            {
                capacity = DefaultCapacity;
            }
            Capacity = capacity;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisConnectionPool
            (
                [NotNull] string connectionString
            )
            : this()
        {
            Code.NotNullNorEmpty(connectionString, "connectionString");

            ConnectionString = connectionString;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public IrbisConnectionPool
            (
                int capacity,
                [NotNull] string connectionString
            )
            : this(capacity)
        {
            Code.NotNullNorEmpty(connectionString, "connectionString");

            ConnectionString = connectionString;
        }

        #endregion

        #region Private members

        private static int _defaultCapacity = 5;

        private readonly List<IrbisConnection> _activeConnections
            = new List<IrbisConnection>();

        private readonly List<IrbisConnection> _idleConnections
            = new List<IrbisConnection>();

        private readonly AutoResetEvent _event = new AutoResetEvent(false);

        private readonly object _syncRoot = new object();

        [CanBeNull]
        private IrbisConnection _GetNewClient()
        {
            if (_activeConnections.Count >= Capacity)
            {
                Log.Trace
                    (
                        "IrbisConnectionPool::_GetNewClient: "
                        + "capacity exhausted"
                    );

                return null;
            }

            IrbisConnection result = new IrbisConnection();
            result.ParseConnectionString(ConnectionString);
            result.Connect();

            return result;
        }

        [CanBeNull]
        private IrbisConnection _GetIdleClient()
        {
            if (_idleConnections.Count == 0)
            {
                Log.Trace
                    (
                        "IrbisConnectionPool::_GetIdleClient: "
                        + "no idle clients"
                    );

                return null;
            }

            IrbisConnection result = _idleConnections[0];
            _idleConnections.RemoveAt(0);

            return result;
        }

        [NotNull]
        private IrbisConnection _WaitForClient()
        {
            while (true)
            {
                if (!_event.WaitOne())
                {
                    Log.Error
                        (
                            "IrbisConnectionPool::_WaitForClient: "
                            + "WaitOne failed"
                        );

                    throw new IrbisException("WaitOne failed");
                }

                lock (_syncRoot)
                {
                    IrbisConnection result = _GetIdleClient();
                    if (!ReferenceEquals(result, null))
                    {
                        return result;
                    }
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Требование нового подключения к серверу.
        /// </summary>
        /// <remarks>Может подвесить поток на неопределённое время.
        /// </remarks>
        [NotNull]
        public IrbisConnection AcquireConnection()
        {
            IrbisConnection result;

            lock (_syncRoot)
            {
                result = _GetIdleClient() ?? _GetNewClient();
            }
            if (ReferenceEquals(result, null))
            {
                result = _WaitForClient();
            }

            return result;
        }

#if FW45

        /// <summary>
        /// Требование нового подключения к серверу.
        /// </summary>
        public Task<IrbisConnection> AcquireConnectionAsync()
        {
            Task<IrbisConnection> result = Task<IrbisConnection>.Factory.StartNew
                    (
                        () =>
                        {
                            return AcquireConnection();
                        }
                    );

            return result;
        }

#endif

        /// <summary>
        /// Исполнение некоторых действий на подключении из пула.
        /// </summary>
        public void Execute
            (
                [NotNull] Action<IrbisConnection> action
            )
        {
            Code.NotNull(action, "action");

            using (IrbisPoolGuard guard = new IrbisPoolGuard (this))
            {
                action(guard);
            }
        }

#if FW45

        /// <summary>
        /// Исполнение некоторых действий на подключении из пула.
        /// </summary>
        public Task ExecuteAsync
            (
                [NotNull] Action<IrbisConnection> action
            )
        {
            Code.NotNull(action, "action");

            Task result = Task.Factory.StartNew
                (
                    () =>
                    {
                        Execute(action);
                    }
                );

            return result;
        }

#endif

        /// <summary>
        /// Исполнение некоторых действий на подключении из пула.
        /// </summary>
        public void Execute<T>
            (
                [NotNull] Action<IrbisConnection, T> action,
                T userData
            )
        {
            Code.NotNull(action, "action");

            using (IrbisPoolGuard guard = new IrbisPoolGuard(this))
            {
                action
                    (
                        guard,
                        userData
                    );
            }
        }

        /// <summary>
        /// Исполнение некоторых действий на подключении из пула.
        /// </summary>
        public TResult Execute<TResult, T1>
            (
                [NotNull] Func<IrbisConnection,T1,TResult> function,
                T1 userData
            )
        {
            Code.NotNull(function, "function");

            using (IrbisPoolGuard guard = new IrbisPoolGuard(this))
            {
                TResult result = function
                    (
                        guard,
                        userData
                    );
                return result;
            }
        }

        /// <summary>
        /// Возвращение подключения в пул.
        /// </summary>
        public void ReleaseConnection
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            lock (_syncRoot)
            {
                if (!_activeConnections.Contains(connection))
                {
                    Log.Error
                        (
                            "IrbisConnectionPool::ReleaseConnection: "
                            + "foreign connection detected"
                        );

                    throw new IrbisException("Foreign connection");
                }

                _activeConnections.Remove(connection);
                if (connection.Connected)
                {
                    _idleConnections.Add(connection);
                }
                _event.Set();
            }
        }

        /// <summary>
        /// Закрывает простаивающие соединения.
        /// </summary>
        public void ReleaseIdleConnections()
        {
            lock (_syncRoot)
            {
                while (_idleConnections.Count != 0)
                {
                    _idleConnections[0].Dispose();
                    _idleConnections.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// Ожидание закрытия всех активных подключений.
        /// </summary>
        public void WaitForAllConnections()
        {
            while (true)
            {
                if (!_event.WaitOne())
                {
                    throw new IrbisException("WaitOne failed");
                }

                lock (_syncRoot)
                {
                    if (_activeConnections.Count == 0)
                    {
                        return;
                    }
                }
            }
        }

#if FW45

        /// <summary>
        /// Ожидание закрытия всех активных подключений.
        /// </summary>
        public Task WaitForAllConnectionsAsync()
        {
            Task result = Task.Factory.StartNew
                (
                    () =>
                    {
                        WaitForAllConnections();
                    }
                );

            return result;
        }

#endif

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            Log.Trace("IrbisConnectionPool::Dispose: Enter");

            lock (_syncRoot)
            {
                if (_activeConnections.Count != 0)
                {
                    Log.Error
                        (
                            "IrbisConnectionPool::Dispose: "
                            + "have active connections"
                        );

                    throw new IrbisException("Have active connections");
                }

                foreach (IrbisConnection client in _idleConnections)
                {
                    client.Dispose();
                }
            }

            Log.Trace("IrbisConnectionPool::Dispose: Leave");
        }

        #endregion
    }
}
