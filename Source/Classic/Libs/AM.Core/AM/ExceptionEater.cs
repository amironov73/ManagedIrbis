// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExceptionEater.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM
{
    /// <summary>
    /// Eats all the exceptions.
    /// </summary>
    [PublicAPI]
    public sealed class ExceptionEater<T>
        : IEnumerable<T>
    {
        #region Events

        /// <summary>
        /// Raised when exception occurs.
        /// </summary>
        public event EventHandler ExceptionOccurs;

        #endregion

        #region Properties

        /// <summary>
        /// Inner enumerable object.
        /// </summary>
        [NotNull]
        public IEnumerable<T> Inner { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExceptionEater
            (
                [NotNull] IEnumerable<T> inner
            )
        {
            Code.NotNull(inner, "inner");

            Inner = inner;
        }

        #endregion

        #region Private members

        private void OnException
            (
                Exception exception
            )
        {
            EventHandler handler = ExceptionOccurs;

            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
        }

        private bool _MoveNext
            (
                [NotNull] IEnumerator<T> enumerator
            )
        {
            bool result = false;

            try
            {
                result = enumerator.MoveNext();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                OnException(exception);
            }

            return result;
        }

        private T _Current(IEnumerator<T> enumerator, out bool success)
        {
            T result = default(T);
            success = false;

            try
            {
                result = enumerator.Current;
                success = true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                OnException(exception);
            }

            return result;
        }

        #endregion

        #region IEnumerable<T> members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerator<T> GetEnumerator()
        {
            IEnumerator<T> inner = Inner.GetEnumerator();

            while (true)
            {
                if (!_MoveNext(inner))
                {
                    break;
                }

                bool success;
                T item = _Current(inner, out success);
                if (success)
                {
                    yield return item;
                }
            }

            inner.Dispose();
        }

        #endregion
    }
}
