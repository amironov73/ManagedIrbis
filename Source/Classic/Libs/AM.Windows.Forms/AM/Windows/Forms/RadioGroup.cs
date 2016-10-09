/* RadioGroup.cs -- Group of radio-buttons.
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
using AM.Collections;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Group of radio-buttons.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [System.ComponentModel.DesignerCategory("Code")]
    public class RadioGroup
        : GroupBox
    {
        private RadioButton[] _buttons = null;

        /// <summary>
        /// Called when current RadioButton changed.
        /// </summary>
        public delegate void CurrentChangedHandler
            (
                object sender,
                int current
            );

        /// <summary>
        /// Current RadioButton changed.
        /// </summary>
        [Browsable(true)]
        [Category("Property Changed")]
        [Description("Current RadioButton changed")]
        public event CurrentChangedHandler CurrentChanged;

        private const int DefaultLeftIndent = 5;
        private int _leftIndent = DefaultLeftIndent;

        /// <summary>
        /// Left indent.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultLeftIndent)]
        public virtual int LeftIndent
        {
            [DebuggerStepThrough]
            get
            {
                return _leftIndent;
            }
            [DebuggerStepThrough]
            set
            {
                if (_leftIndent != value)
                {
                    _leftIndent = value;
                    CreateButtons(Lines, _current);
                }
            }
        }

        private const int DefaultLineIndent = 0;
        private int _lineIndent = DefaultLineIndent;

        /// <summary>
        /// Line indent.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultLineIndent)]
        public virtual int LineIndent
        {
            [DebuggerStepThrough]
            get
            {
                return _lineIndent;
            }
            [DebuggerStepThrough]
            set
            {
                if (_lineIndent != value)
                {
                    _lineIndent = value;
                    CreateButtons(Lines, _current);
                }
            }
        }

        /// <summary>
        /// Button count.
        /// </summary>
        public virtual int Count
        {
            [DebuggerStepThrough]
            get
            {
                return _buttons == null
                           ? 0
                           : _buttons.Length;
            }
        }

        private const bool DefaultEvenly = true;
        private bool _evenly = DefaultEvenly;

        /// <summary>
        /// Property Evenly (bool)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultEvenly)]
        public virtual bool Evenly
        {
            [DebuggerStepThrough]
            get
            {
                return _evenly;
            }
            [DebuggerStepThrough]
            set
            {
                if (_evenly != value)
                {
                    _evenly = value;
                    CreateButtons(Lines, _current);
                }
            }
        }

        private const int DefaultCurrent = -1;
        private int _current = DefaultCurrent;
        private bool _inCurrent = false;

        /// <summary>
        /// Current button index.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultCurrent)]
        public virtual int Current
        {
            [DebuggerStepThrough]
            get
            {
                return _current;
            }
            [DebuggerStepThrough]
            set
            {
                if (_buttons != null)
                {
                    _inCurrent = true;
                    for (int i = 0; i < _buttons.Length; i++)
                    {
                        _buttons[i].Checked = (i == value);
                    }
                    _inCurrent = false;
                }
                if (_current != value)
                {
                    _current = value;
                    if (CurrentChanged != null)
                    {
                        CurrentChanged(this, _current);
                    }
                }
            }
        }

        /// <summary>
        /// Lines of text.
        /// </summary>
        public virtual string[] Lines
        {
            get
            {
                if (_buttons == null)
                {
                    return null;
                }
                string[] result = new string[_buttons.Length];
                for (int i = 0; i < _buttons.Length; i++)
                {
                    result[i] = _buttons[i].Text;
                }
                return result;
            }
            set
            {
                CreateButtons(value, _current);
            }
        }

        private void DeleteButtons()
        {
            if (_buttons == null)
            {
                return;
            }
            foreach (RadioButton button in _buttons)
            {
                Controls.Remove(button);
                button.Dispose();
            }
            _buttons = null;
        }

        private void CreateButtons
            (
                string[] lines,
                int currentPosition
            )
        {
            DeleteButtons();

            if (lines.IsNullOrEmpty())
            {
                return;
            }

            _buttons = new RadioButton[lines.Length];
            int topIndent = Font.Height * 3 / 2;
            int delta = (ClientSize.Height - topIndent) / lines.Length;
            for (int i = 0; i < lines.Length; i++)
            {
                RadioButton button = new RadioButton
                {
                    Text = lines[i],
                };
                button.Location = new Point
                    (
                        _leftIndent, 
                        (button.Height + _lineIndent) * i
                    );
                if (_evenly)
                {
                    button.Top = delta * i + topIndent;
                }
                button.Width = ClientSize.Width - button.Left - _leftIndent;
                button.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                | AnchorStyles.Right;
                button.Checked = (i == currentPosition);
                button.CheckedChanged += button_CheckedChanged;
                button.Visible = true;
                Controls.Add(button);
                _buttons[i] = button;
            }

            if (currentPosition != _current)
            {
                _current = currentPosition;
                if (CurrentChanged != null)
                {
                    CurrentChanged(this, _current);
                }
            }
        }

        private void button_CheckedChanged(object sender, EventArgs e)
        {
            if (_buttons == null)
            {
                return;
            }
            if (_inCurrent)
            {
                return;
            }

            int newCurrent = _current;
            for (int i = 0; i < _buttons.Length; i++)
            {
                if (_buttons[i] == null)
                {
                    break;
                }
                if (_buttons[i].Checked)
                {
                    newCurrent = i;
                    break;
                }
            }
            if (newCurrent != _current)
            {
                _current = newCurrent;
                if (CurrentChanged != null)
                {
                    CurrentChanged(this, _current);
                }
            }
        }
    }
}
