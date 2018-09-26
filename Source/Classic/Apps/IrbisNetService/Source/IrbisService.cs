// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisService.cs --
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

using ManagedIrbis.Server;

#endregion

namespace IrbisNetService
{
    /// <summary>
    /// Обертка над сервисом Windows.
    /// </summary>
    [PublicAPI]
    public class IrbisService
        : ServiceBase
    {
        #region Constants

        /// <summary>
        /// Service name.
        /// </summary>
        public const string IrbisNet = "IRBIS.NET";

        #endregion

        #region Properties

        /// <summary>
        /// Engine.
        /// </summary>
        [CanBeNull]
        public IrbisServerEngine Engine { get; private set; }

        [CanBeNull]
        public Task WorkTask { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisService()
        {
            ServiceName = IrbisNet;
            AutoLog = true;
        }

        #endregion

        #region Private members

        private void _CreateEngine
            (
                string[] args
            )
        {
            if (ReferenceEquals(Engine, null))
            {
                try
                {
                    Engine = ServerUtility.CreateEngine(args);
                    WorkTask = Task.Factory.StartNew(Engine.MainLoop);
                }
                catch (Exception exception)
                {
                    EventLog.WriteEntry
                        (
                            "Exception: " + exception,
                            EventLogEntryType.Error
                        );

                    Engine = null;
                    WorkTask = null;
                    Stop();
                }
            }
        }

        private void _DestroyEngine()
        {
            IrbisServerEngine engine = Engine;
            if (!ReferenceEquals(engine, null))
            {
                engine.CancelProcessing();
                Task workTask = WorkTask;
                if (!ReferenceEquals(workTask, null))
                {
                    int count = 0;

                    while (engine.GetWorkerCount() != 0)
                    {
                        if (count > 50)
                        {
                            break;
                        }

                        RequestAdditionalTime(100);
                        workTask.Wait(100);
                    }
                }

                engine.Dispose();
            }

            WorkTask = null;
            Engine = null;
        }

        #endregion

        #region ServiceBase members

        /// <inheritdoc cref="ServiceBase.OnContinue" />
        protected override void OnContinue()
        {
            Log.Trace("IrbisService::OnContinue: enter");

            try
            {
                base.OnContinue();

                if (!ReferenceEquals(Engine, null))
                {
                    Engine.Paused = true;
                }
            }
            catch (Exception exception)
            {
                Log.TraceException("IrbisService::OnContinue", exception);
            }

            Log.Trace("IrbisService::OnContinue: leave");
        }

        /// <inheritdoc cref="ServiceBase.OnPause" />
        protected override void OnPause()
        {
            Log.Trace("IrbisService::OnPause: enter");

            try
            {
                base.OnPause();

                if (!ReferenceEquals(Engine, null))
                {
                    Engine.Paused = true;
                }
            }
            catch (Exception exception)
            {
                Log.TraceException("IrbisService::OnPause", exception);
            }

            Log.Trace("IrbisService::OnPause: leave");
        }

        /// <inheritdoc cref="ServiceBase.OnShutdown" />
        protected override void OnShutdown()
        {
            Log.Trace("IrbisService::OnShutdown: enter");

            try
            {
                _DestroyEngine();

                base.OnShutdown();
            }
            catch (Exception exception)
            {
                Log.TraceException("IrbisService::OnShutdown", exception);
            }

            Log.Trace("IrbisService::OnShutdown: leave");
        }

        /// <inheritdoc cref="ServiceBase.OnStart" />
        protected override void OnStart
            (
                string[] args
            )
        {
            Log.Trace("IrbisService::OnStart: enter");

            try
            {
                base.OnStart(args);

                string message = "Arguments: " + string.Join(" ", args);
                EventLog.WriteEntry(message, EventLogEntryType.Information);

                _CreateEngine(args);
            }
            catch (Exception exception)
            {
                Log.TraceException("IrbisService::OnStart", exception);
            }

            Log.Trace("IrbisService::OnStart: leave");
        }

        /// <inheritdoc cref="ServiceBase.OnStop" />
        protected override void OnStop()
        {
            Log.Trace("IrbisService::OnStop: enter");

            try
            {
                _DestroyEngine();

                base.OnStop();
            }
            catch (Exception exception)
            {
                Log.TraceException("IrbisService::OnStop", exception);
            }

            Log.Trace("IrbisService::OnStop: leave");
        }

        #endregion
    }
}
