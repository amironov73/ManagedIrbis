/* FieldPainterTest.cs -- 
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
    public sealed class FieldPainterTest
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

                form.Paint += form_Paint;

                form.ShowDialog(ownerWindow);
            }
        }

        void form_Paint
            (
                object sender,
                PaintEventArgs e
            )
        {
            FieldPainter painter = new FieldPainter
                (
                    Color.Red,
                    Color.Black
                );

            Form form = sender as Form;
            Font font = form.Font;
            PointF point = new PointF(10, 10);
            string text = "Text1^atext2^btext3";

            painter.DrawLine
                (
                    e.Graphics,
                    font,
                    point,
                    text
                );
        }

        #endregion
    }
}
