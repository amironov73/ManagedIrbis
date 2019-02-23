// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ServerWatchdog.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AM.Logging;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server
{
    /// <summary>
    /// Server watchdog.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ServerWatchdog
    {
        #region Constants

        /// <summary>
        /// Default timeout, seconds.
        /// </summary>
        public const int DefaultTimeout = 30;

        #endregion

        #region Properties

        /// <summary>
        /// Engine.
        /// </summary>
        [NotNull]
        public IrbisServerEngine Engine { get; private set; }

        /// <summary>
        /// Cancellation token.
        /// </summary>
        public CancellationToken Token { get; private set; }

        /// <summary>
        /// Corresponding task.
        /// </summary>
        public Task Task { get; private set; }

        /// <summary>
        /// Timeout, seconds.
        /// </summary>
        public int Timeout { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ServerWatchdog
            (
                [NotNull] IrbisServerEngine engine
            )
        {
            Code.NotNull(engine, "engine");

            Engine = engine;
            Token = engine.GetCancellationToken();
            Task = new Task(MainLoop);
            Timeout = DefaultTimeout;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Main loop.
        /// </summary>
        public void MainLoop()
        {
            if (Timeout <= 0)
            {
                // TODO is it right decision?
                return;
            }

            while (!Token.IsCancellationRequested)
            {
                ThreadUtility.Sleep(100);

                try
                {
                    ServerWorker[] workers;

                    lock (Engine.SyncRoot)
                    {
                        workers = Engine.Workers.ToArray();
                    }

                    DateTime threshold = DateTime.Now.AddSeconds(-Timeout);
                    ServerWorker[] longRunning = workers
                        .Where(w => w.Data.Started < threshold)
                        .ToArray();

                    foreach (ServerWorker worker in longRunning)
                    {
                        Log.Warn("Long running worker: " + worker.Data.Task.Id);

                        // TODO kill long running task?
                    }
                }
                catch (Exception exception)
                {
                    Log.TraceException("ServerWatchdog::MainLoop", exception);
                }
            }
        }

        #endregion
    }
}
