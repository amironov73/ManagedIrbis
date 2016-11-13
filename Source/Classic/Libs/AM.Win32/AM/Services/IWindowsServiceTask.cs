/* IWindowsServiceTask.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW4

#region Using directives

using System;
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
    /// Interface to run a windows service as a task.
    /// Implement this instead of extending System.ServiceProcess.ServiceBase
    /// </summary>
    /// <remarks>Borrowed from Tom DuPont:
    /// http://www.tomdupont.net/2013/03/multiple-tasks-in-windows-service.html
    /// </remarks>
    public interface IWindowsServiceTask
    {
        /// <summary>
        /// Name of the service.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// <value>true</value> if the RunAsync task should be awaited when cancellation is requested.
        /// </summary>
        bool IsWaitOnStop { get; }

        /// <summary>
        /// <value>true</value> if all services should shutdown when the RunAsync task completes.
        /// </summary>
        bool IsShutdownOnStop { get; }

        /// <summary>
        /// An async version of System.ServiceProcess.ServiceBase.OnStart
        /// This is different than RunAsync because the process should always stop immediately if this fails.
        /// </summary>
        Task OnStartAsync(string[] args, CancellationToken cancellationToken);

        /// <summary>
        /// The actual task that is running the service.
        /// The returned task will be consumed based on the IsWaitOnStop and IsShutdownOnStop properties
        /// </summary>
        Task RunAsync(CancellationToken cancellationToken);

        /// <summary>
        /// An async version of System.ServiceProcess.ServiceBase.OnStop
        /// </summary>
        Task OnStopAsync(CancellationToken cancellationToken);
    }
}

#endif

