/* TextBoxControlHost.cs -- allows to host control in TextBox
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM.Win32;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// <see cref="T:System.Windows.Forms.TextBox"/> 
    /// capable of hosting of arbitrary
    /// <see cref="T:System.Windows.Forms.Control"/>.
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    public class TextBoxControlHost
        : TextBox
    {
        #region Properties

        /// <summary>
        /// Gets the hosted control.
        /// </summary>
        [NotNull]
        [Browsable(false)]
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Hidden)]
        public Control Control { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:TextBoxControlHost"/> class.
        /// </summary>
        public TextBoxControlHost
            (
                [NotNull] Control control
            )
        {
            Code.NotNull(control, "control");

            if (control.Parent != null)
            {
                // TODO issue sensible message
                throw new ApplicationException();
            }

            Control = control;
            //Control.SizeChanged += _control_SizeChanged;
            Controls.Add(control);
        }

        #endregion

        #region Private members

        private bool _guard;

        ///// <summary>
        ///// Handles the SizeChanged event of the _control.
        ///// </summary>
        //private void _control_SizeChanged
        //    (
        //        object sender,
        //        EventArgs e
        //    )
        //{
        //    _PlaceControl();
        //}

        /// <summary>
        /// Places the hosted control.
        /// </summary>
        private void _PlaceControl()
        {
            if (_guard)
            {
                return;
            }

            try
            {
                _guard = true;
                int leftMargin;
                int rightMargin;
                Rectangle rectangle = GetMargins
                    (
                        Size,
                        out leftMargin,
                        out rightMargin
                    );
                User32.SendMessage
                    (
                        Handle,
                        WindowMessage.EM_SETMARGINS,
                        (int)(EditMargins.EC_LEFTMARGIN
                            | EditMargins.EC_RIGHTMARGIN),
                        (rightMargin << 16) + leftMargin
                    );
                Control.Location = rectangle.Location;
                Control.Size = rectangle.Size;
            }
            finally
            {
                _guard = false;
            }
        }

        /// <summary>
        /// Gets the <see cref="T:System.Windows.Forms.TextBox"/> 
        /// margins.
        /// </summary>
        /// <param name="parentSize">Size of the parent.</param>
        /// <param name="leftMargin">The left margin.</param>
        /// <param name="rightMargin">The right margin.</param>
        /// <returns>Calculated location and size of hosted control.
        /// </returns>
        protected virtual Rectangle GetMargins
            (
                Size parentSize,
                out int leftMargin,
                out int rightMargin
            )
        {
            leftMargin = 0;
            rightMargin = 0;
            return new Rectangle();
        }

        #endregion

        #region Control members

        /// <summary>
        /// Raises the handle created event.
        /// </summary>
        protected override void OnHandleCreated
            (
                EventArgs e
            )
        {
            base.OnHandleCreated(e);
            _PlaceControl();
        }

        /// <summary>
        /// Raises the 
        /// <see cref="E:System.Windows.Forms.Control.SizeChanged"/>
        /// event.
        /// </summary>
        protected override void OnSizeChanged
            (
                EventArgs e
            )
        {
            base.OnSizeChanged(e);
            _PlaceControl();
        }

        #endregion
    }
}
