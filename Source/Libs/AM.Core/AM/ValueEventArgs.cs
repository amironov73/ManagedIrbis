/* ValueEventArgs.cs --
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
    /// <see cref="EventArgs"/> with value field.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{Value}")]
    public class ValueEventArgs<T>
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Empty event arguments.
        /// </summary>
        [NotNull]
        public new static readonly ValueEventArgs<T> Empty
            = new ValueEventArgs<T>();

        /// <summary>
        /// Value.
        /// </summary>
        [CanBeNull]
        public T Value { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ValueEventArgs()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ValueEventArgs
            (
                T value
            )
        {
            Value = value;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <inheritdoc />
        public override string ToString()
        {
            string result = ReferenceEquals(Value, null)
                ? "(null)"
                : Value.ToString();

            return result;
        }

        #endregion
    }

    /// <summary>
    /// <see cref="EventArgs"/> with value field.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{Value1} {Value2}")]
    public class ValueEventArgs<T1, T2>
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Empty event arguments.
        /// </summary>
        [NotNull]
        public new static readonly ValueEventArgs<T1, T2> Empty
            = new ValueEventArgs<T1, T2>();

        /// <summary>
        /// Value.
        /// </summary>
        [CanBeNull]
        public T1 Value1 { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        [CanBeNull]
        public T2 Value2 { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ValueEventArgs()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ValueEventArgs
            (
                T1 value1,
                T2 value2
            )
        {
            Value1 = value1;
            Value2 = value2;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <inheritdoc />
        public override string ToString()
        {
            string text1 = ReferenceEquals(Value1, null)
                ? "(null)"
                : Value1.ToString();
            string text2 = ReferenceEquals(Value2, null)
                ? "(null)"
                : Value2.ToString();
            string result = text1 + " " + text2;

            return result;
        }

        #endregion
    }
}
