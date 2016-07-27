/* RetryManager.cs -- retry execution of function
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Threading;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// Retry execution of function for specified number of times.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RetryManager
    {
        #region Properties

        /// <summary>
        /// Delay interval, milliseconds.
        /// </summary>
        public int DelayInterval { get; set; }

        /// <summary>
        /// Retry count.
        /// </summary>
        public int RetryCount { get { return _retryCount; } }

        #endregion

        #region Construction

        /// <summary>
        /// Construction.
        /// </summary>
        public RetryManager
            (
                int retryCount
            )
        {
            Code.Positive(retryCount, "retryCount");

            _retryCount = retryCount;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RetryManager
            (
                int retryCount,
                [NotNull] Func<Exception, bool> resolver
            )
        {
            Code.Positive(retryCount, "retryCount");
            Code.NotNull(resolver, "resolver");

            _retryCount = retryCount;
            _resolver = resolver;
        }

        #endregion

        #region Private members

        private readonly int _retryCount;

        private readonly Func<Exception, bool> _resolver;

        private void _Delay()
        {
            if (DelayInterval > 0)
            {
                Thread.Sleep(DelayInterval);
            }
        }

        private void _Resolve(Exception ex)
        {
            if (_resolver == null)
            {
                return;
            }

            bool result = _resolver(ex);
            if (!result)
            {
                throw new ArsMagnaException
                    (
                        "RetryManager failed",
                        ex
                    );
            }

            _Delay();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Try to execute specified function.
        /// </summary>
        public void Try
            (
                [NotNull] Action action
            )
        {
            Code.NotNull(action, "action");

            for (int i = 0; i <= RetryCount; i++)
            {
                try
                {
                    action();
                    break;
                }
                catch (Exception ex)
                {
                    _Resolve(ex);
                }
            }
        }

        /// <summary>
        /// Try to execute specified function.
        /// </summary>
        public void Try<T>
            (
                [NotNull] Action<T> action,
                T argument
            )
        {
            Code.NotNull(action, "action");

            for (int i = 0; i <= RetryCount; i++)
            {
                try
                {
                    action(argument);
                    return;
                }
                catch (Exception ex)
                {
                    _Resolve(ex);
                }
            }

            throw new ArsMagnaException("RetryManager failed");
        }

        /// <summary>
        /// Try to execute specified function.
        /// </summary>
        public void Try<T1,T2>
            (
                [NotNull] Action<T1,T2> action,
                T1 argument1,
                T2 argument2
            )
        {
            Code.NotNull(action, "action");

            for (int i = 0; i <= RetryCount; i++)
            {
                try
                {
                    action(argument1, argument2);
                    return;
                }
                catch (Exception ex)
                {
                    _Resolve(ex);
                }
            }

            throw new ArsMagnaException("RetryManager failed");
        }

        /// <summary>
        /// Try to execute specified function.
        /// </summary>
        public void Try<T1, T2, T3>
            (
                [NotNull] Action<T1, T2, T3> action,
                T1 argument1,
                T2 argument2,
                T3 argument3
            )
        {
            Code.NotNull(action, "action");

            for (int i = 0; i <= RetryCount; i++)
            {
                try
                {
                    action(argument1, argument2, argument3);
                    return;
                }
                catch (Exception ex)
                {
                    _Resolve(ex);
                }
            }

            throw new ArsMagnaException("RetryManager failed");
        }

        /// <summary>
        /// Try to execute specified function.
        /// </summary>
        public T Try<T>
            (
                [NotNull] Func<T> function
            )
        {
            Code.NotNull(function, "function");

            for (int i = 0; i <= RetryCount; i++)
            {
                try
                {
                    return function();
                }
                catch (Exception ex)
                {
                    _Resolve(ex);
                }
            }

            throw new ArsMagnaException("RetryManager failed");
        }

        /// <summary>
        /// Try to execute specified function.
        /// </summary>
        public TResult Try<T1,TResult>
            (
                [NotNull] Func<T1,TResult> function,
                T1 argument
            )
        {
            Code.NotNull(function, "function");

            for (int i = 0; i <= RetryCount; i++)
            {
                try
                {
                    return function(argument);
                }
                catch (Exception ex)
                {
                    _Resolve(ex);
                }
            }

            throw new ArsMagnaException("RetryManager failed");
        }

        /// <summary>
        /// Try to execute specified function.
        /// </summary>
        public TResult Try<T1, T2, TResult>
            (
                [NotNull] Func<T1, T2, TResult> function,
                T1 argument1,
                T2 argument2
            )
        {
            Code.NotNull(function, "function");

            for (int i = 0; i <= RetryCount; i++)
            {
                try
                {
                    return function(argument1, argument2);
                }
                catch (Exception ex)
                {
                    _Resolve(ex);
                }
            }

            throw new ArsMagnaException("RetryManager failed");
        }

        /// <summary>
        /// Try to execute specified function.
        /// </summary>
        public TResult Try<T1, T2, T3, TResult>
            (
                [NotNull] Func<T1, T2, T3, TResult> function,
                T1 argument1,
                T2 argument2,
                T3 argument3
            )
        {
            Code.NotNull(function, "function");

            for (int i = 0; i <= RetryCount; i++)
            {
                try
                {
                    return function(argument1, argument2, argument3);
                }
                catch (Exception ex)
                {
                    _Resolve(ex);
                }
            }

            throw new ArsMagnaException("RetryManager failed");
        }

        #endregion
    }
}
