namespace IrbisUI.Sources
{
    partial class PftTreeView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._tree = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // _tree
            // 
            this._tree.CheckBoxes = true;
            this._tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tree.FullRowSelect = true;
            this._tree.HideSelection = false;
            this._tree.Location = new System.Drawing.Point(0, 0);
            this._tree.Name = "_tree";
            this._tree.Size = new System.Drawing.Size(258, 214);
            this._tree.TabIndex = 0;
            // 
            // PftTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tree);
            this.Name = "PftTreeView";
            this.Size = new System.Drawing.Size(258, 214);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView _tree;
    }
}
