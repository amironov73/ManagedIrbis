// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NonNullValue.cs -- must not be null
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// Must not be <c>null</c>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{Value}")]
    public struct NonNullValue<T>
        where T: class
    {
        #region Properties

        /// <summary>
        /// Value itself.
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
        /// Get the value.
        /// </summary>
        [NotNull]
        public T GetValue()
        {
            if (ReferenceEquals(_value, null))
            {
                Log.Error
                    (
                        "NonNullValue::GetValue: "
                        + "value is null"
                    );

                throw new ArgumentNullException();
            }

            return _value;
        }

        /// <summary>
        /// Set the value.
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
        /// Implicit conversion.
        /// </summary>
        public static implicit operator NonNullValue<T>
            (
                [NotNull] T value
            )
        {
            return new NonNullValue<T>(value);
        }

        /// <summary>
        /// Implicit conversion.
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

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Value.ToString();
        }

        #endregion
    }
}
