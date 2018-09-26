// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TaskUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WINMOBILE

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Threading.Tasks
{
    /// <summary>
    /// Extensions for <see cref="Task"/> class.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class TaskUtility
    {
        #region Public methods

        /// <summary>
        /// ConfigureAwait(false).
        /// </summary>
        [NotNull]
        public static Task ConfigureSafe
            (
                [NotNull] this Task task
            )
        {
            Code.NotNull(task, "task");

#if FW45 || NETCORE || DROID || UAP

            task.ConfigureAwait(false);

#endif

            return task;
        }

        /// <summary>
        /// ConfigureAwait(false).
        /// </summary>
        [NotNull]
        public static Task<T> ConfigureSafe<T>
            (
                [NotNull] this Task<T> task
            )
        {
            Code.NotNull(task, "task");

#if FW45 || NETCORE || DROID || UAP

            task.ConfigureAwait(false);

#endif

            return task;
        }

        /// <summary>
        /// Dispose the task.
        /// </summary>
        public static void DisposeTask
            (
                [NotNull] Task task
            )
        {
            Code.NotNull(task, "task");

#if CLASSIC || ANDROID || NETCORE

            task.Dispose();

#endif
        }

        /// <summary>
        /// Dispose the task.
        /// </summary>
        public static void DisposeTask<T>
            (
                [NotNull] Task<T> task
            )
        {
            Code.NotNull(task, "task");

#if CLASSIC

            task.Dispose();

#endif
        }

#if FW45 || NETCORE

        ///// <summary>
        ///// Borrowed from Stephen Toub book.
        ///// </summary>
        //public static async Task<T> RetryOnFault<T>
        //    (
        //        Func<Task<T>> function,
        //        int maxTries
        //    )
        //{
        //    for (int i = 0; i < maxTries; i++)
        //    {
        //        try
        //        {
        //            return await function().ConfigureAwait(false);
        //        }
        //        catch
        //        {
        //            if (i == maxTries - 1)
        //            {
        //                Log.Error
        //                    (
        //                        "TaskUtility::RetryOnFault"
        //                    );

        //                throw;
        //            }
        //        }
        //    }

        //    return default(T);
        //}

        /// <summary>
        /// Waits for the task to complete, unwrapping any exceptions.
        /// </summary>
        /// <param name="task">The task. May not be <c>null</c>.</param>
        public static void WaitAndUnwrapException(this Task task)
        {
            task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Waits for the task to complete, unwrapping any exceptions.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the task.</typeparam>
        /// <param name="task">The task. May not be <c>null</c>.</param>
        /// <returns>The result of the task.</returns>
        public static TResult WaitAndUnwrapException<TResult>(this Task<TResult> task)
        {
            return task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Waits for the task to complete, but does not raise task exceptions. The task exception (if any) is unobserved.
        /// </summary>
        /// <param name="task">The task. May not be <c>null</c>.</param>
        public static void WaitWithoutException(this Task task)
        {
            try
            {
                task.Wait();
            }
            catch (AggregateException)
            {
            }
        }

        /// <summary>
        /// Waits for the task to complete, but does not raise task exceptions. The task exception (if any) is unobserved.
        /// </summary>
        /// <param name="task">The task. May not be <c>null</c>.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <exception cref="OperationCanceledException">The <paramref name="cancellationToken"/> was cancelled before the <paramref name="task"/> completed.</exception>
        public static void WaitWithoutException(this Task task, CancellationToken cancellationToken)
        {
            try
            {
                task.Wait(cancellationToken);
            }
            catch (AggregateException)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

#endif

        #endregion
    }
}

#endif
