/* TextBoxPlus.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using AM.Win32;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [System.ComponentModel.DesignerCategoryAttribute("Code")]
    public class TextBoxPlus
        : TextBox
    {
        private string _defaultText = "(null)";

        ///<summary>
        /// 
        ///</summary>
        public string DefaultText
        {
            [DebuggerStepThrough]
            get
            {
                return _defaultText;
            }
            [DebuggerStepThrough]
            set
            {
                _defaultText = value;
                Invalidate();
            }
        }

        private const string DefaultTextColor = "LightGray";
        private Color _defaultColor = Color.FromName(DefaultTextColor);

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(typeof(Color), DefaultTextColor)]
        public Color DefaultColor
        {
            [DebuggerStepThrough]
            get
            {
                return _defaultColor;
            }
            [DebuggerStepThrough]
            set
            {
                _defaultColor = value;
                Invalidate();
            }
        }

        private void _DrawNothing()
        {
            if (string.IsNullOrEmpty(Text) && !Focused)
            {
                using (Graphics g = Graphics.FromHwnd(Handle))
                using (Brush brush = new SolidBrush(DefaultColor))
                {
                    StringFormat format = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Far
                    };
                    g.DrawString
                        (
                            DefaultText,
                            Font,
                            brush,
                            ClientRectangle,
                            format
                        );
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == (int)WindowMessage.WM_PAINT)
            {
                _DrawNothing();
            }
        }
    }
}
