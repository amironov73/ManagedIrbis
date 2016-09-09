/* PenInfo.cs -- информация о пере.
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
    /// Информация о пере.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Editor(typeof(PenInfo.Editor), typeof(UITypeEditor))]
    public class PenInfo
    {
        #region Properties

        private const PenAlignment _DefaultAlignment = PenAlignment.Center;
        private PenAlignment _alignment = _DefaultAlignment;

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(_DefaultAlignment)]
        public PenAlignment Alignment
        {
            [DebuggerStepThrough]
            get
            {
                return _alignment;
            }
            [DebuggerStepThrough]
            set
            {
                _alignment = value;
            }
        }

        private const string _DefaultColor = "Black";
        private Color _color = Color.FromName(_DefaultColor);

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(typeof(Color), _DefaultColor)]
        public Color Color
        {
            [DebuggerStepThrough]
            get
            {
                return _color;
            }
            [DebuggerStepThrough]
            set
            {
                _color = value;
            }
        }

        private const DashCap _DefaultDashCap = DashCap.Flat;
        private DashCap _dashCap = _DefaultDashCap;

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(_DefaultDashCap)]
        public DashCap DashCap
        {
            [DebuggerStepThrough]
            get
            {
                return _dashCap;
            }
            [DebuggerStepThrough]
            set
            {
                _dashCap = value;
            }
        }

        private const DashStyle _DefaultDashStyle = DashStyle.Solid;
        private DashStyle _dashStyle = _DefaultDashStyle;

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(_DefaultDashStyle)]
        public DashStyle DashStyle
        {
            [DebuggerStepThrough]
            get
            {
                return _dashStyle;
            }
            [DebuggerStepThrough]
            set
            {
                _dashStyle = value;
            }
        }

        private const LineCap _DefaultEndCap = LineCap.Flat;
        private LineCap _endCap = _DefaultEndCap;

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(_DefaultEndCap)]
        public LineCap EndCap
        {
            [DebuggerStepThrough]
            get
            {
                return _endCap;
            }
            [DebuggerStepThrough]
            set
            {
                _endCap = value;
            }
        }

        private const LineJoin _DefaultLineJoin = LineJoin.Bevel;
        private LineJoin _lineJoin = _DefaultLineJoin;

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(_DefaultLineJoin)]
        public LineJoin LineJoin
        {
            [DebuggerStepThrough]
            get
            {
                return _lineJoin;
            }
            [DebuggerStepThrough]
            set
            {
                _lineJoin = value;
            }
        }

        private const LineCap _DefaultStartCap = LineCap.Flat;
        private LineCap _startCap = _DefaultStartCap;

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(_DefaultStartCap)]
        public LineCap StartCap
        {
            [DebuggerStepThrough]
            get
            {
                return _startCap;
            }
            [DebuggerStepThrough]
            set
            {
                _startCap = value;
            }
        }

        private const float _DefaultWidth = 1.0f;
        private float _width = _DefaultWidth;

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(_DefaultWidth)]
        public float Width
        {
            [DebuggerStepThrough]
            get
            {
                return _width;
            }
            [DebuggerStepThrough]
            set
            {
                _width = value;
            }
        }

        #endregion

        #region Public methods

        public virtual Pen GetPen()
        {
            Pen result = new Pen(Color, Width);
            result.Alignment = Alignment;
            result.DashStyle = DashStyle;
            result.EndCap = EndCap;
            result.LineJoin = LineJoin;
            result.StartCap = StartCap;
            result.DashCap = DashCap;
            return result;
        }

        #endregion

        #region Editor

        public class Editor : UITypeEditor
        {
            /// <summary>
            /// Constructor.
            /// </summary>
            public Editor()
            {
            }

            /// <inheritdoc />
            public override UITypeEditorEditStyle GetEditStyle(
                ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.DropDown;
            }

            /// <inheritdoc />
            public override object EditValue(
                ITypeDescriptorContext context,
                IServiceProvider provider, object value)
            {
                PenInfo pinfo = (PenInfo)value;

                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc != null)
                {
                    PenInfoControl form = new PenInfoControl(pinfo,
                        edSvc, context, provider);
                    edSvc.DropDownControl(form);
                    if (form.Result != null)
                    {
                        return form.Result;
                    }
                }

                //throw new NotImplementedException ();
                //return new BorderInfo ();
                return pinfo;
            }

            /// <inheritdoc />
            public override bool GetPaintValueSupported(ITypeDescriptorContext context)
            {
                return false;
            }
        }

        #endregion
    }
}
