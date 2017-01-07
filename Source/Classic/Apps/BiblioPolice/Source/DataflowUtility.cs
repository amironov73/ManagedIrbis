// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DataflowUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

using AM;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace BiblioPolice
{
    [PublicAPI]
    public static class DataflowUtility
    {
        #region Public methods

        /// <summary>
        /// Get block execution options.
        /// </summary>
        public static ExecutionDataflowBlockOptions GetExecutionOptions()
        {
            ExecutionDataflowBlockOptions result
                = new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism =
                    //EnvironmentUtility.OptimalParallelism
                    4
                };

            return result;
        }

        /// <summary>
        /// Process the data.
        /// </summary>
        public static void ProcessData<T>
            (
                [NotNull] this IEnumerable<T> data,
                [NotNull] ActionBlock<T> actionBlock
            )
        {
            Code.NotNull(data, "data");
            Code.NotNull(actionBlock, "actionBlock");

            foreach (T item in data)
            {
                actionBlock.Post(item);
                //actionBlock.SendAsync(item);
            }

            actionBlock.Complete();
            actionBlock.Completion.Wait();
        }

        /// <summary>
        /// Process the data.
        /// </summary>
        public static void ProcessData<T>
            (
                [NotNull] this IEnumerable<T> data,
                [NotNull] Action<T> action
            )
        {
            Code.NotNull(data, "data");
            Code.NotNull(action, "action");

            DataflowLinkOptions linkOptions
                = new DataflowLinkOptions
                {
                    PropagateCompletion = true
                };

            ExecutionDataflowBlockOptions executionOptions
                = GetExecutionOptions();

            BufferBlock<T> bufferBlock = new BufferBlock<T>();

            ActionBlock<T> actionBlock = new ActionBlock<T>
                (
                    action,
                    executionOptions
                );

            bufferBlock.LinkTo(actionBlock, linkOptions);

            foreach (T item in data)
            {
                bufferBlock.Post(item);
            }

            bufferBlock.Complete();
            actionBlock.Completion.Wait();
        }

        /// <summary>
        /// Process the data.
        /// </summary>
        public static void ProcessData<T1,T2>
            (
                [NotNull] this IEnumerable<T1> data,
                [NotNull] TransformBlock<T1,T2> transformBlock,
                [NotNull] ActionBlock<T2> actionBlock
            )
        {
            Code.NotNull(data, "data");
            Code.NotNull(transformBlock, "transformBlock");
            Code.NotNull(actionBlock, "actionBlock");

            DataflowLinkOptions options = new DataflowLinkOptions
            {
                PropagateCompletion = true
            };

            transformBlock.LinkTo(actionBlock, options);

            foreach (T1 item in data)
            {
                transformBlock.Post(item);
            }

            transformBlock.Complete();
            actionBlock.Completion.Wait();
        }

        /// <summary>
        /// Process the data.
        /// </summary>
        public static void ProcessData<T1, T2>
            (
                [NotNull] this IEnumerable<T1> data,
                [NotNull] Func<T1, T2> transform,
                [NotNull] Action<T2> action
            )
        {
            Code.NotNull(data, "data");
            Code.NotNull(transform, "transform");
            Code.NotNull(action, "action");

            ExecutionDataflowBlockOptions options
                = GetExecutionOptions();

            TransformBlock<T1,T2> transformBlock 
                = new TransformBlock<T1, T2>
                (
                    transform,
                    options
                );

            ActionBlock<T2> actionBlock
                = new ActionBlock<T2>
                (
                    action,
                    options
                );

            ProcessData
                (
                    data,
                    transformBlock,
                    actionBlock
                );
        }

        /// <summary>
        /// Process the data.
        /// </summary>
        [NotNull]
        public static T2[] ProcessData<T1, T2>
            (
                [NotNull] this IEnumerable<T1> data,
                [NotNull] TransformBlock<T1, T2> transformBlock
            )
        {
            Code.NotNull(data, "data");
            Code.NotNull(transformBlock, "transformBlock");

            ExecutionDataflowBlockOptions options
                = GetExecutionOptions();

            BlockingCollection<T2> result 
                = new BlockingCollection<T2>();

            ActionBlock<T2> actionBlock = new ActionBlock<T2>
                (
                    item => result.Add(item),
                    options
                );

            ProcessData
                (
                    data,
                    transformBlock,
                    actionBlock
                );

            result.CompleteAdding();

            return result.ToArray();
        }

        /// <summary>
        /// Process the data.
        /// </summary>
        [NotNull]
        public static T2[] ProcessData<T1, T2>
            (
                [NotNull] this IEnumerable<T1> data,
                [NotNull] Func<T1, T2> transform
            )
        {
            Code.NotNull(data, "data");
            Code.NotNull(transform, "transform");

            BlockingCollection<T2> result
                = new BlockingCollection<T2>();

            ExecutionDataflowBlockOptions options
                = GetExecutionOptions();

            TransformBlock<T1,T2> transformBlock
                = new TransformBlock<T1, T2>
                    (
                        transform,
                        options
                    );

            ActionBlock<T2> actionBlock = new ActionBlock<T2>
                (
                    item => result.Add(item),
                    options
                );

            ProcessData
                (
                    data,
                    transformBlock,
                    actionBlock
                );

            result.CompleteAdding();

            return result.ToArray();
        }

        #endregion
    }
}
