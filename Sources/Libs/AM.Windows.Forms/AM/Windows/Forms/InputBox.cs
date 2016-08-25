/* InputBox.cs -- simple string value input dialog
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Simple string value input dialog.
    /// </summary>
    [PublicAPI]
    public sealed class InputBox
        : Form
    {
        #region Properties



        #endregion

        #region Construction

        private InputBox()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label topLabel;
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label promptLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources =
                new System.Resources.ResourceManager(typeof(InputBox));
            this.panel1 = new System.Windows.Forms.Panel();
            this.topLabel = new System.Windows.Forms.Label();
            this.promptLabel = new System.Windows.Forms.Label();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.cancelButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.topLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(368, 56);
            this.panel1.TabIndex = 0;
            // 
            // topLabel
            // 
            this.topLabel.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                  (((System.Windows.Forms.AnchorStyles.Top
                        | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
            this.topLabel.Font =
                new System.Drawing.Font("Microsoft Sans Serif",
                                          8.25F,
                                          System.Drawing.FontStyle.Bold,
                                          System.Drawing.GraphicsUnit.Point,
                                          ((System.Byte)(204)));
            this.topLabel.Location = new System.Drawing.Point(16, 8);
            this.topLabel.Name = "topLabel";
            this.topLabel.Size = new System.Drawing.Size(278, 40);
            this.topLabel.TabIndex = 0;
            this.topLabel.Text =
                "Введите значение. Нажмите OK для подтверждения или Отмена для прекращения";
            this.topLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // promptLabel
            // 
            this.promptLabel.Location = new System.Drawing.Point(16, 80);
            this.promptLabel.Name = "promptLabel";
            this.promptLabel.Size = new System.Drawing.Size(328, 16);
            this.promptLabel.TabIndex = 1;
            this.promptLabel.Text = "Поясняющий текст";
            // 
            // inputTextBox
            // 
            this.inputTextBox.Location = new System.Drawing.Point(16, 96);
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(336, 20);
            this.inputTextBox.TabIndex = 2;
            this.inputTextBox.Text = "";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.okButton.ImageIndex = 0;
            this.okButton.ImageList = this.imageList1;
            this.okButton.Location = new System.Drawing.Point(72, 136);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(96, 23);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            // 
            // imageList1
            // 
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.ImageStream =
                ((System.Windows.Forms.ImageListStreamer)
                  (resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.White;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cancelButton.ImageIndex = 1;
            this.cancelButton.ImageList = this.imageList1;
            this.cancelButton.Location = new System.Drawing.Point(184, 136);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(96, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Отмена";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image =
                ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(288, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(72, 40);
            this.pictureBox1.SizeMode =
                System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // InputBox
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(368, 176);
            this.ControlBox = false;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.inputTextBox);
            this.Controls.Add(this.promptLabel);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputBox";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "InputBox";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Запрашивает у пользователя строковое значение 
        /// (предлагая значение по умолчанию).
        /// </summary>
        /// <param name="caption">Заголовок окна.</param>
        /// <param name="prompt">Поясняющий текст.</param>
        /// <param name="theValue">Куда помещать результат.</param>
        /// <returns>Результат отработки диалогового окна.</returns>
        public static DialogResult Query
            (
                string caption,
                string prompt,
                ref string theValue
            )
        {
            return Query
                (
                    caption,
                    prompt,
                    null,
                    ref theValue
                );
        }

        /// <summary>
        /// Запрашивает у пользователя строковое значение
        /// (предлагая значение по умолчанию).
        /// </summary>
        /// <param name="caption">Заголовок окна.</param>
        /// <param name="prompt">Поясняющий текст.</param>
        /// <param name="theValue">Куда помещать результат.</param>
        /// <param name="topText">Текст в верхней части окна.</param>
        /// <returns>Результат отработки диалогового окна.</returns>
        public static DialogResult Query
            (
                string caption,
                string prompt,
                string topText,
                ref string theValue
            )
        {
            using (InputBox box = new InputBox())
            {
                if (topText != null)
                {
                    box.topLabel.Text = topText;
                }
                box.Text = caption;
                box.promptLabel.Text = prompt;
                box.inputTextBox.Text = theValue;
                DialogResult result = box.ShowDialog();
                theValue = box.inputTextBox.Text;

                return result;
            }
        }

        #endregion
    }
}
