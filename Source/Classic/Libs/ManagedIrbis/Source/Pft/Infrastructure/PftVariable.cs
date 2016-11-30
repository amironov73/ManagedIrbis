// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftVariable.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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
                string name,
                bool isNumeric
            )
        {
            Name = name;
            IsNumeric = isNumeric;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariable
            (
                string name,
                double numericValue
            )
        {
            Name = name;
            IsNumeric = true;
            NumericValue = numericValue;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariable
            (
                string name,
                string stringValue
            )
        {
            Name = name;
            StringValue = stringValue;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append(Name);
            result.Append(": ");
            if (IsNumeric)
            {
                result.Append(NumericValue);
            }
            else
            {
                result.Append("\"");
                result.Append(StringValue);
                result.Append("\"");
            }

            return result.ToString();
        }

        #endregion
    }
}
