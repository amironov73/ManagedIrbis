/* BorderInfo.cs -- Информация о границе контрола.
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
    /// <summary>
    /// Информация о границе контрола или его части.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Editor(typeof(BorderInfo.Editor), typeof(UITypeEditor))]
    public class BorderInfo
    {
        #region Properties

        private const bool _DefaultBorder = true;
        private bool _drawBorder = _DefaultBorder;

        ///<summary>
        /// Рисовать границу или нет.
        ///</summary>
        [DefaultValue(_DefaultBorder)]
        [Description("Draw border or not.")]
        public bool DrawBorder
        {
            [DebuggerStepThrough]
            get
            {
                return _drawBorder;
            }
            [DebuggerStepThrough]
            set
            {
                _drawBorder = value;
            }
        }

        private const bool _Default3D = true;
        private bool _draw3D = _Default3D;

        ///<summary>
        /// Граница трехмерная или нет.
        ///</summary>
        [DefaultValue(_Default3D)]
        [Description("Is border 3D.")]
        public bool Draw3D
        {
            [DebuggerStepThrough]
            get
            {
                return _draw3D;
            }
            [DebuggerStepThrough]
            set
            {
                _draw3D = value;
            }
        }

        private const ButtonBorderStyle _DefaultStyle2D =
            ButtonBorderStyle.Solid;
        private ButtonBorderStyle _style2D = _DefaultStyle2D;

        ///<summary>
        /// Стиль двухмерной границы.
        ///</summary>
        [DefaultValue(_DefaultStyle2D)]
        [Description("Style of 2D border.")]
        public ButtonBorderStyle Style2D
        {
            [DebuggerStepThrough]
            get
            {
                return _style2D;
            }
            [DebuggerStepThrough]
            set
            {
                _style2D = value;
            }
        }

        private const string _DefaultColor = "Black";
        private Color _borderColor = Color.FromName(_DefaultColor);

        ///<summary>
        /// Цвет двухмерной границы.
        ///</summary>
        [DefaultValue(typeof(Color), _DefaultColor)]
        [Description("Color for 2D border.")]
        public Color BorderColor
        {
            [DebuggerStepThrough]
            get
            {
                return _borderColor;
            }
            [DebuggerStepThrough]
            set
            {
                _borderColor = value;
            }
        }

        private const Border3DStyle _DefaultStyle3D =
            Border3DStyle.Etched;
        private Border3DStyle _style3D = _DefaultStyle3D;

        ///<summary>
        /// Стиль трехмерной границы.
        ///</summary>
        [DefaultValue(_DefaultStyle3D)]
        [Description("Style for 3D border")]
        public Border3DStyle Style3D
        {
            [DebuggerStepThrough]
            get
            {
                return _style3D;
            }
            [DebuggerStepThrough]
            set
            {
                _style3D = value;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Отрисовка границы контрола.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        public void Draw(Graphics g, Rectangle r)
        {
            if (DrawBorder)
            {
                if (Draw3D)
                {
                    ControlPaint.DrawBorder3D(g, r, Style3D);
                }
                else
                {
                    ControlPaint.DrawBorder(g, r, BorderColor, Style2D);
                }
            }
        }

        #endregion

        #region Designer

        public class Editor : UITypeEditor
        {
            private BorderInfo _bi;

            public Editor()
            {
            }

            public Editor(BorderInfo bi)
            {
                _bi = bi;
            }

            public override UITypeEditorEditStyle GetEditStyle(
                ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.DropDown;
            }

            public override object EditValue(
                ITypeDescriptorContext context,
                IServiceProvider provider, object value)
            {
                BorderInfo binfo = (BorderInfo)value;

                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc != null)
                {
                    BorderInfoControl form = new BorderInfoControl(binfo,
                        edSvc, context, provider);
                    edSvc.DropDownControl(form);
                    if (form.Result != null)
                    {
                        return form.Result;
                    }
                }

                //throw new NotImplementedException ();
                //return new BorderInfo ();
                return binfo;
            }

            public override void PaintValue(PaintValueEventArgs e)
            {
                Graphics g = e.Graphics;
                using (Font font = new Font("Arial", 8))
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    g.DrawString("Not implemented", font, brush, 0, 0);
                }
                //_bi.Draw ( g, e.Bounds );
            }

            public override bool GetPaintValueSupported(ITypeDescriptorContext context)
            {
                return false;
            }
        }

        #endregion
    }
}
