/* HatchComboBox.cs --
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolboxBitmap (typeof(HatchComboBox),
        "Images.HatchComboBox.bmp")]
    public class HatchComboBox
        : ComboBox
    {
        #region Properties

        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        /// <value>The style.</value>
        [DefaultValue(0)]
        public HatchStyle Style
        {
            get
            {
                if (SelectedItem == null)
                {
                    return 0;
                }

                return (HatchStyle)SelectedItem;
            }
            set
            {
                int index = Items.IndexOf(value);
                SelectedIndex = index;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HatchComboBox"/> class.
        /// </summary>
        public HatchComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (HatchStyle style in
                Enum.GetValues(typeof(HatchStyle)))
            {
                Items.Add(style);
            }
            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += _DrawItem;
        }

        #endregion

        #region Private members

        private void _DrawItem
            (
                object sender,
                DrawItemEventArgs e
            )
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                HatchStyle style = (HatchStyle)Items[e.Index];
                using (HatchBrush brush
                    = new HatchBrush(style, e.ForeColor, e.BackColor))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }
            }
            e.DrawFocusRectangle();
        }

        #endregion
    }
}
