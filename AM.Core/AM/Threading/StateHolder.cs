/* StateHolder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Threading
{
    /// <summary>
    /// Контейнер для хранения значения и отслеживания его изменений.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{Value}")]
    public sealed class StateHolder<T>
        where T: IEquatable<T>
    {
        #region Events

        /// <summary>
        /// Вызывается при изменении значения.
        /// </summary>
        public event EventHandler ValueChanged;

        #endregion

        #region Properties

        public bool AllowNull { get; set; }

        /// <summary>
        /// Хэндл для ожидания изменения значения.
        /// </summary>
        [NotNull]
        public WaitHandle WaitHandle { get { return _waitHandle; } }

        public T Value
        {
            get { return _value; }
            set { SetValue(value); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StateHolder()
        {
            AllowNull = true;
            _waitHandle = new AutoResetEvent(false);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StateHolder(T value)
            : this()
        {
            _value = value;
        }

        #endregion

        #region Private members

        private T _value;

        private readonly AutoResetEvent _waitHandle;

        #endregion

        #region Public methods

        /// <summary>
        /// Установка нового значения
        /// </summary>
        public void SetValue
            (
                [CanBeNull] T newValue
            )
        {
            if (!AllowNull && ReferenceEquals(newValue, null))
            {
                throw new ArgumentNullException();
            }

            bool changed = false;

            if (ReferenceEquals(_value, null))
            {
                if (!ReferenceEquals(newValue, null))
                {
                    changed = true;
                }
            }
            else
            {
                if (!_value.Equals(newValue))
                {
                    changed = true;
                }
            }

            _value = newValue;

            if (changed)
            {
                _waitHandle.Set();
                ValueChanged.Raise(this);
            }
        }

        public static implicit operator T
            (
                [NotNull] StateHolder<T> holder
            )
        {
            return holder.Value;
        }

        public static implicit operator StateHolder<T>
            (
                T value
            )
        {
            return new StateHolder<T>(value);
        }

        #endregion

        #region Object members

        #endregion
    }
}
