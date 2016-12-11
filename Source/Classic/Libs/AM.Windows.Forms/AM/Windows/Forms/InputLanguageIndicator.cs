// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* InputLanguageIndicator.cs -- input language indicator
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Input language indicator.
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    public sealed class InputLanguageIndicator
        : Control
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="InputLanguageIndicator"/> class.
        /// </summary>
        public InputLanguageIndicator()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Selectable, true);

            ForeColor = Color.White;
            BackColor = Color.Blue;

            ContextMenu = new ContextMenu();
            foreach (InputLanguage language
                in InputLanguage.InstalledInputLanguages)
            {
                CultureInfo culture = language.Culture;
                string menuText = string.Format
                    (
                    "{0} {1}",
                    culture.TwoLetterISOLanguageName.ToUpperInvariant(),
                    language.LayoutName
                    );
                ContextMenu.MenuItems.Add(menuText, _MenuClick);
            }
        }

        #endregion

        #region Private members

        private Form _form;

        private void _MenuClick
            (
                object sender,
                EventArgs ea
            )
        {
            MenuItem item = (MenuItem)sender;
            int index = ContextMenu.MenuItems.IndexOf(item);
            InputLanguage.CurrentInputLanguage
                = InputLanguage.InstalledInputLanguages[index];
            Application.DoEvents();
            Invalidate();
        }

        private void _InputLanguageChanged
            (
                object sender,
                InputLanguageChangedEventArgs e
            )
        {
            Invalidate();
        }

        private void _ShowContextMenu()
        {
            ContextMenu.Show(this, new Point(0, 0));
        }

        #endregion

        #region Control members

        ///<summary>
        ///Gets the default size of the control.
        ///</summary>
        protected override Size DefaultSize
        {
            get
            {
                int height = 22;
                int width = height;
                return new Size(width, height);
            }
        }

        ///<summary>
        ///Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control"></see> and its child controls and optionally releases the managed resources.
        ///</summary>
        protected override void Dispose
            (
                bool disposing
            )
        {
            base.Dispose(disposing);
            if (_form != null)
            {
                _form.InputLanguageChanged -= _InputLanguageChanged;
            }
        }

        /// <summary>
        /// Raises the
        /// <see cref="E:System.Windows.Forms.Control.KeyDown"/>
        /// event.
        /// </summary>
        protected override void OnKeyDown
            (
                KeyEventArgs e
            )
        {
            base.OnKeyDown(e);
            switch (e.KeyData)
            {
                case Keys.Space:
                case Keys.Enter:
                    _ShowContextMenu();
                    break;
            }
        }

        /// <summary>
        /// Raises the
        /// <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        protected override void OnPaint
            (
                PaintEventArgs e
            )
        {
            Graphics g = e.Graphics;
            Rectangle r = ClientRectangle;
            using (Brush textBrush = new SolidBrush(ForeColor))
            using (Brush backBrush = new SolidBrush(BackColor))
            using (StringFormat format = new StringFormat())
            {
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                g.FillRectangle(backBrush, r);
                CultureInfo culture = InputLanguage.CurrentInputLanguage.Culture;
                string languageCode
                    = culture.TwoLetterISOLanguageName.ToUpperInvariant();
                g.DrawString(languageCode, Font, textBrush, r, format);
            }
            base.OnPaint(e);
        }

        ///<summary>
        ///Raises the <see cref="E:System.Windows.Forms.Control.ParentChanged"></see> event.
        ///</summary>
        protected override void OnParentChanged
            (
                EventArgs e
            )
        {
            base.OnParentChanged(e);
            _form = FindForm();
            if (_form != null)
            {
                _form.InputLanguageChanged += _InputLanguageChanged;
            }
        }

        #endregion
    }
}
