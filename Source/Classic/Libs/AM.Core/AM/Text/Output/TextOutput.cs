// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextOutput.cs -- вывод в текстовую строку.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text.Output
{
    /// <summary>
    /// Вывод в текстовую строку.
    /// </summary>
    public sealed class TextOutput
        : AbstractOutput
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TextOutput"/> class.
        /// </summary>
        public TextOutput()
        {
            _builder = new StringBuilder();
        }

        #endregion

        #region Private members

        private readonly StringBuilder _builder;

        #endregion

        #region Public methods

        #endregion

        #region AbstractOutput members

        /// <summary>
        /// Флаг: был ли вывод с помощью WriteError.
        /// </summary>
        public override bool HaveError { get; set; }

        /// <summary>
        /// Очищает вывод, например, окно.
        /// Надо переопределить в потомке.
        /// </summary>
        public override AbstractOutput Clear()
        {
            _builder.Length = 0;
            HaveError = false;

            return this;
        }

        /// <summary>
        /// Конфигурирование объекта.
        /// Надо переопределить в потомке.
        /// </summary>
        public override AbstractOutput Configure
            (
                string configuration
            )
        {
            // TODO: implement

            return this;
        }

        /// <summary>
        /// Метод, который нужно переопределить
        /// в потомке.
        /// </summary>
        public override AbstractOutput Write
            (
                string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                _builder.Append(text);
            }

            return this;
        }

        /// <summary>
        /// Выводит ошибку. Например, красным цветом.
        /// Надо переопределить в потомке.
        /// </summary>
        public override AbstractOutput WriteError
            (
                string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                _builder.Append(text);
            }

            return this;
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
            return _builder.ToString();
        }

        #endregion
    }
}
