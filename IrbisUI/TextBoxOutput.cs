/* TextBoxOutput.cs -- вывод в текстовое поле
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

using AM.Text.Output;

using IrbisUI.Extensions;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI
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
            if (ReferenceEquals(textBox, null))
            {
                throw new ArgumentNullException("textBox");
            }
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
                if (TextBox != null)
                {
                    TextBox.InvokeIfRequired
                        (
                            () => TextBox.AppendText(text)
                        );
                }
            }
            if (TextBox != null)
            {
                TextBox.InvokeIfRequired
                    (
                        () => TextBox.SelectionStart = TextBox.TextLength
                    );
            }
        }

        #endregion

        #region AbstractOutput members

        public override bool HaveError { get; set; }

        public override AbstractOutput Clear()
        {
            HaveError = false;
            if (TextBox != null)
            {
                TextBox.InvokeIfRequired
                    (
                        () => TextBox.Clear()
                );
            }
            return this;
        }

        public override AbstractOutput Configure
            (
                string configuration
            )
        {
            // TODO: implement
            return this;
        }

        public override AbstractOutput Write
            (
                string text
            )
        {
            AppendText(text);
            return this;
        }

        public override AbstractOutput WriteError(string text)
        {
            HaveError = true;
            AppendText(text);
            return this;
        }

        #endregion
    }
}
