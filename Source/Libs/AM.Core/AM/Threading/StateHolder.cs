/* StateHolder.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
//using System.Threading.Tasks;

using AM.IO;
using AM.Runtime;

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
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("{Value}")]
#endif
    public sealed class StateHolder<T>
        : IHandmadeSerializable
        where T: IEquatable<T>
    {
        #region Events

        /// <summary>
        /// Вызывается при изменении значения.
        /// </summary>
        public event EventHandler ValueChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Allow <c>null</c> values?
        /// </summary>
        public bool AllowNull { get; set; }

        /// <summary>
        /// Хэндл для ожидания изменения значения.
        /// </summary>
        [NotNull]
        public WaitHandle WaitHandle { get { return _waitHandle; } }

        /// <summary>
        /// Value itself.
        /// </summary>
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
            _lock = new object();
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

        private object _lock;

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
            lock (_lock)
            {

                if (!AllowNull &&
                    ReferenceEquals(newValue, null))
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
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        public static implicit operator T
            (
                [NotNull] StateHolder<T> holder
            )
        {
            return holder.Value;
        }

        /// <summary>
        /// Implicit conversion operator.
        /// </summary>
        public static implicit operator StateHolder<T>
            (
                T value
            )
        {
            return new StateHolder<T>(value);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            bool flag = reader.ReadBoolean();

            if (flag)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            if (ReferenceEquals(_value, null))
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);

                IHandmadeSerializable intf = _value as IHandmadeSerializable;

                intf.SaveToStream(writer);

                throw new NotImplementedException();
            }
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("Value: {0}", Value);
        }

        #endregion
    }
}
