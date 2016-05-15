/* TaskUtility.cs -- 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
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
#if FW45

    /// <summary>
    /// Extensions for <see cref="Task"/> class.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class TaskUtility
    {
        #region Public methods

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

        #endregion
    }

#endif
}
