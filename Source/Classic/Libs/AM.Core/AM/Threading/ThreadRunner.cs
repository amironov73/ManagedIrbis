/* ThreadRunner.cs -- runs method in new thread
 * Ars Magna project, http://arsmagna.ru
 */

#if CLASSIC

#region Using directives

using System;
using System.Threading;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Threading
{
    /// <summary>
    /// Runs specified method (delegate) in new <see cref="Thread"/>.
    /// </summary>
    public class ThreadRunner
    {
        #region Construction

        /// <summary>
        /// Don't allow somebody to create instance.
        /// </summary>
        private ThreadRunner()
        {
            // Nothing to do.
        }

        #endregion

        #region Private members

        private object[] _parameters;

        private ThreadMethod _method;

        private void _RunMethod()
        {
            _method(_parameters);
        }

        private Delegate _delegate;

        private void _RunDelegate()
        {
            _delegate.DynamicInvoke(_parameters);
        }

        private void _MethodCallback(object notUsed)
        {
            _method(_parameters);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        public static Thread RunThread
            (
                [NotNull] ThreadMethod method,
                params object[] parameters
            )
        {
            Code.NotNull(method, "method");

            ThreadRunner runner = new ThreadRunner
            {
                _method = method,
                _parameters = parameters
            };
            ThreadStart start = runner._RunMethod;
            Thread result = new Thread(start)
            {
                IsBackground = true
            };
            result.Start();

            return result;
        }

        /// <summary>
        /// Run the method on background.
        /// </summary>
        public static Thread RunThread
            (
                [NotNull] Delegate method,
                params object[] parameters
            )
        {
            Code.NotNull(method, "method");

            ThreadRunner runner = new ThreadRunner
            {
                _delegate = method,
                _parameters = parameters
            };
            ThreadStart start = runner._RunDelegate;
            Thread result = new Thread(start)
            {
                IsBackground = true
            };
            result.Start(parameters);

            return result;
        }

        /// <summary>
        /// Run the method on the system thread pool.
        /// </summary>
        public static void RunOnPool
            (
                [NotNull] ThreadMethod method,
                params object[] parameters
            )
        {
            Code.NotNull(method, "method");

            ThreadRunner runner = new ThreadRunner
            {
                _method = method,
                _parameters = parameters
            };

            ThreadPool.QueueUserWorkItem
                (
                    runner._MethodCallback
                );
        }

        #endregion
    }
}

#endif
