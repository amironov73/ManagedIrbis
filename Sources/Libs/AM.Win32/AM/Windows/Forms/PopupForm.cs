/* PopupForm.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Windows.Forms;

using AM.Drawing;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PopupForm
        : Form
    {
        #region Events

        /// <summary>
        /// Fired when this instance appeared on the screen.
        /// </summary>
        public event EventHandler Appeared;

        /// <summary>
        /// Fired when this instance timed out.
        /// </summary>
        public event EventHandler TimedOut;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the embedded Web browser.
        /// </summary>
        /// <value>The browser.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public WebBrowser Browser
        {
            [DebuggerStepThrough]
            get
            {
                return _browser;
            }
        }

        private double _deactivatedOpacity = 0.7;

        /// <summary>
        /// Gets or sets the deactivated opacity.
        /// </summary>
        /// <value>The deactivated opacity.</value>
        [DefaultValue(0.7)]
        public double DeactivatedOpacity
        {
            [DebuggerStepThrough]
            get
            {
                return _deactivatedOpacity;
            }
            [DebuggerStepThrough]
            set
            {
                _deactivatedOpacity = value;
            }
        }

        /// <summary>
        /// Gets or sets the message text.
        /// </summary>
        /// <value>The message text.</value>
        public string MessageText
        {
            get
            {
                return _browser.DocumentText;
            }
            set
            {
                _browser.DocumentText = value;
            }
        }

        private Size _indent = new Size(5, 5);

        /// <summary>
        /// Gets or sets the indent.
        /// </summary>
        /// <value>The indent.</value>
        [DefaultValue(typeof(Size), "5;5")]
        public Size Indent
        {
            [DebuggerStepThrough]
            get
            {
                return _indent;
            }
            [DebuggerStepThrough]
            set
            {
                _indent = value;
            }
        }

        private WindowPlacement _placement = WindowPlacement.BottomRightCorner;

        /// <summary>
        /// Gets or sets the placement.
        /// </summary>
        /// <value>The placement.</value>
        [CLSCompliant(false)]
        [DefaultValue(WindowPlacement.BottomRightCorner)]
        public WindowPlacement Placement
        {
            [DebuggerStepThrough]
            get
            {
                return _placement;
            }
            [DebuggerStepThrough]
            set
            {
                _placement = value;
            }
        }

        private SystemSound _sound;

        /// <summary>
        /// Gets or sets the sound.
        /// </summary>
        /// <value>The sound.</value>
        [DefaultValue(null)]
        public SystemSound Sound
        {
            [DebuggerStepThrough]
            get
            {
                return _sound;
            }
            [DebuggerStepThrough]
            set
            {
                _sound = value;
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public override string Text
        {
            [DebuggerStepThrough]
            get
            {
                return _titleLabel == null
                    ? null
                    : _titleLabel.Text;
            }
            [DebuggerStepThrough]
            set
            {
                if (_titleLabel != null)
                {
                    _titleLabel.Text = value;
                }
            }
        }

        private int _timeout;

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>The timeout.</value>
        [DefaultValue(0)]
        public int Timeout
        {
            [DebuggerStepThrough]
            get
            {
                return _timeout;
            }
            [DebuggerStepThrough]
            set
            {
                _timeout = value;
                if (value <= 0)
                {
                    _timeoutTimer.Stop();
                }
                else
                {
                    _timeoutTimer.Tick += _timeoutTimer_Tick;
                    _timeoutTimer.Interval = value;
                    _timeoutTimer.Start();
                }
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="PopupForm"/> class.
        /// </summary>
        public PopupForm()
        {
            InitializeComponent();
            Region = new Region(_GetPath());
        }

        #endregion

        #region Private members

        /// <summary>
        /// Handles the Tick event of the _appearanceTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> 
        /// instance containing the event data.</param>
        private void _appearanceTimer_Tick(object sender, EventArgs e)
        {
            _appearanceTimer.Enabled = false;
            OnAppeared(EventArgs.Empty);
        }

        private void _browser_PreviewKeyDown
            (
            object sender,
            PreviewKeyDownEventArgs e
            )
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    e.IsInputKey = false;
                    Close();
                    break;
            }
        }

        private GraphicsPath _GetPath()
        {
            return PaintUtility.CreateRoundedRectanglePath
                (
                Bounds,
                new Size(Width / 6, Height / 3)
                );
        }

        private void _timeoutTimer_Tick(object sender, EventArgs e)
        {
            OnTimedOut(e);
        }

        /// <summary>
        /// Raises the <see cref="Appeared"/> event.
        /// </summary>
        /// <param name="ea">The <see cref="System.EventArgs"/> 
        /// instance containing the event data.</param>
        protected virtual void OnAppeared(EventArgs ea)
        {
            EventHandler handler = Appeared;
            if (handler != null)
            {
                handler(this, ea);
            }
        }

        /// <summary>
        /// Raises the <see cref="TimedOut"/> event.
        /// </summary>
        /// <param name="ea">The <see cref="System.EventArgs"/> 
        /// instance containing the event data.</param>
        protected virtual void OnTimedOut(EventArgs ea)
        {
            EventHandler handler = TimedOut;
            if (handler != null)
            {
                handler(this, ea);
            }
            Close();
        }

        #endregion

        #region Form members

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Activated"/>
        /// event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/>
        /// that contains the event data.</param>
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            Opacity = 1.0;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Deactivate"/>
        /// event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/>
        /// that contains the event data.</param>
        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            Opacity = DeactivatedOpacity;
        }

        /// <inheritdoc />
        protected override void OnKeyDown
            (
                KeyEventArgs e
            )
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    e.Handled = true;
                    Close();
                    break;
            }

            base.OnKeyDown(e);
        }

        /// <inheritdoc />
        protected override void OnPaint
            (
                PaintEventArgs e
            )
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            GraphicsPath path = _GetPath();
            g.DrawPath(Pens.Black, path);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Popups this instance.
        /// </summary>
        public void Popup()
        {
            Location = FormUtility.CalculatePlacement
                (
                    Size,
                    Placement,
                    Indent
                );
            Opacity = DeactivatedOpacity;

            Win32FormUtility.ShowNoActivate(this);
        }

        /// <summary>
        /// Popups the form with specified title and message text.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message text.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>Created form.</returns>
        public static PopupForm Popup
            (
                [NotNull] string title,
                [NotNull] string message,
                int timeout
            )
        {
            Code.NotNullNorEmpty(title, "title");
            Code.NotNullNorEmpty(message, "message");

            PopupForm result = new PopupForm
            {
                Text = title,
                MessageText = message,
                Timeout = timeout
            };
            result.Popup();

            return result;
        }

        #endregion
    }
}
