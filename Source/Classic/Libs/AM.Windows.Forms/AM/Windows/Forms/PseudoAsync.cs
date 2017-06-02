// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PseudoAsync.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PseudoAsync
    {
        #region Constants

        /// <summary>
        /// Smoothness, milliseconds.
        /// </summary>
        public const int DefaultSmoothness = 20;

        #endregion

        #region Properties

        /// <summary>
        /// Smoothness, milliseconds.
        /// </summary>
        public static int Smoothness
        {
            get { return _smoothness; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _smoothness = value;
            }
        }

        #endregion

        #region Construction

        static PseudoAsync()
        {
            Smoothness = DefaultSmoothness;
        }

        #endregion

        #region Private members

        private static int _smoothness;

        private static void DiscoverExceptions
            (
                [NotNull] Task task
            )
        {
#if FW4
            task.GetAwaiter().GetResult();

#else

            // TODO implement

#endif
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Sleep for a short time.
        /// </summary>
        public static void SleepALittle()
        {
            Application.DoEvents();

            // ThreadUtility.Sleep(Smoothness);
            Thread.Sleep(Smoothness);
        }

        /// <summary>
        /// Run some code in pseudo-async manner.
        /// </summary>
        public static void Run
            (
                [NotNull] Action action
            )
        {
            Code.NotNull(action, "action");
            Log.Trace("PseudoAsync::Run: entering");

            using (Task task = Task.Factory.StartNew(action))
            {
                WaitFor(task);
                DiscoverExceptions(task);
            }

            Log.Trace("PseudoAsync::Run: leaving");
        }

        /// <summary>
        /// Run some code in pseudo-async manner.
        /// </summary>
        public static void Run<T>
            (
                [NotNull] Action<T> action,
                T argument
            )
        {
            Code.NotNull(action, "action");
            Log.Trace("PseudoAsync::Run: entering");

            Action interim = () => action(argument);
            using (Task task = Task.Factory.StartNew(interim))
            {
                WaitFor(task);
                DiscoverExceptions(task);
            }

            Log.Trace("PseudoAsync::Run: leaving");
        }

        /// <summary>
        /// Run some code in pseudo-async manner.
        /// </summary>
        public static void Run<T1, T2>
            (
                [NotNull] Action<T1, T2> action,
                T1 argument1,
                T2 argument2
            )
        {
            Code.NotNull(action, "action");
            Log.Trace("PseudoAsync::Run: entering");

            Action interim = () => action(argument1, argument2);
            using (Task task = Task.Factory.StartNew(interim))
            {
                WaitFor(task);
                DiscoverExceptions(task);
            }

            Log.Trace("PseudoAsync::Run: leaving");
        }

        /// <summary>
        /// Run some code in pseudo-async manner.
        /// </summary>
        public static void Run<T1, T2, T3>
            (
                [NotNull] Action<T1, T2, T3> action,
                T1 argument1,
                T2 argument2,
                T3 argument3
            )
        {
            Code.NotNull(action, "action");
            Log.Trace("PseudoAsync::Run: entering");

            Action interim = () => action(argument1, argument2, argument3);
            using (Task task = Task.Factory.StartNew(interim))
            {
                WaitFor(task);
                DiscoverExceptions(task);
            }

            Log.Trace("PseudoAsync::Run: leaving");
        }

        /// <summary>
        /// Run some code in pseudo-async manner.
        /// </summary>
        public static TResult Run<TResult>
            (
                [NotNull] Func<TResult> func
            )
        {
            Code.NotNull(func, "func");
            Log.Trace("PseudoAsync::Run: entering");

            TResult result;
            using (Task<TResult> task = Task<TResult>.Factory.StartNew(func))
            {
                WaitFor(task);
                result = task.Result;
            }

            Log.Trace("PseudoAsync::Run: leaving");

            return result;
        }

        /// <summary>
        /// Run some code in pseudo-async manner.
        /// </summary>
        public static TResult Run<T1, TResult>
            (
                [NotNull] Func<T1, TResult> func,
                T1 argument
            )
        {
            Code.NotNull(func, "func");
            Log.Trace("PseudoAsync::Run: entering");

            TResult result;
            Func<TResult> interim = () => func(argument);
            using (Task<TResult> task
                = Task<TResult>.Factory.StartNew(interim))
            {
                WaitFor(task);
                result = task.Result;
            }

            Log.Trace("PseudoAsync::Run: leaving");

            return result;
        }

        /// <summary>
        /// Run some code in pseudo-async manner.
        /// </summary>
        public static TResult Run<T1, T2, TResult>
            (
                [NotNull] Func<T1, T2, TResult> func,
                T1 argument1,
                T2 argument2
            )
        {
            Code.NotNull(func, "func");
            Log.Trace("PseudoAsync::Run: entering");

            TResult result;
            Func<TResult> interim = () => func(argument1, argument2);
            using (Task<TResult> task
                = Task<TResult>.Factory.StartNew(interim))
            {
                WaitFor(task);
                result = task.Result;
            }

            Log.Trace("PseudoAsync::Run: leaving");

            return result;
        }

        /// <summary>
        /// Wait for flag.
        /// </summary>
        public static void WaitFor
            (
                ref bool readyFlag
            )
        {
            Log.Trace("PseudoAsync::WaitFor: entering");

            while (!readyFlag)
            {
                SleepALittle();
            }

            Log.Trace("PseudoAsync::WaitFor: leaving");
        }

        /// <summary>
        /// Wait for flag.
        /// </summary>
        public static void WaitFor
            (
                [NotNull] Func<bool> readyCheck
            )
        {
            Code.NotNull(readyCheck, "readyCheck");

            Log.Trace("PseudoAsync::WaitFor: entering");


            while (!readyCheck())
            {
                SleepALittle();
            }

            Log.Trace("PseudoAsync::WaitFor: leaving");
        }

        /// <summary>
        /// Wait for flag.
        /// </summary>
        public static void WaitFor<T>
            (
                [NotNull] Func<T, bool> readyCheck,
                [NotNull] T argument
            )
            where T : class
        {
            Code.NotNull(readyCheck, "readyCheck");
            Code.NotNull(argument, "argument");

            Log.Trace("PseudoAsync::WaitFor: entering");

            while (!readyCheck(argument))
            {
                SleepALittle();
            }

            Log.Trace("PseudoAsync::WaitFor: leaving");
        }

        /// <summary>
        /// Wait for task.
        /// </summary>
        public static void WaitFor
            (
                [NotNull] IAsyncResult handle
            )
        {
            Code.NotNull(handle, "handle");

            Log.Trace("PseudoAsync::WaitFor: entering");

            while (!handle.IsCompleted)
            {
                SleepALittle();
            }

            Log.Trace("PseudoAsync::WaitFor: leaving");
        }

        /// <summary>
        /// Wait for some tasks.
        /// </summary>
        public static void WaitFor
            (
                bool waitAll,
                [NotNull] params WaitHandle[] handles
            )
        {
            Log.Trace("PseudoAsync::WaitFor: entering");

            bool complete;
            do
            {
                complete = waitAll
                    ? WaitHandle.WaitAll(handles, 0)
                    : WaitHandle.WaitAny(handles, 0) >= 0;

                if (!complete)
                {
                    SleepALittle();
                }
            }
            while (!complete);

            Log.Trace("PseudoAsync::WaitFor: leaving");
        }

        /// <summary>
        /// Wait for some tasks.
        /// </summary>
        public static void WaitFor
            (
                bool waitAll,
                [NotNull] params Task[] tasks
            )
        {
            Log.Trace("PseudoAsync::WaitFor: entering");

            bool complete;
            do
            {
                complete = waitAll
                    ? Task.WaitAll(tasks, 0)
                    : Task.WaitAny(tasks, 0) >= 0;

                if (!complete)
                {
                    SleepALittle();
                }
            }
            while (!complete);

            Log.Trace("PseudoAsync::WaitFor: leaving");
        }

        #endregion
    }
}
