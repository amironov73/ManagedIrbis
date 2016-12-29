// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisMonitor.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if NETCORE || FW45

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Monitoring
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisMonitor
    {
        #region Properties

        /// <summary>
        /// Whether monitoring is active.
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [NotNull]
        public string Database { get; private set; }

        /// <summary>
        /// Interval between measuring.
        /// </summary>
        public int Interval 
        {
            get { return _interval; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("value");
                }

                _interval = value;
            }
        }

        /// <summary>
        /// Sink to write data.
        /// </summary>
        public NonNullValue<MonitoringSink> Sink { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisMonitor
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string database
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");

            Connection = connection;
            Database = database;
            _interval = 1000;
            Sink = new NullMonitoringSink();
        }

        #endregion

        #region Private members

        private int _interval;

        private Task _workerTask;

        private void _MonitoringRoutine()
        {
            while (Active)
            {
                MonitoringData data = GetDataPortion();
                if (!Sink.Value.WriteData(data))
                {
                    Active = false;
                    break;
                }

                Task.Delay(Interval).Wait();

            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get portion of monitoring data.
        /// </summary>
        [NotNull]
        public MonitoringData GetDataPortion()
        {
            MonitoringData result = new MonitoringData
            {
                Moment = DateTime.Now
            };

            ServerStat serverStat = Connection.GetServerStat();
            result.Clients = serverStat.RunningClients.Length;
            result.Commands = serverStat.TotalCommandCount;

            DatabaseInfo databaseInfo = Connection.GetDatabaseInfo(Database);
            if (!ReferenceEquals(databaseInfo.LogicallyDeletedRecords, null))
            {
                result.DeletedRecords = databaseInfo.LogicallyDeletedRecords.Length;
            }
            if (!ReferenceEquals(databaseInfo.LockedRecords, null))
            {
                result.LockedRecords = databaseInfo.LockedRecords.Length;
            }

            return result;
        }

        /// <summary>
        /// Start monitoring.
        /// </summary>
        public void StartMonitoring()
        {
            if (Active)
            {
                return;
            }

            Active = true;
            _workerTask = new Task(_MonitoringRoutine);
            //_workerThread = new Thread(_MonitoringRoutine)
            //{
            //    Name = "IrbisMonitor",
            //    IsBackground = true
            //};
            _workerTask.Start();
        }

        /// <summary>
        /// Stop monitoring.
        /// </summary>
        public void StopMonitoring()
        {
            if (!Active)
            {
                return;
            }

            Active = false;

            if (!ReferenceEquals(_workerTask, null))
            {
                //_workerTask.Wait();
                _workerTask = null;
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}

#endif
