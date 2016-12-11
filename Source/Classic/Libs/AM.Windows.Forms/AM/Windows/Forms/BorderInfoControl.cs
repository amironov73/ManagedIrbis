// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BorderInfoControl.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.ComponentModel;
using System.Windows.Forms.Design;

#endregion

namespace AM.Windows.Forms
{
    class BorderInfoControl
        : UserControl
    {
        private IWindowsFormsEditorService _svc;
        private ITypeDescriptorContext _context;
        private IServiceProvider _provider;

        private System.Windows.Forms.CheckBox drawBox;
        private System.Windows.Forms.CheckBox draw3D;
        private System.Windows.Forms.ComboBox style2D;
        private System.Windows.Forms.ComboBox style3D;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Panel color2D;

        public BorderInfo Result = null;

        public BorderInfoControl
            (
                BorderInfo binfo,
                IWindowsFormsEditorService svc,
                ITypeDescriptorContext context,
                IServiceProvider provider
            )
        {
            _svc = svc;
            _context = context;
            _provider = provider;
            InitializeComponent();

            drawBox.Checked = binfo.DrawBorder;
            draw3D.Checked = binfo.Draw3D;

            foreach (object o in Enum.GetValues(typeof(ButtonBorderStyle)))
            {
                style2D.Items.Add(o);
            }
            style2D.SelectedItem = binfo.Style2D;
            foreach (object o in Enum.GetValues(typeof(Border3DStyle)))
            {
                style3D.Items.Add(o);
            }
            style3D.SelectedItem = binfo.Style3D;
            color2D.BackColor = binfo.BorderColor;
        }

        private BorderInfo _Border()
        {
            BorderInfo result = new BorderInfo();
            result.DrawBorder = drawBox.Checked;
            result.Draw3D = draw3D.Checked;
            result.Style2D = (ButtonBorderStyle)style2D.SelectedItem;
            result.Style3D = (Border3DStyle)style3D.SelectedItem;
            result.BorderColor = color2D.BackColor;
            return result;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Result = _Border();
            _svc.CloseDropDown();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Result = null;
            _svc.CloseDropDown();
        }

        private void drawBox_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            BorderInfo b = _Border();
            b.Draw(e.Graphics, panel1.ClientRectangle);
        }

        private void color2D_Click(object sender, EventArgs e)
        {
            using (ColorDialog dialog = new ColorDialog())
            {
                dialog.Color = color2D.BackColor;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    color2D.BackColor = dialog.Color;
                    drawBox_CheckedChanged(sender, e);
                }
            }
        }

        private void BorderInfoControl_Load(object sender, EventArgs e)
        {
            BackColor = SystemColors.Control;
            //ParentForm.AcceptButton = okButton;
            //ParentForm.CancelButton = cancelButton;
        }

        private void InitializeComponent()
        {
            this.drawBox = new System.Windows.Forms.CheckBox();
            this.draw3D = new System.Windows.Forms.CheckBox();
            this.style2D = new System.Windows.Forms.ComboBox();
            this.style3D = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.color2D = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // drawBox
            // 
            this.drawBox.AutoSize = true;
            this.drawBox.Location = new System.Drawing.Point(13, 13);
            this.drawBox.Name = "drawBox";
            this.drawBox.Size = new System.Drawing.Size(108, 21);
            this.drawBox.TabIndex = 0;
            this.drawBox.Text = "Draw border";
            this.drawBox.CheckedChanged += new System.EventHandler(this.drawBox_CheckedChanged);
            // 
            // draw3D
            // 
            this.draw3D.AutoSize = true;
            this.draw3D.Location = new System.Drawing.Point(122, 13);
            this.draw3D.Name = "draw3D";
            this.draw3D.Size = new System.Drawing.Size(94, 21);
            this.draw3D.TabIndex = 1;
            this.draw3D.Text = "3D border";
            this.draw3D.CheckedChanged += new System.EventHandler(this.drawBox_CheckedChanged);
            // 
            // style2D
            // 
            this.style2D.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.style2D.FormattingEnabled = true;
            this.style2D.Location = new System.Drawing.Point(73, 37);
            this.style2D.Margin = new System.Windows.Forms.Padding(2, 3, 3, 3);
            this.style2D.Name = "style2D";
            this.style2D.Size = new System.Drawing.Size(139, 24);
            this.style2D.TabIndex = 2;
            this.style2D.SelectedIndexChanged += new System.EventHandler(this.drawBox_CheckedChanged);
            // 
            // style3D
            // 
            this.style3D.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.style3D.FormattingEnabled = true;
            this.style3D.Location = new System.Drawing.Point(73, 95);
            this.style3D.Name = "style3D";
            this.style3D.Size = new System.Drawing.Size(139, 24);
            this.style3D.TabIndex = 4;
            this.style3D.SelectedIndexChanged += new System.EventHandler(this.drawBox_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(14, 124);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(198, 72);
            this.panel1.TabIndex = 5;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 44);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "2D style";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "2D color";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "3D style";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(14, 203);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(107, 23);
            this.okButton.TabIndex = 9;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(122, 203);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(90, 23);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // color2D
            // 
            this.color2D.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.color2D.Location = new System.Drawing.Point(73, 68);
            this.color2D.Name = "color2D";
            this.color2D.Size = new System.Drawing.Size(139, 19);
            this.color2D.TabIndex = 11;
            this.color2D.Click += new System.EventHandler(this.color2D_Click);
            // 
            // BorderInfoControl
            // 
            this.Controls.Add(this.color2D);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.style3D);
            this.Controls.Add(this.style2D);
            this.Controls.Add(this.draw3D);
            this.Controls.Add(this.drawBox);
            this.Name = "BorderInfoControl";
            this.Size = new System.Drawing.Size(224, 239);
            this.Load += new System.EventHandler(this.BorderInfoControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}