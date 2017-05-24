// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ParallelProcessor.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW45

#region Using directives

using System;
using System.Threading;
using System.Threading.Tasks;

using AM.Logging;

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
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
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
            {
                Log.Error
                    (
                        "ParallelProcessor::Constructor: "
                        + "maxParallelization="
                        + maxParallelization
                    );

                throw new ArgumentException
                (
                    "maxParallelization is required"
                );
            }

            _processHandler = processHandler;
            _exceptionHandler = exceptionHandler;
        }

        #endregion

        #region Private members

        private readonly Func<T, CancellationToken, Task> _processHandler;
        private readonly Action<T, Exception> _exceptionHandler;

        #endregion

        #region ProcessorBase members

        /// <inheritdoc/>
        protected override async Task ProcessLoopAsync()
        {
            T item;
            while (!CancelSource.IsCancellationRequested
                && Queue.TryDequeue(out item))
            {
                try
                {
                    await _processHandler(item, CancelSource.Token)
                        .ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    if (CancelSource.IsCancellationRequested)
                    {
                        // Cancellation was requested, ignore and exit.
                        return;
                    }

                    Log.Error
                        (
                            "ParallelProcessor::ProcessLoopAsync: "
                            + "TaskCanceledException"
                        );

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

        #endregion
    }
}

#endif
