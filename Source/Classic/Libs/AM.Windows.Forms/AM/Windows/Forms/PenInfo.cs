// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PenInfo.cs -- информация о пере.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms.Design;

using JetBrains.Annotations;

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

        private const PenAlignment DefaultAlignment = PenAlignment.Center;
        private PenAlignment _alignment = DefaultAlignment;

        ///<summary>
        /// Alingnment.
        ///</summary>
        [DefaultValue(DefaultAlignment)]
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

        private const string DefaultColor = "Black";
        private Color _color = Color.FromName(DefaultColor);

        ///<summary>
        /// Color.
        ///</summary>
        [DefaultValue(typeof(Color), DefaultColor)]
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

        private const DashCap DefaultDashCap = DashCap.Flat;
        private DashCap _dashCap = DefaultDashCap;

        ///<summary>
        /// Dash.
        ///</summary>
        [DefaultValue(DefaultDashCap)]
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

        private const DashStyle DefaultDashStyle = DashStyle.Solid;
        private DashStyle _dashStyle = DefaultDashStyle;

        ///<summary>
        /// Dash.
        ///</summary>
        [DefaultValue(DefaultDashStyle)]
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

        private const LineCap DefaultEndCap = LineCap.Flat;
        private LineCap _endCap = DefaultEndCap;

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(DefaultEndCap)]
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

        private const LineJoin DefaultLineJoin = LineJoin.Bevel;
        private LineJoin _lineJoin = DefaultLineJoin;

        ///<summary>
        /// Line join.
        ///</summary>
        [DefaultValue(DefaultLineJoin)]
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

        private const LineCap DefaultStartCap = LineCap.Flat;
        private LineCap _startCap = DefaultStartCap;

        ///<summary>
        /// 
        ///</summary>
        [DefaultValue(DefaultStartCap)]
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

        private const float DefaultWidth = 1.0f;
        private float _width = DefaultWidth;

        ///<summary>
        /// Pen width.
        ///</summary>
        [DefaultValue(DefaultWidth)]
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

        /// <summary>
        /// Convert <see cref="PenInfo"/> to <see cref="Pen"/>.
        /// </summary>
        [NotNull]
        public virtual Pen ToPen()
        {
            Pen result = new Pen(Color, Width)
            {
                Alignment = Alignment,
                DashStyle = DashStyle,
                EndCap = EndCap,
                LineJoin = LineJoin,
                StartCap = StartCap,
                DashCap = DashCap
            };

            return result;
        }

        #endregion

        #region Editor

        /// <summary>
        /// Editor for <see cref="PenInfo"/>.
        /// </summary>
        public class Editor
            : UITypeEditor
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
                PenInfo penInfo = (PenInfo)value;

                if (ReferenceEquals(provider, null))
                {
                    return penInfo;
                }

                IWindowsFormsEditorService edSvc
                    = (IWindowsFormsEditorService)provider.GetService
                    (
                        typeof(IWindowsFormsEditorService)
                    );
                if (edSvc != null)
                {
                    PenInfoControl form = new PenInfoControl
                        (
                            penInfo,
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

                return penInfo;
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
