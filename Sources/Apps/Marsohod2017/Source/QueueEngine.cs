/* QueueEngine.cs -- многопоточная очередь заданий.
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading;

#endregion

namespace Marsohod2017
{

    /// <summary>
    /// Многопоточная очередь заданий.
    /// </summary>
    /// <typeparam name="T">Тип данных для обработки.</typeparam>
    public sealed class QueueEngine<T>
        : IDisposable
    {
        #region Nested classes

        /// <summary>
        /// Информация о задании.
        /// </summary>
        sealed class TaskInfo
        {
            #region Properties

            /// <summary>
            /// Делегат - выполняемые действия.
            /// </summary>
            public Action <T> Action;

            /// <summary>
            /// Данные для обработки.
            /// </summary>
            public T Data;

            #endregion
        }

        #endregion

        #region Events

        public event EventHandler Waiting;

        #endregion

        #region Properties

        /// <summary>
        /// Максимальное количество одновременно работающих потоков
        /// (не считая поток диспетчера).
        /// </summary>
        public int MaxWorkers { get { return _maxWorkers; } }

        /// <summary>
        /// Количество активных рабочих потоков в настоящий момент.
        /// </summary>
        public int WorkersActive { get { return _workersActive; } }

        /// <summary>
        /// Длина очереди заданий, ожидающих обработки.
        /// </summary>
        public int QueueLength
        {
            get
            {
                lock ( _syncRoot )
                {
                    return _inbox.Count;
                }
            }
        }

        /// <summary>
        /// Максимальная длина очереди заданий.
        /// </summary>
        public int MaxQueueLength { get { return _maxQueueLength; } }

        /// <summary>
        /// Признак остановки: при true прекращается
        /// постановка новых заданий в очередь.
        /// </summary>
        public bool StopFlag { get { return _stopFlag; } }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public QueueEngine()
            : this
            (
                Environment.ProcessorCount,
                int.MaxValue
            )
        {
        }

        /// <summary>
        /// Параметризованный конструктор.
        /// </summary>
        /// <param name="maxWorkers">Максимальное число рабочих потоков.</param>
        /// <param name="maxQueueLength">Максимальная длина очереди.</param>
        public QueueEngine
            (
                int maxWorkers,
                int maxQueueLength
            )
        {
            if (maxWorkers < 1)
            {
                throw new ArgumentOutOfRangeException("maxWorkers");
            }
            if ( maxQueueLength < 1 )
            {
                throw new ArgumentOutOfRangeException("maxQueueLength");
            }

            _maxWorkers = maxWorkers;
            _workersActive = 0;

            _syncRoot = new object();
            _inbox = new Queue<TaskInfo>();
            _maxQueueLength = maxQueueLength;
            _flagEvent = new AutoResetEvent(false);

            _dispatchThread = new Thread(_DispatcherMethod)
                              {
                                  Name = "Dispatch",
                                  IsBackground = true
                              };
            _dispatchThread.Start();
        }

        #endregion

        #region Private members

        private readonly int _maxWorkers;

        private volatile int _workersActive;

        private readonly object _syncRoot;

        private readonly Queue<TaskInfo> _inbox;

        private readonly int _maxQueueLength;

        /// <summary>
        /// Событие, сигнализирующее о: добавлении новых заданий в очередь,
        /// окончании обработки задания, установке флага останова.
        /// </summary>
        private readonly AutoResetEvent _flagEvent;

        private volatile bool _stopFlag;

        /// <summary>
        /// Поток-диспетчер. Берет данные из очереди
        /// и запускает рабочие потоки.
        /// </summary>
        private readonly Thread _dispatchThread;

        /// <summary>
        /// Диспетчеризация заданий.
        /// </summary>
        private void _DispatcherMethod()
        {
            while (true)
            {
                if (StopFlag)
                {
                    lock (_syncRoot)
                    {
                        // Если очередь заданий пуста, то
                        // по выставлению флага диспетчер может
                        // просто завершиться
                        if (_inbox.Count == 0)
                        {
                            return;
                        }
                    }
                }
                _flagEvent.WaitOne(100);
                if (StopFlag)
                {
                    lock (_syncRoot)
                    {
                        // Если очередь заданий пуста, то
                        // по выставлению флага диспетчер может
                        // просто завершиться
                        if (_inbox.Count == 0)
                        {
                            return;
                        }
                    }
                }

                lock (_syncRoot)
                {
                    if (WorkersActive >= MaxWorkers)
                    {
                        continue;
                    }

                    if (_inbox.Count != 0)
                    {
                        // Если есть задания в очереди, берем первое
                        // и создаем рабочий поток
                        TaskInfo taskInfo = _inbox.Dequeue();
                        Thread worker = new Thread
                            (
                                _WorkerMethod
                            )
                            {
                                Name = "Worker",
                                IsBackground = true
                            };
                        _workersActive++;
                        worker.Start(taskInfo);
                    }

                    if ((_inbox.Count != 0)
                        && (WorkersActive < MaxWorkers)
                        && !StopFlag)
                    {
                        // Если очередь не пустая и число рабочих
                        // потоков не достигло максимума, сигнализируем
                        // сами себе о возможности диспетчеризации
                        // задания
                        _flagEvent.Set();
                    }
                }
            }
        }

        /// <summary>
        /// Выполнение заданий.
        /// </summary>
        /// <param name="arg">Данные для обработки.</param>
        private void _WorkerMethod
            (
                object arg
            )
        {
            try
            {
                TaskInfo taskInfo = (TaskInfo) arg;
                taskInfo.Action ( taskInfo.Data );
            }
            finally
            {
                lock ( _syncRoot )
                {
                    if (_workersActive > 0)
                    {
                        _workersActive--;
                    }
                    _flagEvent.Set ();
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Постановка задания в очередь.
        /// </summary>
        /// <param name="action">Выполняемое действие</param>
        /// <param name="data">Данные для обработки</param>
        public void QueueTask
            (
                Action<T> action,
                T data
            )
        {
            if ( action == null )
            {
                throw new ArgumentNullException("action");
            }
            if (StopFlag)
            {
                throw new ApplicationException("Queue stopped");
            }

            if ( QueueLength >= MaxQueueLength )
            {
                // Если количество рабочих потоков велико,
                // ожидаем, когда они завершатся и появится вакансия
                while ( true )
                {
                    _flagEvent.WaitOne (100);
                    if ( QueueLength < MaxQueueLength )
                    {
                        break;
                    }
                }
            } // if

            TaskInfo taskInfo = new TaskInfo
            {
                Action = action,
                Data = data
            };

            lock (_syncRoot)
            {
                _inbox.Enqueue(taskInfo);

                // Сигнализируем диспетчеру об изменениях в очереди заданий
                _flagEvent.Set();
            }
        }

        /// <summary>
        /// Сигнал к прекращению принятия новых заданий.
        /// </summary>
        public void StopWorking()
        {
            _stopFlag = true;

            // Сигнализируем диспетчеру, что он может завершаться.
            _flagEvent.Set();
        }

        /// <summary>
        /// Принудительная очистка очереди заданий,
        /// т. е. полная отмена запланированных заданий
        /// (уже выполняющиеся задания не будут затронуты)
        /// </summary>
        public void ClearQueue()
        {
            lock (_syncRoot)
            {
                _inbox.Clear();

                // Сигнализируем диспетчеру
                _flagEvent.Set();
            }
        }

        /// <summary>
        /// Ожидание, пока все запланированные задания завершатся.
        /// </summary>
        public void WaitForComplete()
        {
            // Прекращаем прием новых заданий
            StopWorking();
            
            // Ожидаем окончания работы диспетчера
            _dispatchThread.Join();

            while (true)
            {
                EventHandler handler = Waiting;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }

                lock (_syncRoot)
                {
                    if (WorkersActive <= 0)
                    {
                        return;
                    }
                }

                // Освобождаем процессор, ожидая завершения заданий.
                _flagEvent.WaitOne();
            }
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            WaitForComplete();
        }

        #endregion
    }
}
