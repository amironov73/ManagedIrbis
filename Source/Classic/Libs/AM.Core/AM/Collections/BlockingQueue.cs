// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BlockingQueue.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.Logging;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Borrowed from Tom DuPont:
    /// http://www.tomdupont.net/2012/06/net-blockingqueue.html
    /// </remarks>
    [PublicAPI]
    [MoonSharpUserData]
    public class BlockingQueue<T>
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Count.
        /// </summary>
        public int Count
        {
            get { return _collection.Count; }
        }

        /// <summary>
        /// Is canceled?
        /// </summary>
        public bool IsCanceled
        {
            get { return _tokenSource.IsCancellationRequested; }
        }

        /// <summary>
        /// Is completed?
        /// </summary>
        public bool IsCompleted
        {
            get { return _tasks.All(t => t.IsCompleted); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BlockingQueue
            (
                int threadCount
            )
        {
            _tokenSource = new CancellationTokenSource();

            var queue = new ConcurrentQueue<T>();
            _collection = new BlockingCollection<T>(queue);

            _tasks = new Task[threadCount];
            for (var i = 0; i < threadCount; i++)
            {
                _tasks[i] = Task.Factory.StartNew(_ProcessQueue);
            }
        }

        #endregion

        #region Private members

        private const int Timeout = 60000;

        private bool _disposed;
        
        private readonly CancellationTokenSource _tokenSource;
        
        private readonly BlockingCollection<T> _collection;
        
        private readonly Task[] _tasks;

        /// <summary>
        /// Handle the exception.
        /// </summary>
        protected virtual void HandleException
            (
                Exception exception
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Process the model.
        /// </summary>
        protected virtual void ProcessModel
            (
                T model
            )
        {
            // Nothing to do here
        }

        private void _ProcessQueue()
        {
            while (!IsCanceled)
            {
                try
                {
                    T model;
                    var result = _collection.TryTake
                        (
                            out model,
                            Timeout,
                            _tokenSource.Token
                        );

                    if (result
                        && !ReferenceEquals(model, null))
                    {
                        ProcessModel(model);
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Enqueue.
        /// </summary>
        public void Enqueue
            (
                T model
            )
        {
            if (IsCompleted)
            {
                Log.Error
                    (
                        "BlockingQueue::Enqueue: "
                        + "completed"
                    );

                throw new Exception("BlockingQueue has been Completed");
            }

            if (IsCanceled)
            {
                Log.Error
                    (
                        "BlockingQueue::Enqueue: "
                        + "canceled"
                    );

                throw new Exception("BlockingQueue has been Canceled");
            }

            _collection.Add(model);
        }

        /// <summary>
        /// Cancel.
        /// </summary>
        public void Cancel()
        {
            if (!IsCanceled)
            {
                _tokenSource.Cancel(false);
            }
        }

        /// <summary>
        /// Cancel and wait.
        /// </summary>
        public void CancelAndWait()
        {
            Cancel();
            Task.WaitAll(_tasks);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        private void Dispose
            (
                bool finalizing
            )
        {
            if (_disposed)
            {
                return;
            }

            Cancel();

            if (!finalizing)
            {
                GC.SuppressFinalize(this);
            }

            _disposed = true;
        }

        #endregion
    }
}
