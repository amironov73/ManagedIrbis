/* PenInfoControl.cs -- 
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
    class PenInfoControl
        : UserControl
    {
        private IWindowsFormsEditorService _svc;
        private ITypeDescriptorContext _context;
        private IServiceProvider _provider;

        private ComboBox alignment;
        private Panel color;
        private ComboBox dashStyle;
        private ComboBox startCap;
        private ComboBox endcap;
        private ComboBox lineJoin;
        private NumericUpDown width;
        private Button okButton;
        private Button cancelButton;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Panel panel1;
        private ComboBox dashCap;
        private Label label8;

        public PenInfo Result = null;

        public PenInfoControl ( PenInfo pinfo,
            IWindowsFormsEditorService svc,
            ITypeDescriptorContext context,
            IServiceProvider provider )
        {
            _svc = svc;
            _context = context;
            _provider = provider;
            InitializeComponent ();
            color.BackColor = pinfo.Color;

            foreach ( object o in Enum.GetValues ( typeof ( PenAlignment ) ) )
            {
                alignment.Items.Add ( o );
            }
            alignment.SelectedItem = pinfo.Alignment;
            foreach ( object o in Enum.GetValues ( typeof ( DashStyle ) ) )
            {
                dashStyle.Items.Add ( o );
            }
            dashStyle.SelectedItem= pinfo.DashStyle;
            foreach ( object o in Enum.GetValues ( typeof ( LineCap ) ) )
            {
                startCap.Items.Add ( o );
                endcap.Items.Add ( o );
            }
            startCap.SelectedItem = pinfo.StartCap;
            endcap.SelectedItem = pinfo.EndCap;
            foreach ( object o in Enum.GetValues ( typeof ( LineJoin ) ) )
            {
                lineJoin.Items.Add ( o );
            }
            lineJoin.SelectedItem = pinfo.LineJoin;
            foreach ( object o in Enum.GetValues ( typeof ( DashCap ) ) )
            {
                dashCap.Items.Add ( o );
            }
            dashCap.SelectedItem = pinfo.DashCap;
            width.Value = (decimal) pinfo.Width;
        }

        private PenInfo _Pen ()
        {
            PenInfo result = new PenInfo ();
            result.Alignment = (PenAlignment) alignment.SelectedItem;
            result.DashStyle = (DashStyle) dashStyle.SelectedItem;
            result.Color = color.BackColor;
            result.StartCap = (LineCap) startCap.SelectedItem;
            result.EndCap = (LineCap) endcap.SelectedItem;
            result.LineJoin = (LineJoin) lineJoin.SelectedItem;
            result.DashCap = (DashCap) dashCap.SelectedItem;
            result.Width = (float) width.Value;
            return result;
        }


        private void okButton_Click ( object sender, EventArgs e )
        {
            Result = _Pen ();
            _svc.CloseDropDown ();
        }

        private void cancelButton_Click ( object sender, EventArgs e )
        {
            Result = null;
            _svc.CloseDropDown ();
        }

        private void panel1_Paint ( object sender, PaintEventArgs e )
        {
            Graphics g = e.Graphics;
            using ( Pen pen = _Pen ().GetPen () )
            {
                int y = panel1.Height / 2;
                g.DrawLine ( pen, 10, y, panel1.Width - 10, y );
            }
        }

        private void color_Click ( object sender, EventArgs e )
        {
            using ( ColorDialog dialog = new ColorDialog () )
            {
                dialog.Color = color.BackColor;
                if ( dialog.ShowDialog () == DialogResult.OK )
                {
                    color.BackColor = dialog.Color;
                    alignment_SelectedIndexChanged ( sender, e );
                }
            }
        }

        private void alignment_SelectedIndexChanged ( object sender, EventArgs e )
        {
            panel1.Invalidate ();
        }

        private void InitializeComponent ()
        {
            this.alignment = new System.Windows.Forms.ComboBox();
            this.color = new System.Windows.Forms.Panel();
            this.dashStyle = new System.Windows.Forms.ComboBox();
            this.startCap = new System.Windows.Forms.ComboBox();
            this.endcap = new System.Windows.Forms.ComboBox();
            this.lineJoin = new System.Windows.Forms.ComboBox();
            this.width = new System.Windows.Forms.NumericUpDown();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dashCap = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.width)).BeginInit();
            this.SuspendLayout();
            // 
            // alignment
            // 
            this.alignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.alignment.FormattingEnabled = true;
            this.alignment.Location = new System.Drawing.Point(92, 9);
            this.alignment.Margin = new System.Windows.Forms.Padding(2, 3, 3, 3);
            this.alignment.Name = "alignment";
            this.alignment.Size = new System.Drawing.Size(121, 24);
            this.alignment.TabIndex = 0;
            this.alignment.SelectedIndexChanged += new System.EventHandler(this.alignment_SelectedIndexChanged);
            // 
            // color
            // 
            this.color.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.color.Location = new System.Drawing.Point(93, 37);
            this.color.Name = "color";
            this.color.Size = new System.Drawing.Size(120, 24);
            this.color.TabIndex = 1;
            this.color.Click += new System.EventHandler(this.color_Click);
            // 
            // dashStyle
            // 
            this.dashStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dashStyle.FormattingEnabled = true;
            this.dashStyle.Location = new System.Drawing.Point(93, 68);
            this.dashStyle.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.dashStyle.Name = "dashStyle";
            this.dashStyle.Size = new System.Drawing.Size(121, 24);
            this.dashStyle.TabIndex = 2;
            this.dashStyle.SelectedIndexChanged += new System.EventHandler(this.alignment_SelectedIndexChanged);
            // 
            // startCap
            // 
            this.startCap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.startCap.FormattingEnabled = true;
            this.startCap.Location = new System.Drawing.Point(92, 96);
            this.startCap.Name = "startCap";
            this.startCap.Size = new System.Drawing.Size(121, 24);
            this.startCap.TabIndex = 3;
            this.startCap.SelectedIndexChanged += new System.EventHandler(this.alignment_SelectedIndexChanged);
            // 
            // endcap
            // 
            this.endcap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.endcap.FormattingEnabled = true;
            this.endcap.Location = new System.Drawing.Point(92, 124);
            this.endcap.Name = "endcap";
            this.endcap.Size = new System.Drawing.Size(121, 24);
            this.endcap.TabIndex = 4;
            this.endcap.SelectedIndexChanged += new System.EventHandler(this.alignment_SelectedIndexChanged);
            // 
            // lineJoin
            // 
            this.lineJoin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lineJoin.FormattingEnabled = true;
            this.lineJoin.Location = new System.Drawing.Point(93, 152);
            this.lineJoin.Name = "lineJoin";
            this.lineJoin.Size = new System.Drawing.Size(121, 24);
            this.lineJoin.TabIndex = 5;
            this.lineJoin.SelectedIndexChanged += new System.EventHandler(this.alignment_SelectedIndexChanged);
            // 
            // width
            // 
            this.width.Location = new System.Drawing.Point(94, 180);
            this.width.Name = "width";
            this.width.Size = new System.Drawing.Size(120, 22);
            this.width.TabIndex = 6;
            this.width.ValueChanged += new System.EventHandler(this.alignment_SelectedIndexChanged);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(12, 307);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(93, 23);
            this.okButton.TabIndex = 7;
            this.okButton.Text = "Ok";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(111, 307);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(102, 23);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "Alignment";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "Color";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 75);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 17);
            this.label3.TabIndex = 11;
            this.label3.Text = "Dash style";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "Start cap";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 17);
            this.label5.TabIndex = 13;
            this.label5.Text = "End cap";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 159);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 17);
            this.label6.TabIndex = 14;
            this.label6.Text = "Line join";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 185);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 17);
            this.label7.TabIndex = 15;
            this.label7.Text = "Width";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(10, 239);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(203, 61);
            this.panel1.TabIndex = 16;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // dashCap
            // 
            this.dashCap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dashCap.FormattingEnabled = true;
            this.dashCap.Location = new System.Drawing.Point(94, 207);
            this.dashCap.Name = "dashCap";
            this.dashCap.Size = new System.Drawing.Size(121, 24);
            this.dashCap.TabIndex = 17;
            this.dashCap.SelectedIndexChanged += new System.EventHandler(this.alignment_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 214);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 17);
            this.label8.TabIndex = 18;
            this.label8.Text = "Dash cap";
            // 
            // PenInfoControl
            // 
            this.Controls.Add(this.label8);
            this.Controls.Add(this.dashCap);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.width);
            this.Controls.Add(this.lineJoin);
            this.Controls.Add(this.endcap);
            this.Controls.Add(this.startCap);
            this.Controls.Add(this.dashStyle);
            this.Controls.Add(this.color);
            this.Controls.Add(this.alignment);
            this.Name = "PenInfoControl";
            this.Size = new System.Drawing.Size(229, 338);
            ((System.ComponentModel.ISupportInitialize)(this.width)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
