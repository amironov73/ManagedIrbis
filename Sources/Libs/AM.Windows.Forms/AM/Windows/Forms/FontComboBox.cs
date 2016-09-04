/* FontComboBox.cs --
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Комбинированный список, позволяющий выбрать
    /// шрифт из списка.
    /// </summary>
    [PublicAPI]
    //[ToolboxBitmap(typeof(FontComboBox), "FontComboBox.bmp")]
    [System.ComponentModel.DesignerCategory("Code")]
    public class FontComboBox
        : ComboBox
    {
        #region Properties

        ///<summary>
        /// Имя выбранного шрифта.
        ///</summary>
        [Browsable(false)]
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Hidden)]
        public string SelectedFontName
        {
            [DebuggerStepThrough]
            get
            {
                object selected = SelectedItem;
                if (selected == null)
                {
                    return null;
                }

                FontFamily font = (FontFamily) selected;

                return font.Name;
            }
            [DebuggerStepThrough]
            set
            {
                if (value != null)
                {
                    for (int i = 0; i < Items.Count; i++)
                    {
                        if (((FontFamily)Items[i]).Name == value)
                        {
                            SelectedIndex = i;
                            return;
                        }
                    }
                    //throw new ArgumentException ( "No such font: " + value );
                }
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:FontComboBox"/> class.
        /// </summary>
        public FontComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += _DrawItem;
            FontFamily[] families = FontFamily.Families;
            foreach (FontFamily family in families)
            {
                Items.Add(family);
            }
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
            e.DrawBackground();
            e.DrawFocusRectangle();
            if (e.Index >= 0)
            {
                FontFamily family = (FontFamily)Items[e.Index];
                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };
                Rectangle r = e.Bounds;
                r.Inflate(-4, 0);
                int height = (r.Height * 7) / 10;
                FontStyle style = FontStyle.Regular;
                if (!family.IsStyleAvailable(style))
                {
                    style = FontStyle.Bold;
                }
                if (!family.IsStyleAvailable(style))
                {
                    style = FontStyle.Italic;
                }
                if (!family.IsStyleAvailable(style))
                {
                    family = FontFamily.GenericSerif;
                    style = FontStyle.Regular;
                }
                Brush brush = ((e.State & DrawItemState.Selected) != 0)
                                  ?
                              SystemBrushes.HighlightText
                                  : SystemBrushes.WindowText;
                using (Font font = new Font(family, height, style))
                {
                    g.DrawString(family.Name, font, brush, r, format);
                }
            }
        }

        #endregion

        #region ComboBox members

        /// <summary>
        /// Gets an object representing the collection of the items contained in this <see cref="T:System.Windows.Forms.ComboBox"></see>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Hidden)]
        public new ObjectCollection Items
        {
            [DebuggerStepThrough]
            get
            {
                return base.Items;
            }
        }

        #endregion
    }
}
