/* CollapsibleGroupBox.cs -- 
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// <see cref="GroupBox"/> that can be collapsed.
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    public class CollapsibleGroupBox
        : GroupBox
    {
        #region Events

        /// <summary>
        /// Raised when collapse action performed.
        /// </summary>
        public event EventHandler Collapse;

        #endregion

        #region Properties

        private bool _collapsed;

        /// <summary>
        /// Gets or sets a value indicating whether this 
        /// <see cref="CollapsibleGroupBox"/> is collapsed.
        /// </summary>
        /// <value><c>true</c> if collapsed; 
        /// otherwise, <c>false</c>.</value>
        [System.ComponentModel.DefaultValue(false)]
        public bool Collapsed
        {
            [DebuggerStepThrough]
            get
            {
                return _collapsed;
            }
            [DebuggerStepThrough]
            set
            {
                if (_collapsed != value)
                {
                    _Collapse(value);
                }
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="CollapsibleGroupBox"/> class.
        /// </summary>
        public CollapsibleGroupBox()
        {
            _savedState = new Dictionary<Control, bool>();
            MouseDoubleClick += _MouseDoubleClick;
        }

        #endregion

        #region Private members

        private int _savedHeight;
        private Dictionary<Control, bool> _savedState;

        private void _Collapse(bool coll)
        {
            if (coll)
            {
                _savedState.Clear();
                _savedHeight = Height;
                Height = FontHeight + 2;
            }
            else
            {
                Height = _savedHeight;
            }
            foreach (Control control in Controls)
            {
                if (coll)
                {
                    _savedState.Add(control, control.Enabled);
                    control.Enabled = false;
                }
                else
                {
                    if (_savedState.ContainsKey(control))
                    {
                        control.Enabled = _savedState[control];
                    }
                    else
                    {
                        control.Enabled = true;
                    }
                }
            }
            _collapsed = coll;
            if (Collapse != null)
            {
                Collapse(this, EventArgs.Empty);
            }
        }

        private void _MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Y < FontHeight)
            {
                _Collapse(!Collapsed);
            }
        }

        #endregion

        #region Control members

        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        protected override void OnPaintBackground
            (
                PaintEventArgs paintEvent
            )
        {
            base.OnPaintBackground(paintEvent);
            Graphics g = paintEvent.Graphics;
            Rectangle r = ClientRectangle;
            r.Height = FontHeight + 2;
            using (Brush b = new LinearGradientBrush
                (
                    r,
                    SystemColors.ControlDark,
                    SystemColors.ControlLight,
                    0f
                ))
            {
                g.FillRectangle(b, r);
            }
        }

        #endregion
    }
}