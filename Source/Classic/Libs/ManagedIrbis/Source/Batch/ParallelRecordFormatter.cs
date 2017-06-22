// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ParallelRecordFormatter.cs -- formats records from the server in parallel threads
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW4 || ANDROID || UAP || NETCORE || PORTABLE

#region Using directives

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Logging;
using AM.Threading;
using AM.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Batch
{
    /// <summary>
    /// Formats records from the server in parallel threads.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ParallelRecordFormatter
        : IEnumerable<string>,
            IDisposable
    {
        #region Properties

        /// <summary>
        /// Степень параллелизма.
        /// </summary>
        public int Parallelism { get; private set; }

        /// <summary>
        /// Строка подключения.
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Признак окончания.
        /// </summary>
        public bool Stop { get { return _AllDone(); } }

        /// <summary>
        /// Используемый формат.
        /// </summary>
        public string Format { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ParallelRecordFormatter
            (
                int parallelism,
                [NotNull] string connectionString,
                [NotNull] int[] mfnList,
                [NotNull] string format
            )
        {
            Code.NotNullNorEmpty(connectionString, "connectionString");
            Code.NotNull(mfnList, "mfnList");
            Code.NotNullNorEmpty(format, "format");

            if (parallelism <= 0)
            {
                parallelism = EnvironmentUtility.OptimalParallelism;
            }

            Parallelism = parallelism;
            ConnectionString = connectionString;
            Format = format;
            Parallelism = Math.Min(mfnList.Length / 1000, parallelism);

            _Run(mfnList);
        }

        #endregion

        #region Private members

        private Task[] _tasks;

        private ConcurrentQueue<string> _queue;

        private AutoResetEvent _event;

        private object _lock;

        private int[] _GetMfnList
            (
                [NotNull] string connectionString
            )
        {
            using (IrbisConnection connection
                = new IrbisConnection(connectionString))
            {
                int maxMfn = connection.GetMaxMfn() - 1;
                if (maxMfn <= 0)
                {
                    throw new IrbisException("MaxMFN=0");
                }
                int[] result = Enumerable.Range(1, maxMfn).ToArray();

                return result;
            }
        }

        private void _Run
            (
                [NotNull] int[] mfnList
            )
        {
            _queue = new ConcurrentQueue<string>();
            _event = new AutoResetEvent(false);
            _lock = new object();

            _tasks = new Task[Parallelism];
            int[][] chunks = ArrayUtility.SplitArray
                (
                    mfnList,
                    Parallelism
                );
            for (int i = 0; i < Parallelism; i++)
            {
                Task task = new Task
                (
                    _Worker,
                    chunks[i]
                );
                _tasks[i] = task;
            }
            foreach (Task task in _tasks)
            {
                ThreadUtility.Sleep(50);
                task.Start();
            }
        }

        private void _Worker
            (
                [NotNull] object state
            )
        {
            int[] chunk = (int[])state;
            int first = chunk.GetItem(0, -1);
            int threadId = ThreadUtility.ThreadId;

            Log.Trace
                (
                    "ParallelRecordFormatter::_Worker: begin: "
                    + "first="
                    + first
                    + ", length="
                    + chunk.Length
                    + ", thread="
                    + threadId
                );

            using (IrbisConnection connection
                = new IrbisConnection(ConnectionString))
            {
                BatchRecordFormatter batch = new BatchRecordFormatter
                    (
                        connection,
                        connection.Database,
                        Format,
                        1000,
                        chunk
                    );
                foreach (string line in batch)
                {
                    _PutLine(line);
                }

            }
            _event.Set();

            Log.Trace
                (
                    "ParallelRecordFormatter::_Worker: end: "
                    + "first="
                    + first
                    + ", length="
                    + chunk.Length
                    + ", thread="
                    + threadId
                );
        }

        private void _PutLine
            (
                string line
            )
        {
            _queue.Enqueue(line);
            _event.Set();
        }

        private bool _AllDone()
        {
            return _queue.IsEmpty
                   && _tasks.All(t => t.IsCompleted);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Форматирование всех записей.
        /// </summary>
        [NotNull]
        public string[] FormatAll()
        {
            List<string> result = new List<string>();

            foreach (string line in this)
            {
                result.Add(line);
            }

            return result.ToArray();
        }

        #endregion

        #region IEnumerable<T> members

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<string> GetEnumerator()
        {
            while (true)
            {
                if (Stop)
                {
                    yield break;
                }

                string line;
                while (_queue.TryDequeue(out line))
                {
                    yield return line;
                }
                _event.Reset();

                _event.WaitOne(10);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            _event.Dispose();
            foreach (Task task in _tasks)
            {
                TaskUtility.DisposeTask(task);
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}

#endif
