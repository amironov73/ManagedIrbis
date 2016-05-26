/* FieldIndicator.cs -- индикатор поля
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
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
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Значение не установлено.
        /// </summary>
        public const string NoValue = " ";

        #endregion

        #region Properties

        /// <summary>
        /// Значение установлено?
        /// </summary>
        public bool HasValue
        {
            get { return !Value.SameString(NoValue); }
        }

        /// <summary>
        /// Значение индикатора.
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { SetValue(value); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FieldIndicator()
        {
            _value = NoValue;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FieldIndicator
            (
                [CanBeNull] string value
            )
        {
            SetValue(value);
        }

        #endregion

        #region Private members

        private string _value;

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
        /// Установка значения.
        /// </summary>
        [NotNull]
        public FieldIndicator SetValue
            (
                [CanBeNull] string value
            )
        {
            _value = string.IsNullOrEmpty(value) 
                ? NoValue
                : value.Substring(0, 1);
            return this;
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
            Code.NotNull(() => reader);

            string value = reader.ReadString();
            SetValue(value);
        }

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(() => writer);

            writer.Write(Value);
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
            return Value;
        }

        #endregion
    }
}
