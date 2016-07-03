/* IrbisConnectionPool.cs -- пул соединений с сервером.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Pooling
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
        public string ConnectionString { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public IrbisConnectionPool()
        {
            if (string.IsNullOrEmpty(DefaultConnectionString))
            {
                DefaultConnectionString
                    = ManagedClientUtility.GetStandardConnectionString();
            }
            ConnectionString = DefaultConnectionString;
        }

        /// <summary>
        /// Конструктор.
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
        /// Конструктор с конкретной строкой соединения.
        /// </summary>
        public IrbisConnectionPool
            (
                string connectionString
            )
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public IrbisConnectionPool
            (
                int capacity,
                string connectionString
            )
        {
            Capacity = capacity;
            ConnectionString = connectionString;
        }

        #endregion

        #region Private members

        private static int _defaultCapacity = 5;

        private readonly List<ManagedClient64> _activeConnections
            = new List<ManagedClient64>();

        private readonly List<ManagedClient64> _idleConnections
            = new List<ManagedClient64>();

        private readonly AutoResetEvent _event = new AutoResetEvent(false);

        private readonly object _syncRoot = new object();

        [CanBeNull]
        private ManagedClient64 _GetNewClient()
        {
            if (_activeConnections.Count >= Capacity)
            {
                return null;
            }

            ManagedClient64 result = new ManagedClient64();
            result.ParseConnectionString(ConnectionString);
            result.Connect();

            return result;
        }

        [CanBeNull]
        private ManagedClient64 _GetIdleClient()
        {
            if (_idleConnections.Count == 0)
            {
                return null;
            }
            ManagedClient64 result = _idleConnections[0];
            _idleConnections.RemoveAt(0);
            return result;
        }

        [NotNull]
        private ManagedClient64 _WaitForClient()
        {
            while (true)
            {
                if (!_event.WaitOne())
                {
                    throw new ApplicationException("WaitOne failed");
                }
                lock (_syncRoot)
                {
                    ManagedClient64 result = _GetIdleClient();
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
        public ManagedClient64 AcquireConnection()
        {
            ManagedClient64 result;

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

        /// <summary>
        /// Исполнение некоторых действий на подключении из пула.
        /// </summary>
        /// <param name="action"></param>
        public void Execute
            (
                [NotNull] Action<ManagedClient64> action
            )
        {
            if (ReferenceEquals(action, null))
            {
                throw new ArgumentNullException("action");
            }

            using (IrbisPoolGuard guard = new IrbisPoolGuard (this))
            {
                action(guard);
            }
        }

        /// <summary>
        /// Исполнение некоторых действий на подключении из пула.
        /// </summary>
        public void Execute<T>
            (
                [NotNull] Action<ManagedClient64, T> action,
                T userData
            )
        {
            if (ReferenceEquals(action, null))
            {
                throw new ArgumentNullException("action");
            }

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
                [NotNull] Func<ManagedClient64,T1,TResult> function,
                T1 userData
            )
        {
            if (ReferenceEquals(function, null))
            {
                throw new ArgumentNullException("function");
            }

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
                [NotNull] ManagedClient64 client
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }

            lock (_syncRoot)
            {
                if (!_activeConnections.Contains(client))
                {
                    throw new ApplicationException("Foreign connection");
                }
                _activeConnections.Remove(client);
                if (client.Connected)
                {
                    _idleConnections.Add(client);
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
                    throw new ApplicationException("WaitOne failed");
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

        #endregion

        #region IDisposable members

        /// <summary>
        /// Performs application-defined tasks associated
        /// with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="System.ApplicationException">
        /// Have active connections</exception>
        public void Dispose()
        {
            lock (_syncRoot)
            {
                if (_activeConnections.Count != 0)
                {
                    throw new ApplicationException("Have active connections");
                }

                foreach (ManagedClient64 client in _idleConnections)
                {
                    client.Disconnect();
                }
            }
        }

        #endregion
    }
}
