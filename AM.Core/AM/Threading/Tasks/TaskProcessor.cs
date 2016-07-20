/* TaskProcessor.cs -- 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Threading.Tasks
{
    /// <summary>
    /// Task processor.
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
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {
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

        #endregion

        #region Construction

        public TaskProcessor(int degree)
        {
            if (degree < 0)
            {
                throw new ArgumentOutOfRangeException("degree");
            }

            _queue = new BlockingCollection<Action>();
            _running = new List<ActionWrapper>();
            _semaphore = new Semaphore(degree, degree);

            Task.Factory.StartNew(_MainWorker);
        }

        #endregion

        #region Private members

        private readonly BlockingCollection<Action> _queue;
        private readonly List<ActionWrapper> _running;

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

            _queue.Add(action);
        }

        /// <summary>
        /// Wait for completion.
        /// </summary>
        public void WaitForCompletion()
        {
            while (!_queue.IsCompleted)
            {
                Thread.SpinWait(100000);
            }

            Task[] tasks;

            lock (_running)
            {
                tasks = _running
                    .Select(r => r.Task)
                    .ToArray();
            }

            Task.WaitAll(tasks);
        }

        #endregion
    }
}
