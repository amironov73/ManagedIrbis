// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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

        private const bool DefaultBorder = true;
        private bool _drawBorder = DefaultBorder;

        ///<summary>
        /// Рисовать границу или нет.
        ///</summary>
        [DefaultValue(DefaultBorder)]
        [Description("Draw border or not")]
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

        private const bool Default3D = true;
        private bool _draw3D = Default3D;

        ///<summary>
        /// Граница трехмерная или нет.
        ///</summary>
        [DefaultValue(Default3D)]
        [Description("Whether border is 3D?")]
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

        private const ButtonBorderStyle DefaultStyle2D =
            ButtonBorderStyle.Solid;
        private ButtonBorderStyle _style2D = DefaultStyle2D;

        ///<summary>
        /// Стиль двухмерной границы.
        ///</summary>
        [DefaultValue(DefaultStyle2D)]
        [Description("Style of 2D border")]
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

        private const string DefaultColor = "Black";
        private Color _borderColor = Color.FromName(DefaultColor);

        ///<summary>
        /// Цвет двухмерной границы.
        ///</summary>
        [DefaultValue(typeof(Color), DefaultColor)]
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

        private const Border3DStyle DefaultStyle3D =
            Border3DStyle.Etched;
        private Border3DStyle _style3D = DefaultStyle3D;

        ///<summary>
        /// Стиль трехмерной границы.
        ///</summary>
        [DefaultValue(DefaultStyle3D)]
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

        /// <summary>
        /// Editor for <see cref="BorderInfo"/>.
        /// </summary>
        public class Editor : UITypeEditor
        {
            /// <inheritdoc />
            public override UITypeEditorEditStyle GetEditStyle
                (
                    ITypeDescriptorContext context
                )
            {
                return UITypeEditorEditStyle.DropDown;
            }

            /// <inheritdoc />
            public override object EditValue
                (
                    ITypeDescriptorContext context,
                    IServiceProvider provider,
                    object value
                )
            {
                BorderInfo borderInfo = (BorderInfo)value;

                if (ReferenceEquals(provider, null))
                {
                    return borderInfo;
                }

                IWindowsFormsEditorService edSvc
                    = (IWindowsFormsEditorService)provider.GetService
                    (
                        typeof(IWindowsFormsEditorService)
                    );
                if (edSvc != null)
                {
                    BorderInfoControl form = new BorderInfoControl
                        (
                            borderInfo,
                            edSvc,
                            context,
                            provider
                        );
                    edSvc.DropDownControl(form);

                    if (form.Result != null)
                    {
                        return form.Result;
                    }
                }

                return borderInfo;
            }

            /// <inheritdoc />
            public override void PaintValue
                (
                    PaintValueEventArgs e
                )
            {
                Graphics g = e.Graphics;
                using (Font font = new Font("Arial", 8))
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    g.DrawString("Not implemented", font, brush, 0, 0);
                }
            }

            /// <inheritdoc />
            public override bool GetPaintValueSupported
                (
                    ITypeDescriptorContext context
                )
            {
                return false;
            }
        }

        #endregion
    }
}
