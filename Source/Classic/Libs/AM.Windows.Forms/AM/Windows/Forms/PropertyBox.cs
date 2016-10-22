/* PropertyBox.cs --  
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PropertyBox
        : Form
    {
        private System.Windows.Forms.Label _label1;
        private System.Windows.Forms.Panel _panel1;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.PropertyGrid _propertyGrid;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container _components = null;

        /// <summary>
        /// Property SelectedObject (object)
        /// </summary>
        public object SelectedObject
        {
            get
            {
                return _propertyGrid.SelectedObject;
            }
            set
            {
                _propertyGrid.SelectedObject = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected objects.
        /// </summary>
        /// <value>The selected objects.</value>
        public object[] SelectedObjects
        {
            get
            {
                return _propertyGrid.SelectedObjects;
            }
            set
            {
                _propertyGrid.SelectedObjects = value;
            }
        }

        #region Construction

        private PropertyBox()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        private PropertyBox(object obj)
            : this()
        {
            SelectedObject = obj;
        }

        private PropertyBox(object[] obj)
            : this()
        {
            SelectedObjects = obj;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public static DialogResult ShowDialog
            (
                IWin32Window ownerWindow,
                object obj
            )
        {
            DialogResult result;

            using (PropertyBox box = new PropertyBox(obj))
            {
                result = box.ShowDialog(ownerWindow);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public static DialogResult ShowDialog
            (
                IWin32Window ownerWindow,
                object[] obj
            )
        {
            DialogResult result;

            using (PropertyBox box = new PropertyBox(obj))
            {
                result = box.ShowDialog(ownerWindow);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static PropertyBox Show(object obj)
        {
            PropertyBox box = new PropertyBox(obj);
            box.Show();
            return box;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static PropertyBox Show(object[] obj)
        {
            PropertyBox box = new PropertyBox(obj);
            box.Show();
            return box;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_components != null)
                {
                    _components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyBox));
            this._label1 = new System.Windows.Forms.Label();
            this._panel1 = new System.Windows.Forms.Panel();
            this._cancelButton = new System.Windows.Forms.Button();
            this._okButton = new System.Windows.Forms.Button();
            this._propertyGrid = new System.Windows.Forms.PropertyGrid();
            this._panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this._label1.BackColor = System.Drawing.Color.White;
            this._label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._label1.Dock = System.Windows.Forms.DockStyle.Top;
            this._label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this._label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._label1.Location = new System.Drawing.Point(0, 0);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(336, 46);
            this._label1.TabIndex = 0;
            this._label1.Text = "Настройте свойства объекта\r\nи нажмите OK или Отмена";
            this._label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this._panel1.Controls.Add(this._cancelButton);
            this._panel1.Controls.Add(this._okButton);
            this._panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._panel1.Location = new System.Drawing.Point(0, 399);
            this._panel1.Name = "_panel1";
            this._panel1.Size = new System.Drawing.Size(336, 47);
            this._panel1.TabIndex = 1;
            // 
            // cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(231, 9);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(90, 27);
            this._cancelButton.TabIndex = 1;
            this._cancelButton.Text = "Отмена";
            // 
            // okButton
            // 
            this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okButton.Location = new System.Drawing.Point(125, 9);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(90, 27);
            this._okButton.TabIndex = 0;
            this._okButton.Text = "OK";
            // 
            // propertyGrid
            // 
            this._propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
            this._propertyGrid.Location = new System.Drawing.Point(0, 46);
            this._propertyGrid.Name = "_propertyGrid";
            this._propertyGrid.Size = new System.Drawing.Size(336, 353);
            this._propertyGrid.TabIndex = 2;
            // 
            // PropertyBox
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(336, 446);
            this.ControlBox = false;
            this.Controls.Add(this._propertyGrid);
            this.Controls.Add(this._panel1);
            this.Controls.Add(this._label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PropertyBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Свойства";
            this._panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
    }
}
