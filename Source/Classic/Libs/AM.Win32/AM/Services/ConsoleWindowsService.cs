/* ConsoleWindowsService.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
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

namespace AM.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Borrowed from Tom DuPont:
    /// http://www.tomdupont.net/2013/03/multiple-tasks-in-windows-service.html
    /// </remarks>
    public class ConsoleWindowsService
        : ServiceBase
    {
        private static readonly TimeSpan StartStopTimeoutMin = TimeSpan.FromSeconds(15);
        private static readonly TimeSpan StartStopTimeoutMax = TimeSpan.FromSeconds(25);

        private readonly object _startStopLock;
        private readonly CancellationTokenSource _runCancelSource;

        private readonly IList<ServiceTaskPair> _taskPairs;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConsoleWindowsService
            (
                params IWindowsServiceTask[] windowsServiceTasks
            )
        {
            // Ensure that we have service tasks to run.
            if (windowsServiceTasks.Length == 0)
            {
                throw new ArgumentException
                    (
                        "WindowsServiceTasks required",
                        "windowsServiceTasks"
                    );
            }

            _startStopLock = new object();
            _runCancelSource = new CancellationTokenSource();

            _taskPairs = windowsServiceTasks.Select(ServiceTaskPair.Create).ToList();
        }

        /// <summary>
        /// Start.
        /// </summary>
        public void Start
            (
                params string[] args
            )
        {
            // If not user interactive then run the service normally.
            if (!Environment.UserInteractive)
            {
                Run(new ServiceBase[] { this });
                return;
            }


            // Running in console mode, call OnStart.
            OnStart(args);

            // Wait for user input to before shutting down.
            Console.ReadLine();

            if (_runCancelSource.IsCancellationRequested)
            {
                // Something already stopped the services.
            }
            else
            {
                // Call OnStop before to initiate shutdown.
                OnStop();
            }

            // Let user read output before shutting down.
            Console.ReadLine();
        }

        protected override void OnStart(string[] args)
        {
            LockAndTry(() =>
            {
                // Call OnStart for each service, same way windows would.
                using (var startCancelSource = new CancellationTokenSource(StartStopTimeoutMin))
                {
                    var startTasks = new Task[_taskPairs.Count];

                    for (var i = 0; i < _taskPairs.Count; i++)
                        startTasks[i] = _taskPairs[i].ServiceTask.OnStartAsync(args, startCancelSource.Token);

                    Task.WaitAll(startTasks, StartStopTimeoutMax);
                }

                // Call RunAsync for each service.
                foreach (var pair in _taskPairs)
                    pair.Task = RunServiceAsync(pair.ServiceTask);
            });
        }

        private async Task RunServiceAsync
            (
                IWindowsServiceTask serviceTask
            )
        {
            Exception exception = null;

            try
            {
                await serviceTask.RunAsync(_runCancelSource.Token).ConfigureAwait(false);
            }
            catch (TaskCanceledException ex)
            {
                if (!_runCancelSource.IsCancellationRequested)
                    exception = ex;
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (exception != null)
            {
                if (Environment.UserInteractive)
                {
                    // _lazyConsoleLogger.Value.Error(exception);
                }

                // ReSharper disable once InconsistentlySynchronizedField
                //_registeredlogger.ErrorFormat("ConsoleWindowsService.RunServiceAsync - Unhandled Exception - Name: {0}", exception, serviceTask.Name);
            }

            // If specified, shutdown all serviecs.
            if (serviceTask.IsShutdownOnStop)
            {
                OnStop(serviceTask);
            }
        }

        protected override void OnStop()
        {
            OnStop(null);
        }

        protected void OnStop
            (
                IWindowsServiceTask serviceTask
            )
        {
            LockAndTry(() =>
            {
                // Exit if another thread has already initiated shutdown.
                if (_runCancelSource.IsCancellationRequested)
                    return true;

                //_lazyConsoleLogger.Value.Debug("Stopping...");

                // Signal shutdown.
                _runCancelSource.Cancel();

                // Call OnStop for each service, same way windows would.
                using (var stopCancelSource = new CancellationTokenSource(StartStopTimeoutMin))
                {
                    var stopTasks = new Task[_taskPairs.Count];

                    for (var i = 0; i < _taskPairs.Count; i++)
                        stopTasks[i] = _taskPairs[i].ServiceTask.OnStopAsync(stopCancelSource.Token);

                    Task.WaitAll(stopTasks, StartStopTimeoutMax);
                }

                // Find...
                // 1) All OTHER service tasks (to avoid deadlock) 
                // 2) that we should wait on stop
                // 3) and have tasks to wait on.
                // ...and then select their tasks into an array.
                var tasks = _taskPairs
                    .Where(p => !ReferenceEquals(p.ServiceTask, serviceTask))
                    .Where(p => p.ServiceTask.IsWaitOnStop)
                    .Where(p => p.Task != null)
                    .Select(p => p.Task)
                    .ToArray();

                // Wait on the other tasks.
                Task.WaitAll(tasks, StartStopTimeoutMax);

                //_lazyConsoleLogger.Value.Debug("...Stopped");

                // Success if normal OnStop call from ServiceProcess, otherwise
                // this was unexpected and should cause a shutdown. 
                return serviceTask == null;
            });
        }

        private void LockAndTry(Action action)
        {
            LockAndTry(() =>
            {
                action();
                return true;
            });
        }

        private void LockAndTry(Func<bool> func)
        {
            lock (_startStopLock)
            {
                bool isSuccess;

                try
                {
                    isSuccess = func();
                }
                catch (Exception ex)
                {
                    //_lazyConsoleLogger.Value.Fatal(ex);
                    //_registeredlogger.Fatal(ex);

                    isSuccess = false;
                }

                if (isSuccess)
                    return;

                if (Environment.UserInteractive)
                {
                    //_lazyConsoleLogger.Value.Fatal("Service has stopped unexpectedly.");
                }
                else
                {
                    Environment.Exit(1);
                }
            }
        }

        private class ServiceTaskPair
        {
            public static ServiceTaskPair Create(IWindowsServiceTask serviceTask)
            {
                return new ServiceTaskPair(serviceTask);
            }

            private ServiceTaskPair(IWindowsServiceTask serviceTask)
            {
                ServiceTask = serviceTask;
            }

            public IWindowsServiceTask ServiceTask { get; private set; }

            public Task Task { get; set; }
        }
    }
}
