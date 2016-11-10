/* BatchParallelProcessor.cs -- 
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
    public sealed class BatchParallelProcessor<T>
        : ProcessorBase<T>
{
    private readonly int _batchSize;
    private readonly Func<List<T>, CancellationToken, Task> _processHandler;
    private readonly Action<List<T>, Exception> _exceptionHandler;
        
    public BatchParallelProcessor
        (
            int maxParallelization,
            int batchSize,
            Func<List<T>, CancellationToken, Task> processHandler,
            Action<List<T>, Exception> exceptionHandler = null,
            int disposeTimeoutMs = 30000,
            int? maxQueueSize = null
        )
        : base(maxParallelization, disposeTimeoutMs, maxQueueSize)
    {
        if (batchSize < 1)
            throw new ArgumentException
                (
                    "batchSize is required"
                );
 
        _batchSize = batchSize;
        _processHandler = processHandler;
        _exceptionHandler = exceptionHandler;
    }
 
    protected override async Task ProcessLoopAsync()
    {
        while (!CancelSource.IsCancellationRequested)
        {
            var count = Math.Min(_batchSize, Queue.Count + 1);
            var list = new List<T>(count);
 
            T item;
            while (list.Count < _batchSize && Queue.TryDequeue(out item))
                list.Add(item);
 
            if (list.Count == 0)
                return;
 
            try
            {
                await _processHandler(list, CancelSource.Token).ConfigureAwait(false);
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
                    _exceptionHandler.Invoke(list, ex);
                }
            }
        }
    }
}
}

#endif
