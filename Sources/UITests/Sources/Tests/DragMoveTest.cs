/* DragMoveTest.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Windows.Forms;

using CodeJam;

using IrbisUI;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace UITests
{
    public sealed class DragMoveTest
        : IUITest
    {
        #region IUITest members

        public void Run
            (
                IWin32Window ownerWindow
            )
        {
            using (Form form = new Form())
            {
                form.Size = new Size(800, 600);

                Button button1 = new Button
                {
                    Location = new Point(118, 124),
                    Size = new Size(75, 23),
                    TabIndex = 1,
                    Text = "Close",
                    UseVisualStyleBackColor = true
                };                
                form.Controls.Add(button1);
                button1.Click += (sender, args) =>
                {
                    form.Close();
                };

                Label label1 = new Label
                {
                    AutoSize = true,
                    BorderStyle = BorderStyle.FixedSingle,
                    Location = new Point(23, 21),
                    Size = new Size(37, 15),
                    Text = "label1"
                };
                form.Controls.Add(label1);
                label1.EnableDragMove(true);

                Label label2 = new Label
                {
                    AutoSize = true,
                    BorderStyle = BorderStyle.FixedSingle,
                    Location = new Point(81, 51),
                    Size = new Size(37, 15),
                    Text = "label2"
                };
                form.Controls.Add(label2);
                label2.MouseDown += (sender, args) =>
                {
                    label2.DragMove();
                };

                Label label3 = new Label
                {
                    AutoSize = true,
                    BorderStyle = BorderStyle.FixedSingle,
                    Location = new Point(141, 77),
                    Size = new Size(37, 15),
                    Text = "label3"
                };
                form.Controls.Add(label3);

                CheckBox checkBox1 = new CheckBox
                {
                    AutoSize = true,
                    Location = new Point(12, 100),
                    Size = new Size(75, 17),
                    Text = "Drag Form",
                    UseVisualStyleBackColor = true
                };
                form.Controls.Add(checkBox1);
                checkBox1.CheckedChanged += (sender, args) =>
                {
                    form.EnableDragMove(checkBox1.Checked);
                };

                IContainer components = new Container();

                ToolTip toolTip1 = new ToolTip(components);
                DragMoveProvider provider = new DragMoveProvider(components);

                toolTip1.SetToolTip(label1, "Drag the label to move it (automatic mode using EnableDragMove)");
                toolTip1.SetToolTip(label2, "Drag the label to move it (manual mode using the MouseDown event)");
                toolTip1.SetToolTip(label3, "Drag the label to move it (automatic mode using the DragMoveProvider)");
                toolTip1.SetToolTip(form, "Déplacez la fenêtre en la trainant sur le bureau (mode automatique avec RegisterForDragMove)");


                form.ShowDialog(ownerWindow);
                components.Dispose();
            }
        }

        #endregion
    }
}
