// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TaskProcessor.cs -- simplest task processor
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !SILVERLIGHT

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AM.Collections;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Threading.Tasks
{
    /// <summary>
    /// Simplest task processor.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class TaskProcessor
    {
        #region Nested classes

        class ActionWrapper
        {
            public Task Task { get; set; }

            public Action Action { get; set; }

            public TaskProcessor Processor { get; set; }

            public void Worker()
            {
                try
                {
                    Action();
                }
                catch (Exception ex)
                {
                    lock (Processor._exceptions)
                    {
                        Processor._exceptions.Add(ex);
                    }
                }

                lock (Processor._running)
                {
                    Processor._running.Remove(this);
                }

                Semaphore semaphore = Processor._semaphore;
                semaphore.Release();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Exceptions.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public NonNullCollection<Exception> Exceptions
        {
            get { return _exceptions; }
        }

        /// <summary>
        /// Have errors?
        /// </summary>
        public bool HaveErrors
        {
            get { return _exceptions.Count != 0; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TaskProcessor
            (
                int degree
            )
        {
            Log.Trace
                (
                    "TaskProcessor::Constructor"
                );

            if (degree < 0)
            {
                Log.Error
                    (
                        "TaskProcessor::Constructor: "
                        + "degree="
                        + degree
                    );

                throw new ArgumentOutOfRangeException("degree");
            }

            _queue = new BlockingCollection<Action>();
            _running = new NonNullCollection<ActionWrapper>();
            _exceptions = new NonNullCollection<Exception>();
            _semaphore = new Semaphore(degree, degree);

            Task.Factory.StartNew(_MainWorker);
        }

        #endregion

        #region Private members

        private readonly BlockingCollection<Action> _queue;
        private readonly NonNullCollection<ActionWrapper> _running;
        private readonly NonNullCollection<Exception> _exceptions;

        private readonly Semaphore _semaphore;

        private void _MainWorker()
        {
            while (!_queue.IsCompleted)
            {
                _semaphore.WaitOne();

                Action action;
                if (!_queue.TryTake(out action))
                {
                    continue;
                }

                ActionWrapper wrapper = new ActionWrapper
                {
                    Action = action,
                    Processor = this
                };
                Task task = new Task(wrapper.Worker);
                wrapper.Task = task;
                lock (_running)
                {
                    _running.Add(wrapper);
                }
                wrapper.Task.Start();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        public void Complete()
        {
            Log.Trace
                (
                    "TaskProcessor::Complete"
                );

            _queue.CompleteAdding();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Enqueue
            (
                [NotNull] Action action
            )
        {
            Code.NotNull(action, "action");

            Log.Trace
                (
                    "TaskProcessor::Enqueue"
                );

            _queue.Add(action);
        }

        /// <summary>
        /// Wait for completion.
        /// </summary>
        public void WaitForCompletion()
        {
            Log.Trace
                (
                    "TaskProcessor::WaitForCompletion: "
                    + "begin1"
                );

            while (!_queue.IsCompleted)
            {
#if UAP

                Task.Delay(TimeSpan.FromSeconds(30)).Wait();

#else

                Thread.SpinWait(100000);

#endif
            }

                Log.Trace
                    (
                        "TaskProcessor::WaitForCompletion: "
                        + "begin2"
                    );

            Task[] tasks;

            lock (_running)
            {
                tasks = _running
                    .Select(r => r.Task)
                    .ToArray();
            }

            Task.WaitAll(tasks);

            Log.Trace
            (
                "TaskProcessor::WaitForCompletion: "
                + "end"
            );
        }

        #endregion
    }
}

#endif
