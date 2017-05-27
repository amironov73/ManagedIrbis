// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Reference.cs -- Generic reference to given object
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// Generic reference to given object. Allows 
    /// to track object changes.
    /// </summary>
    /// <typeparam name="T">Type of object to reference.
    /// </typeparam>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{Target}")]
    public class Reference<T>
    {
        #region Events

        /// <summary>
        /// Fired when target value changed.
        /// </summary>
        public event EventHandler TargetChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Access counter.
        /// </summary>
        public int Counter
        {
            get { return _counter; }
        }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        public T Target
        {
            [DebuggerStepThrough]
            get
            {
                ++_counter;

                return _target;
            }
            [DebuggerStepThrough]
            set
            {
                _target = value;
                OnTargetChanged();
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Reference()
        {
            _target = default(T);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Reference
            (
                T initialValue
            )
        {
            _target = initialValue;
        }

        #endregion

        #region Private members

        private int _counter;

        private T _target;

        #endregion

        #region Public methods

        /// <summary>
        /// Reset access counter.
        /// </summary>
        public int ResetCounter()
        {
            int result = _counter;

            _counter = 0;

            return result;
        }

        /// <summary>
        /// Implicit operators the specified reference.
        /// </summary>
        public static implicit operator T
            (
                Reference<T> reference
            )
        {
            return reference.Target;
        }

        /// <summary>
        /// Implicit operators the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public static implicit operator Reference<T>
            (
                T value
            )
        {
            return new Reference<T>(value);
        }

        #endregion

        #region Private members

        /// <summary>
        /// Fired when target value changed.
        /// </summary>
        protected virtual void OnTargetChanged()
        {
            EventHandler handler = TargetChanged;
            if (!ReferenceEquals(handler, null))
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return _target.ToVisibleString();
        }

        #endregion
    }
}
