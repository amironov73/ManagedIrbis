// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextBoxOutput.cs -- вывод в текстовое поле
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Windows.Forms;

using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Вывод в текстовое поле.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class TextBoxOutput
        : AbstractOutput
    {
        #region Properties

        /// <summary>
        /// Текстбокс
        /// </summary>
        public TextBox TextBox { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public TextBoxOutput()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public TextBoxOutput
            (
                [NotNull] TextBox textBox
            )
        {
            Code.NotNull(textBox, "textBox");

            TextBox = textBox;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление текста в конец текстбокса.
        /// </summary>
        /// <param name="text"></param>
        public void AppendText
            (
                [CanBeNull] string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                TextBox.InvokeIfRequired
                    (
                        () => TextBox.AppendText(text)
                    );
            }
            TextBox.InvokeIfRequired
                (
                    () => TextBox.SelectionStart = TextBox.TextLength
                );
        }

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
            HaveError = false;
            TextBox.InvokeIfRequired
                (
                    () => TextBox.Clear()
                );

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
            AppendText(text);

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
            HaveError = true;
            AppendText(text);

            return this;
        }

        #endregion
    }
}
