// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ParallelRecordReader.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW4 || NETCORE

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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Batch
{
    /// <summary>
    /// Reads records from the server in parallel threads.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ParallelRecordReader
        : IEnumerable<MarcRecord>,
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
        [CanBeNull]
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Признак окончания.
        /// </summary>
        public bool Stop { get { return _AllDone(); } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ParallelRecordReader()
            : this(-1)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ParallelRecordReader
            (
                int parallelism
            )
            : this
                (
                    -1,
                    IrbisConnectionUtility.GetStandardConnectionString()
                )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ParallelRecordReader
            (
                int parallelism,
                [NotNull] string connectionString
            )
            : this
                (
                    -1,
                    connectionString,
                    _GetMfnList(connectionString)
                )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ParallelRecordReader
            (
                int parallelism,
                [NotNull] string connectionString,
                [NotNull] int[] mfnList
            )
        {
            Code.NotNullNorEmpty(connectionString, "connectionString");
            Code.NotNull(mfnList, "mfnList");

            if (parallelism <= 0)
            {
                parallelism = EnvironmentUtility.OptimalParallelism;
            }

            _Run
            (
                parallelism,
                connectionString,
                mfnList
            );
        }

        #endregion

        #region Private members

        private Task[] _tasks;

        private ConcurrentQueue<MarcRecord> _queue;

        private AutoResetEvent _event;

        private object _lock;

        private static int[] _GetMfnList
            (
                [NotNull] string connectionString
            )
        {
            Code.NotNullNorEmpty(connectionString, "connectionString");

            using (IrbisConnection client
                = new IrbisConnection(connectionString))
            {
                int maxMfn = client.GetMaxMfn() - 1;
                if (maxMfn <= 0)
                {
                    throw new ApplicationException("MaxMFN=0");
                }
                int[] result = Enumerable.Range(1, maxMfn).ToArray();

                return result;
            }
        }

        private void _Run
            (
                int parallelism,
                [NotNull] string connectionString,
                [NotNull] int[] mfnList
            )
        {
            ConnectionString = connectionString;
            parallelism = Math.Min(mfnList.Length / 1000, parallelism);
            Parallelism = parallelism;

            _queue = new ConcurrentQueue<MarcRecord>();
            _event = new AutoResetEvent(false);
            _lock = new object();

            _tasks = new Task[parallelism];
            int[][] chunks = ArrayUtility.SplitArray
                (
                    mfnList,
                    parallelism
                );
            for (int i = 0; i < parallelism; i++)
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
                Thread.Sleep(50);
                task.Start();
            }
        }

        private void _Worker
            (
                [NotNull] object state
            )
        {
            Log.Trace
                (
                    "ParallelRecordReader::_Worker: begin"
                );

            int[] chunk = (int[])state;

            using (IrbisConnection client 
                = new IrbisConnection(ConnectionString.ThrowIfNull()))
            {
                BatchRecordReader batch = new BatchRecordReader
                    (
                        client,
                        client.Database,
                        1000,
                        chunk
                    );
                foreach (MarcRecord record in batch)
                {
                    _PutRecord(record);
                }
            }
            _event.Set();

            Log.Trace
                (
                    "ParallelRecordReader::_Worker: end"
                );
        }

        private void _PutRecord
            (
                MarcRecord record
            )
        {
            _queue.Enqueue(record);
            _event.Set();
        }

        private bool _AllDone()
        {
            return _queue.IsEmpty
                   && _tasks.All(t => t.IsCompleted);
        }

        #endregion

        #region Public methods

        #endregion

        #region IEnumerable<T> members

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<MarcRecord> GetEnumerator()
        {
            while (true)
            {
                if (Stop)
                {
                    yield break;
                }

                MarcRecord record;
                while (_queue.TryDequeue(out record))
                {
                    yield return record;
                }
                _event.Reset();

                _event.WaitOne(10);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Read all records.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public MarcRecord[] ReadAll()
        {
            List<MarcRecord> result = new List<MarcRecord>();

            foreach (MarcRecord record in this)
            {
                result.Add(record);
            }

            return result.ToArray();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            _event.Dispose();
            foreach (Task task in _tasks)
            {
                task.Dispose();
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}

#endif
