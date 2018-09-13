// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EventLoop.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Threading
{
    /// <summary>
    /// Event loop.
    /// </summary>
    /// <remarks>
    /// Inspired by https://docs.python.org/3/library/asyncio-eventloop.html
    /// </remarks>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class EventLoop
    {
        #region Properties

        /// <summary>
        /// Whether the loop is running?
        /// </summary>
        public bool IsRunning
        {
            get
            {
                // TODO implement

                return false;
            }
        }

        #endregion

        #region Private members

        /// <summary>
        /// Factory
        /// </summary>
        private static Func<EventLoop> _factory;

        #endregion

        #region Public methods

        /// <summary>
        /// Call the callback as soon as possible.
        /// </summary>
        public void CallSoon<T>
            (
                [NotNull] EventLoopCallback<T> callback,
                T argument
            )
        {
            Code.NotNull(callback, "callback");

            // TODO implement
        }

        /// <summary>
        /// Close the event loop.
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// Get global event loop.
        /// </summary>
        [NotNull]
        public static EventLoop GetEventLoop()
        {
            EventLoop result = _factory.ThrowIfNull("Factory")()
                .ThrowIfNull("result");

            return result;
        }

        /// <summary>
        /// Idle state.
        /// </summary>
        public abstract void Idle();

        /// <summary>
        /// Run until <see cref="Stop"/> is called.
        /// </summary>
        public void RunForever()
        {
            // TODO implement
        }

        /// <summary>
        /// Set global event loop.
        /// </summary>
        public static void SetEventLoop
            (
                [NotNull] Func<EventLoop> factory
            )
        {
            Code.NotNull(factory, "factory");

            _factory = factory;
        }

        /// <summary>
        /// Stop.
        /// </summary>
        public void Stop()
        {
            // TODO implement
        }

        #endregion
    }
}
