/* FieldIndicator.cs -- индикатор поля
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Data;
using System.Diagnostics;
using System.IO;

using AM;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Индикатор поля.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [DebuggerDisplay("Value = '{Value}'")]
    public sealed class FieldIndicator
        : IHandmadeSerializable,
        IReadOnly<FieldIndicator>
    {
        #region Constants

        /// <summary>
        /// Значение не установлено.
        /// </summary>
        public const string NoValue = " ";

        /// <summary>
        /// For visual only.
        /// </summary>
        public const string EmptyValue = "#";

        #endregion

        #region Properties

        /// <summary>
        /// Field.
        /// </summary>
        [CanBeNull]
        public RecordField Field { get { return _field; } }

        /// <summary>
        /// Whether value is set?
        /// </summary>
        public bool HasValue
        {
            get
            {
                return !Value.SameString(NoValue)
                    && !string.IsNullOrEmpty(Value);
            }
        }

        /// <summary>
        /// Value of the indicator.
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { SetValue(value); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldIndicator()
        {
            _value = NoValue;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldIndicator
            (
                [CanBeNull] string value
            )
        {
            SetValue(value);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldIndicator
            (
                [CanBeNull] string value,
                bool readOnly
            )
        {
            SetValue(value);
            if (readOnly)
            {
                SetReadOnly();
            }
        }

        #endregion

        #region Private members

        private string _value;

        [NonSerialized]
        internal RecordField _field;

        #endregion

        #region Public methods

        /// <summary>
        /// Создание клона индикатора.
        /// </summary>
        [NotNull]
        public FieldIndicator Clone()
        {
            FieldIndicator result = new FieldIndicator(Value);

            return result;
        }

        /// <summary>
        /// Marks the indicator as read-only.
        /// </summary>
        public void SetReadOnly()
        {
            _readOnly = true;
        }

        /// <summary>
        /// Установка значения.
        /// </summary>
        [NotNull]
        public FieldIndicator SetValue
            (
                [CanBeNull] string value
            )
        {
            ThrowIfReadOnly();

            _value = string.IsNullOrEmpty(value) 
                ? NoValue
                : value.Substring(0, 1);
            return this;
        }

        /// <summary>
        /// Text representation of the field.
        /// </summary>
        [NotNull]
        public string ToText()
        {
            return (string.IsNullOrEmpty(Value))
                ? EmptyValue
                : Value;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore the object state from the given stream.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(reader, "reader");

            string value = reader.ReadString();
            SetValue(value);
        }

        /// <summary>
        /// Save the object state to the given stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.Write(Value);
        }

        #endregion

        #region IReadOnly<T> members

        [NonSerialized]
        internal bool _readOnly;

        /// <summary>
        /// Whether the indicator is read-only?
        /// </summary>
        public bool ReadOnly { get { return _readOnly; } }

        /// <summary>
        /// Creates read-only clone of the indicator.
        /// </summary>
        public FieldIndicator AsReadOnly()
        {
            FieldIndicator result = Clone();
            result._readOnly = true;

            return result;
        }

        /// <summary>
        /// Throws if read only.
        /// </summary>
        public void ThrowIfReadOnly()
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
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
            return ToText();
        }

        #endregion
    }
}
