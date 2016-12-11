// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CheckedGroupBox.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// <see cref="GroupBox"/> with <see cref="CheckBox"/>
    /// in caption.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class CheckedGroupBox
        : GroupBox
    {
        #region Events

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CheckedChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this <see cref="CheckedGroupBox"/> 
        /// is checked.
        /// </summary>
        /// <value><c>true</c> if checked; 
        /// otherwise, <c>false</c>.</value>
        [DefaultValue(true)]
        public bool Checked
        {
            [DebuggerStepThrough]
            get
            {
                return _checkBox.Checked;
            }
            [DebuggerStepThrough]
            set
            {
                _checkBox.Checked = value;
            }
        }

        /// <summary>
        /// </summary>
        /// <value></value>
        public override string Text
        {
            [DebuggerStepThrough]
            get
            {
                return _checkBox.Text;
            }
            [DebuggerStepThrough]
            set
            {
                _checkBox.Text = value;
                OnTextChanged(EventArgs.Empty);
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckedGroupBox"/> class.
        /// </summary>
        public CheckedGroupBox()
        {
            _savedState = new Dictionary<Control, bool>();
            _checkBox = new CheckBox
            {
                Text = base.Text,
                Left = 5,
                AutoSize = true,
                ForeColor = SystemColors.ControlText,
                Checked = true,
                ThreeState = false,
                Parent = this
            };
            _checkBox.CheckedChanged += _checkBox_CheckedChanged;
        }

        #endregion

        #region Private members

        private readonly CheckBox _checkBox;
        private readonly Dictionary<Control, bool> _savedState;

        private void _checkBox_CheckedChanged
            (
                object sender,
                EventArgs e
            )
        {
            if (!_checkBox.Checked)
            {
                _savedState.Clear();
            }
            foreach (Control control in Controls)
            {
                if (control != _checkBox)
                {
                    if (_checkBox.Checked)
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
                    else
                    {
                        _savedState.Add(control, control.Enabled);
                        control.Enabled = false;
                    }
                }
            }
            CheckedChanged.Raise(this, e);
        }

        #endregion
    }
}