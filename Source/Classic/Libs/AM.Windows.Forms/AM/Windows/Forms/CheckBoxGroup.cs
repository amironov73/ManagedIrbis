// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CheckBoxGroup.cs -- 
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [System.ComponentModel.DesignerCategory("Code")]
    public class CheckBoxGroup
        : GroupBox
    {
        private CheckBox[] _buttons = null;

        /// <summary>
        /// Called when current RadioButton changed.
        /// </summary>
        public delegate void CurrentChangedHandler
            (
                object sender,
                long current
            );

        /// <summary>
        /// Current CheckButton changed.
        /// </summary>
        [Browsable(true)]
        [Category("Property Changed")]
        [Description("Current CheckButton changed")]
        public event CurrentChangedHandler CurrentChanged;

        private const int DefaultLeftIndent = 5;
        private int _leftIndent = DefaultLeftIndent;

        /// <summary>
        /// Left indent value.
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
                    _CreateButtons(Lines, _current);
                }
            }
        }

        private const int DefaultLineIndent = 0;
        private int _lineIndent = DefaultLineIndent;

        /// <summary>
        /// Line indent value.
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
                    _CreateButtons(Lines, _current);
                }
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
                    _CreateButtons(Lines, _current);
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
                return (_buttons == null)
                           ? 0
                           : _buttons.Length;
            }
        }

        private const long DefaultCurrent = 0;
        private long _current = DefaultCurrent;
        private bool _inCurrent = false;

        /// <summary>
        /// Current CheckBox selection.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultCurrent)]
        public virtual long Current
        {
            [DebuggerStepThrough]
            get
            {
                return _current;
            }
            set
            {
                if (_buttons != null)
                {
                    _inCurrent = true;
                    long mask = 1;
                    for (int i = 0; i < _buttons.Length; i++, mask <<= 1)
                    {
                        _buttons[i].Checked = ((mask & value) != 0);
                    }
                    _inCurrent = false;
                }
                if (_current != value)
                {
                    _current = value;

                    CurrentChangedHandler handler = CurrentChanged;
                    if (!ReferenceEquals(handler, null))
                    {
                        handler(this, _current);
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
                _CreateButtons(value, _current);
            }
        }

        /// <summary>
        /// Checked buttons
        /// </summary>
        public virtual bool[] Checked
        {
            get
            {
                if (_buttons == null)
                {
                    return null;
                }

                long mask = 1;
                bool[] result = new bool[_buttons.Length];

                for (int i = 0; i < _buttons.Length; i++, mask <<= 1)
                {
                    result[i] = _buttons[i].Checked;
                }
                return result;
            }
            set
            {
                long curpos = 0,
                     mask = 1;

                if (_buttons != null)
                {
                    _inCurrent = true;
                    for (int i = 0; i < _buttons.Length; i++, mask <<= 1)
                    {
                        if (value[i])
                        {
                            curpos |= mask;
                        }
                        _buttons[i].Checked = value[i];
                    }
                    _inCurrent = false;
                }
                if (_current != curpos)
                {
                    _current = curpos;

                    CurrentChangedHandler handler = CurrentChanged;
                    if (!ReferenceEquals(handler, null))
                    {
                        handler(this, _current);
                    }
                }
            }
        }

        private const Appearance DefaultAppearance = Appearance.Normal;
        private Appearance _appearance = DefaultAppearance;

        /// <summary>
        /// Appearance.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultAppearance)]
        public virtual Appearance Appearance
        {
            [DebuggerStepThrough]
            get
            {
                return _appearance;
            }
            [DebuggerStepThrough]
            set
            {
                if (_appearance != value)
                {
                    if (_buttons == null)
                    {
                        return;
                    }
                    foreach (CheckBox button in _buttons)
                    {
                        button.Appearance = value;
                    }
                    _appearance = value;
                }
            }
        }

        private void _DeleteButtons()
        {
            if (_buttons == null)
            {
                return;
            }
            foreach (CheckBox button in _buttons)
            {
                Controls.Remove(button);
                button.Dispose();
            }
            _buttons = null;
        }

        private void _CreateButtons(string[] lines, long curpos)
        {
            _DeleteButtons();
            if ((lines == null)
                 || (lines.Length == 0))
            {
                return;
            }
            _buttons = new CheckBox[lines.Length];
            long mask = 1;
            int topIndent = Font.Height * 3 / 2;
            int delta = (ClientSize.Height - topIndent) / lines.Length;
            for (int i = 0; i < lines.Length; i++, mask <<= 1)
            {
                CheckBox button = new CheckBox();
                button.Text = lines[i];
                button.Location =
                    new Point(_leftIndent, (button.Height + _lineIndent) * i);
                if (_evenly)
                {
                    button.Top = delta * i + topIndent;
                }
                button.Width = ClientSize.Width - button.Left - _leftIndent;
                button.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                | AnchorStyles.Right;
                button.ThreeState = false;
                button.Checked = ((curpos & mask) != 0);
                button.CheckedChanged += new EventHandler(button_CheckedChanged);
                button.Visible = true;
                Controls.Add(button);
                _buttons[i] = button;
            }
            if (curpos != _current)
            {
                _current = curpos;

                CurrentChangedHandler handler = CurrentChanged;
                if (!ReferenceEquals(handler, null))
                {
                    handler(this, _current);
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

            long newcurrent = 0;
            long mask = 1;
            for (int i = 0; i < _buttons.Length; i++, mask <<= 1)
            {
                if (_buttons[i] == null)
                {
                    break;
                }
                if (_buttons[i].Checked)
                {
                    newcurrent |= mask;
                }
            }
            if (newcurrent != _current)
            {
                _current = newcurrent;

                CurrentChangedHandler handler = CurrentChanged;
                if (!ReferenceEquals(handler, null))
                {
                    handler(this, _current);
                }
            }
        }
    }
}