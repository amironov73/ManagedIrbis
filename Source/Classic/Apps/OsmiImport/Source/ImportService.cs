// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ImportService.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading.Tasks;

using AM.Logging;

using JetBrains.Annotations;

#endregion

namespace OsmiImport
{
    /// <summary>
    /// Обертка над сервисом Windows.
    /// </summary>
    [PublicAPI]
    public sealed class ImportService
        : ServiceBase
    {
        #region Constants

        /// <summary>
        /// Service name.
        /// </summary>
        public const string ImportDaemon = "IMPORT.daemon";

        #endregion

        #region Properties

        [CanBeNull]
        public Task WorkTask { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImportService()
        {
            ServiceName = ImportDaemon;
            AutoLog = true;
        }

        #endregion

        #region ServiceBase members

        /// <inheritdoc cref="ServiceBase.OnContinue" />
        protected override void OnContinue()
        {
            Log.Trace("ImportService::OnContinue: enter");

            try
            {
                base.OnContinue();

                Daemon.Paused = false;
            }
            catch (Exception exception)
            {
                Log.TraceException("ImportService::OnContinue", exception);
            }

            Log.Trace("ImportService::OnContinue: leave");
        }

        /// <inheritdoc cref="ServiceBase.OnPause" />
        protected override void OnPause()
        {
            Log.Trace("ImportService::OnPause: enter");

            try
            {
                base.OnPause();

                Daemon.Paused = true;
            }
            catch (Exception exception)
            {
                Log.TraceException("ImportService::OnPause", exception);
            }

            Log.Trace("ImportService::OnPause: leave");
        }

        /// <inheritdoc cref="ServiceBase.OnShutdown" />
        protected override void OnShutdown()
        {
            Log.Trace("ImportService::OnShutdown: enter");

            try
            {
                //var client = Daemon.GetClient();
                //client.StopReceiving();

                base.OnShutdown();
            }
            catch (Exception exception)
            {
                Log.TraceException("ImportService::OnShutdown", exception);
            }

            Log.Trace("ImportService::OnShutdown: leave");
        }

        /// <inheritdoc cref="ServiceBase.OnStart" />
        protected override void OnStart
            (
                string[] args
            )
        {
            Log.Trace("ImportService::OnStart: enter");

            try
            {
                base.OnStart(args);

                string message = "Arguments: " + string.Join(" ", args);
                EventLog.WriteEntry(message, EventLogEntryType.Information);

                //Daemon.GetClient();
                //Daemon.MessageLoop();
            }
            catch (Exception exception)
            {
                Log.TraceException("ImportService::OnStart", exception);
            }

            Log.Trace("ImportService::OnStart: leave");
        }

        /// <inheritdoc cref="ServiceBase.OnStop" />
        protected override void OnStop()
        {
            Log.Trace("ImportService::OnStop: enter");

            try
            {
                //var client = Daemon.GetClient();
                //client.StopReceiving();

                base.OnStop();
            }
            catch (Exception exception)
            {
                Log.TraceException("ImportService::OnStop", exception);
            }

            Log.Trace("ImportService::OnStop: leave");
        }

        #endregion
    }
}
