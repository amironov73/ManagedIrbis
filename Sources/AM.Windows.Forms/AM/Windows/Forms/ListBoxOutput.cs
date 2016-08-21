/* ListBoxOutput.cs -- вывод в ListBox
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
    /// Вывод в ListBox.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ListBoxOutput
        : AbstractOutput
    {
        #region Properties

        /// <summary>
        /// Текстбокс
        /// </summary>
        public ListBox ListBox { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor
        /// </summary>
        public ListBoxOutput()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ListBoxOutput
            (
                [NotNull] ListBox listBox
            )
        {
            Code.NotNull(listBox, "listBox");

            ListBox = listBox;
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
                ListBox.InvokeIfRequired
                    (
                        () => ListBox.Items.Add(text)
                    );
            }
            ListBox.InvokeIfRequired
                (
                    () =>
                        {
                            if (ListBox.Items.Count != 0)
                            {
                                ListBox.SelectedIndex
                                    = ListBox.Items.Count - 1;
                            }
                        }
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
            ListBox.InvokeIfRequired
                (
                    () => ListBox.Items.Clear()
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
