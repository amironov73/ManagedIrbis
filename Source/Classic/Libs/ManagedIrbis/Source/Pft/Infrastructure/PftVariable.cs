// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftVariable.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftVariable
    {
        #region Events

        ///// <summary>
        ///// Вызывается непосредственно перед считыванием значения.
        ///// </summary>
        //public event EventHandler<PftDebugEventArgs> BeforeReading;

        ///// <summary>
        ///// Вызывается непосредственно после модификации.
        ///// </summary>
        //public event EventHandler<PftDebugEventArgs> AfterModification;

        #endregion

        #region Properties

        /// <summary>
        /// Имя переменной.
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        /// <summary>
        /// Признак числовой переменной.
        /// </summary>
        public bool IsNumeric { get; set; }

        /// <summary>
        /// Числовое значение.
        /// </summary>
        public double NumericValue { get; set; }

        /// <summary>
        /// Строковое значение.
        /// </summary>
        public string StringValue { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariable()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariable
            (
                [NotNull] string name,
                bool isNumeric
            )
        {
            Code.NotNullNorEmpty(name, "name");

            Name = name;
            IsNumeric = isNumeric;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariable
            (
                [NotNull] string name,
                double numericValue
            )
        {
            Code.NotNullNorEmpty(name, "name");

            Name = name;
            IsNumeric = true;
            NumericValue = numericValue;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariable
            (
                [NotNull] string name,
                string stringValue
            )
        {
            Code.NotNullNorEmpty(name, "name");

            Name = name;
            StringValue = stringValue;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append(Name.ToVisibleString());
            result.Append(": ");
            if (IsNumeric)
            {
                result.Append(NumericValue.ToInvariantString());
            }
            else
            {
                if (ReferenceEquals(StringValue, null))
                {
                    result.Append("(null)");
                }
                else
                {
                    result.Append('\"');
                    result.Append(StringValue);
                    result.Append('\"');
                }
            }

            return result.ToString();
        }

        #endregion
    }
}
