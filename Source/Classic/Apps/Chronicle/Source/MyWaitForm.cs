// Decompiled with JetBrains decompiler
// Type: Chronicle.MyWaitForm
// Assembly: Chronicle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DD9FD551-21B3-438F-8FDE-06D95EFCFAFD
// Assembly location: D:\Users\1\Google Диск\Projects\Cronicle\Chronicle.exe

using DevExpress.XtraWaitForm;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Chronicle
{
  public class MyWaitForm : WaitForm
  {
    private IContainer components = (IContainer) null;
    private ProgressPanel progressPanel1;
    private TableLayoutPanel tableLayoutPanel1;

    public MyWaitForm()
    {
      this.InitializeComponent();
      this.progressPanel1.AutoHeight = true;
    }

    public override void SetCaption(string caption)
    {
      base.SetCaption(caption);
      this.progressPanel1.Caption = caption;
    }

    public override void SetDescription(string description)
    {
      base.SetDescription(description);
      this.progressPanel1.Description = description;
    }

    public override void ProcessCommand(Enum cmd, object arg)
    {
      base.ProcessCommand(cmd, arg);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.progressPanel1 = new ProgressPanel();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.progressPanel1.Appearance.BackColor = Color.Transparent;
      this.progressPanel1.Appearance.Options.UseBackColor = true;
      this.progressPanel1.AppearanceCaption.Font = new Font("Microsoft Sans Serif", 12f);
      this.progressPanel1.AppearanceCaption.Options.UseFont = true;
      this.progressPanel1.AppearanceDescription.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.progressPanel1.AppearanceDescription.Options.UseFont = true;
      this.progressPanel1.Dock = DockStyle.Fill;
      this.progressPanel1.ImageHorzOffset = 20;
      this.progressPanel1.Location = new Point(0, 17);
      this.progressPanel1.Margin = new Padding(0, 3, 0, 3);
      this.progressPanel1.Name = "progressPanel1";
      this.progressPanel1.Size = new Size(246, 39);
      this.progressPanel1.TabIndex = 0;
      this.progressPanel1.Text = "progressPanel1";
      this.tableLayoutPanel1.AutoSize = true;
      this.tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.tableLayoutPanel1.BackColor = Color.Transparent;
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.Controls.Add((Control) this.progressPanel1, 0, 0);
      this.tableLayoutPanel1.Dock = DockStyle.Fill;
      this.tableLayoutPanel1.Location = new Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.Padding = new Padding(0, 14, 0, 14);
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.Size = new Size(246, 73);
      this.tableLayoutPanel1.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSize = true;
      this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.ClientSize = new Size(246, 73);
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.DoubleBuffered = true;
      this.Name = "Form1";
      this.StartPosition = FormStartPosition.Manual;
      this.Text = "Form1";
      this.tableLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public enum WaitFormCommand
    {
    }
  }
}
