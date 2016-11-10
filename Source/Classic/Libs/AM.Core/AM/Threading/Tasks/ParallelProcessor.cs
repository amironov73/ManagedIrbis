/* ParallelProcessor.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW45

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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

namespace AM.Threading.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Borrowed from Tom DuPont:
    /// http://www.tomdupont.net/2016/09/net-asynchronous-parallel-batch.html
    /// </remarks>
    public sealed class ParallelProcessor<T>
        : ProcessorBase<T>
    {
        private readonly Func<T, CancellationToken, Task> _processHandler;
        private readonly Action<T, Exception> _exceptionHandler;

        public ParallelProcessor
            (
                int maxParallelization,
                Func<T, CancellationToken, Task> processHandler,
                Action<T, Exception> exceptionHandler = null,
                int disposeTimeoutMs = 30000,
                int? maxQueueSize = null
            )
            : base(maxParallelization, disposeTimeoutMs, maxQueueSize)
        {
            if (maxParallelization < 1)
                throw new ArgumentException
                    (
                        "maxParallelization is required"
                    );

            _processHandler = processHandler;
            _exceptionHandler = exceptionHandler;
        }

        protected override async Task ProcessLoopAsync()
        {
            T item;
            while (!CancelSource.IsCancellationRequested && Queue.TryDequeue(out item))
            {
                try
                {
                    await _processHandler(item, CancelSource.Token).ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    if (CancelSource.IsCancellationRequested)
                    {
                        // Cancellation was requested, ignore and exit.
                        return;
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    if (!ReferenceEquals(_exceptionHandler, null))
                    {
                        _exceptionHandler.Invoke(item, ex);
                    }
                }
            }
        }
    }
}

#endif
