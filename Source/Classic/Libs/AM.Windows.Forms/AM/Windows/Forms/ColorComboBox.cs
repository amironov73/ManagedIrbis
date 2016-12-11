// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ColorComboBox.cs --
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Комбинированный список, позволяющий выбрать
    /// цвет из списка.
    /// </summary>
    [PublicAPI]
    [ToolboxBitmap(typeof(ColorComboBox),
        "Images.ColorComboBox.bmp")]
    [System.ComponentModel.DesignerCategory("Code")]
    public class ColorComboBox
        : ComboBox
    {
        #region Properties

        ///<summary>
        /// Выбранный цвет.
        ///</summary>
        public Color SelectedColor
        {
            [DebuggerStepThrough]
            get
            {
                if (SelectedIndex < 0)
                {
                    return Color.Black;
                }
                return (Color)Items[SelectedIndex];
            }
            [DebuggerStepThrough]
            set
            {
                if (!Items.Contains(value))
                {
                    int index = Items.Add(value);
                    SelectedIndex = index;
                }
                SelectedIndex = Items.IndexOf(value);
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:ColorComboBox"/> class.
        /// </summary>
        public ColorComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += _DrawItem;
            Items.AddRange(new object[]
                {
                    Color.Black,
                    Color.White,
                    Color.Red,
                    Color.Green,
                    Color.Blue,
                    Color.DarkGray,
                    Color.Gray,
                    Color.Cyan,
                    Color.Magenta,
                    Color.DarkRed,
                    Color.DarkGreen,
                    Color.DarkBlue,
                    Color.Brown
                });
        }

        #endregion

        #region Private members

        private void _DrawItem
            (
                object sender,
                DrawItemEventArgs e
            )
        {
            Graphics g = e.Graphics;
            Rectangle r = e.Bounds;
            e.DrawBackground();
            e.DrawFocusRectangle();
            if (e.Index >= 0)
            {
                r.Inflate(-4, -2);
                using (Brush brush
                    = new SolidBrush((Color)Items[e.Index]))
                {
                    g.FillRectangle(brush, r);
                }
                g.DrawRectangle(Pens.Black, r);
            }
        }

        #endregion
    }
}
