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
