/* NonNullValue.cs -- не должен принимать значение null
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// Не должен принимать значение null.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("{Value}")]
#endif
    public struct NonNullValue<T>
        where T: class
    {
        #region Properties

        /// <summary>
        /// Собственно значение
        /// </summary>
        [NotNull]
        public T Value
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public NonNullValue
            (
                [NotNull] T value
            )
        {
            Code.NotNull(value, "value");
            _value = value;
        }

        #endregion

        #region Private members

        private T _value;

        #endregion

        #region Public methods

        /// <summary>
        /// Получение значения
        /// </summary>
        [NotNull]
        public T GetValue()
        {
            if (ReferenceEquals(_value, null))
            {
                throw new ArgumentNullException();
            }

            return _value;
        }

        /// <summary>
        /// Присвоение значения.
        /// </summary>
        public void SetValue
            (
                [NotNull] T value
            )
        {
            Code.NotNull(value, "value");

            _value = value;
        }

        /// <summary>
        /// Преобразование.
        /// </summary>
        public static implicit operator NonNullValue<T>
            (
                [NotNull] T value
            )
        {
            return new NonNullValue<T>(value);
        }

        /// <summary>
        /// Преобразование.
        /// </summary>
        [NotNull]
        public static implicit operator T
            (
                NonNullValue<T> value
            )
        {
            return value.Value;
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
            return Value.ToString();
        }

        #endregion
    }
}
