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
        /// Constructor.
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

        /// <inheritdoc cref="Control.DefaultSize" />
        protected override Size DefaultSize
        {
            get
            {
                int height = 22;
                int width = height;
                return new Size(width, height);
            }
        }

        /// <inheritdoc cref="Control.Dispose(bool)" />
        protected override void Dispose
            (
                bool disposing
            )
        {
            base.Dispose(disposing);
            if (!ReferenceEquals(_form, null))
            {
                _form.InputLanguageChanged -= _InputLanguageChanged;
            }
        }

        /// <inheritdoc cref="Control.OnKeyDown" />
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

        /// <inheritdoc cref="Control.OnPaint" />
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
                CultureInfo culture
                    = InputLanguage.CurrentInputLanguage.Culture;
                string languageCode
                    = culture.TwoLetterISOLanguageName.ToUpperInvariant();
                g.DrawString(languageCode, Font, textBrush, r, format);
            }
            base.OnPaint(e);
        }

        /// <inheritdoc cref="Control.OnParentChanged" />
        protected override void OnParentChanged
            (
                EventArgs e
            )
        {
            base.OnParentChanged(e);
            _form = FindForm();
            if (!ReferenceEquals(_form, null))
            {
                _form.InputLanguageChanged += _InputLanguageChanged;
            }
        }

        #endregion
    }
}
