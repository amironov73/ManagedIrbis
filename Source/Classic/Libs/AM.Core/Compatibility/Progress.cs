// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Progress.cs -- temporary solution for compatibility
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW35 || FW40 || WINMOBILE || POCKETPC

using CodeJam;

using JetBrains.Annotations;

namespace System
{
    /// <summary>Defines a provider for progress updates.</summary>
    /// <typeparam name="T">The type of progress update value.</typeparam>
    public interface IProgress<T>
    {
        /// <summary>Reports a progress update.</summary>
        /// <param name="value">The value of the updated progress.</param>
        void Report(T value);
    }

    /// <summary>
    /// Provides an <see cref="IProgress{T}"/> that invokes callbacks
    /// for each reported progress value.
    /// </summary>
    public class Progress<T>
        : IProgress<T>
    {
        #region Events

        // TODO 3.5 compatibility
        ///// <summary>
        ///// Raised for each reported progress value.
        ///// </summary>
        //public event EventHandler<T> ProgressChanged;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Progress()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Progress
            (
                [NotNull] Action<T> handler
            )
            : this()
        {
            Code.NotNull(handler, "handler");

            _handler = handler;
        }

        #endregion

        #region Private members

        private readonly Action<T> _handler;

        /// <summary>
        /// Reports a progress change.
        /// </summary>
        protected virtual void OnReport(T value)
        {
            Action<T> handler = _handler;
            if (!ReferenceEquals(handler, null))
            {
                handler(value);
            }

            // TODO 3.5 compatibility
            //EventHandler<T> eventHandler = ProgressChanged;
            //if (!ReferenceEquals(eventHandler, null))
            //{
            //    eventHandler.Invoke(this, value);
            //}
        }

        #endregion

        #region IProgress<T> members

        /// <inheritdoc cref="IProgress{T}.Report"/>
        void IProgress<T>.Report(T value)
        {
            OnReport(value);
        }

        #endregion
    }
}

#endif
