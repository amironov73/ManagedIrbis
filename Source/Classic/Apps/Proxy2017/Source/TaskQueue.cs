// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TaskQueue.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Proxy2017
{
    /// <summary>
    /// Простой планировщик задач. Выполняет поставленные задачи последовательно,
    /// одну за другой.
    /// Например, чтобы записи в лог-файлах не налезали друг на друга.
    /// </summary>
    /// <typeparam name="T">Тип аргумента, передаваемого задаче.</typeparam>
    class TaskQueue<T>
    {
        class TaskInfo
        {
            public Action<T> Action;

            public T Data;
        }

        // ====================================================================

        private readonly object _syncRoot;

        private readonly Queue<TaskInfo> _queue;

        private readonly AutoResetEvent _event;

        private volatile bool _stop;

        private volatile bool _executing;

        // ====================================================================

        public TaskQueue()
        {
            _syncRoot = new object();
            _queue = new Queue<TaskInfo>();
            _event = new AutoResetEvent(false);
            _stop = false;

            Thread workingThread = new Thread
                (
                    WorkingThread
                )
            {
                Name = "WorkingThread",
                IsBackground = true
            };
            workingThread.Start();
        }

        // ====================================================================

        public bool IsExecuting
        {
            get { return _executing; }
        }

        // ====================================================================

        public bool IsBusy
        {
            get { return (_queue.Count != 0) || _executing; }
        }

        // ====================================================================

        public void Stop()
        {
            _stop = true;
        }

        // ====================================================================

        void WorkingThread()
        {
            while (!_stop)
            {
                if (_stop)
                {
                    break;
                }

                TaskInfo task = null;

                lock (_syncRoot)
                {
                    if (_queue.Count != 0)
                    {
                        task = _queue.Dequeue();
                    }
                }

                if (task == null)
                {
                    _event.WaitOne();
                }
                else
                {
                    try
                    {
                        try
                        {
                            _executing = true;
                            task.Action(task.Data);
                        }
                        finally
                        {
                            _executing = false;
                            _event.Set();
                        }
                    }
                    catch (Exception ex)
                    {
                        // IrbisProxy.WriteException(ex);
                        Debug.WriteLine(ex);
                    }
                }
            }
        }

        // ====================================================================

        public void Enqueue
            (
                Action<T> action,
                T data
            )
        {
            TaskInfo task = new TaskInfo
            {
                Action = action,
                Data = data
            };

            lock (_syncRoot)
            {
                _queue.Enqueue(task);
            }

            _event.Set();
        }
    }
}
