// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ProcessorBase.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW45

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace AM.Threading.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Borrowed from Tom DuPont:
    /// http://www.tomdupont.net/2016/09/net-asynchronous-parallel-batch.html
    /// </remarks>
    public abstract class ProcessorBase<T>
        : IDisposable
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ProcessorBase
        (
            int maxParallelization,
            int disposeTimeoutMs,
            int? maxQueueSize
        )
        {
            _tasks = new Task[maxParallelization];
            _disposeTimeoutMs = disposeTimeoutMs;
            _maxQueueSize = maxQueueSize;
        }

        #endregion

        #region Private members

        /// <summary>
        /// Queue.
        /// </summary>
        protected readonly ConcurrentQueue<T> Queue 
            = new ConcurrentQueue<T>();

        /// <summary>
        /// Cancellation token.
        /// </summary>
        protected readonly CancellationTokenSource CancelSource
            = new CancellationTokenSource();

        private readonly object _lock = new object();
        private readonly Task[] _tasks;
        private readonly int _disposeTimeoutMs;
        private readonly int? _maxQueueSize;

        private bool _isDisposed;

        /// <summary>
        /// Process loop.
        /// </summary>
        protected abstract Task ProcessLoopAsync();

        private void TryStartProcessLoop()
        {
            // Another thread is in the lock, bail out.
            if (!Monitor.TryEnter(_lock))
            {
                return;
            }

            // Create task outside of lock to ensure that we attach the
            // continue without while another thread can be in the block.
            Task task;

            try
            {
                // If cancellation has been requested, do not start.
                if (CancelSource.IsCancellationRequested)
                {
                    return;
                }

                // If the queue is empty, do not start.
                if (Queue.Count == 0)
                {
                    return;
                }

                var freeIndex = 0;
                var activeCount = 0;

                // Find last free index
                for (var i = 0; i < _tasks.Length; i++)
                {
                    if (_tasks[i] == null 
                        || _tasks[i].IsCompleted
                       )
                    {
                        freeIndex = i;
                    }
                    else
                    {
                        activeCount++;
                    }
                }

                // All tasks are active, do not start.
                if (activeCount == _tasks.Length)
                {
                    return;
                }

                // Only one in queue, at least one thread is active,
                // do not start additional thread.
                if (activeCount > 0 && Queue.Count <= 1)
                {
                    return;
                }

                // Start a new task to process the queue.
                task = _tasks[freeIndex] = Task.Run
                (
                    // ReSharper disable once RedundantCast
                    (Func<Task>)ProcessLoopAsync,
                    CancelSource.Token
                );
            }
            finally
            {
                Monitor.Exit(_lock);
            }

            // When the process queue task completes, check to see if
            // the queue has been populated again and needs to restart.
            task.ContinueWith(t => TryStartProcessLoop());
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Enqueue item.
        /// </summary>
        public void Enqueue
            (
                T item
            )
        {
            if (_isDisposed)
            {
                throw new InvalidOperationException
                    (
                        "Cancellation has been requested"
                    );
            }

            if (_maxQueueSize.HasValue 
                && Queue.Count >= _maxQueueSize)
            {
                throw new InvalidOperationException("Queue is full");
            }

            Queue.Enqueue(item);
            TryStartProcessLoop();
        }

        /// <summary>
        /// Try enqueue item.
        /// </summary>
        public bool TryEnqueue
            (
                T item
            )
        {
            if (_isDisposed)
            {
                return false;
            }

            if (_maxQueueSize.HasValue
                && Queue.Count >= _maxQueueSize)
            {
                return false;
            }

            Queue.Enqueue(item);
            TryStartProcessLoop();

            return true;
        }

        #endregion


        #region IDisposable members

        /// <inheritdoc />
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (_disposeTimeoutMs > 0)
            {
                var tasks = _tasks.Where(t => t != null).ToArray();
                var allTask = Task.WhenAll(tasks);
                var delayTask = Task.Delay(_disposeTimeoutMs);
                Task.WaitAny(allTask, delayTask);
            }

            CancelSource.Cancel();
        }

        #endregion
    }
}

#endif
