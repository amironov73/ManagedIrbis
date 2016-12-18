// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SimplestTextPrinter.cs -- 
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Drawing.Printing
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [System.ComponentModel.DesignerCategory("Code")]
    public class PlainTextPrinter
        : TextPrinter
    {
        #region Events

        #endregion

        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        private string _text;
        private int _offset;

        /// <summary>
        /// Called when [print page].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="T:System.Drawing.Printing.PrintPageEventArgs"/> instance containing the event data.</param>
        protected override void OnPrintPage
            (
                object sender,
                PrintPageEventArgs e
            )
        {
            base.OnPrintPage(sender, e);

            Graphics g = e.Graphics;
            string s = _text.Substring(_offset);
            using (Brush brush = new SolidBrush(TextColor))
            using (StringFormat format = new StringFormat())
            {
                format.Alignment = StringAlignment.Near;
                format.LineAlignment = StringAlignment.Near;
                format.HotkeyPrefix = HotkeyPrefix.None;
                format.Trimming = StringTrimming.Word;
                format.FormatFlags = StringFormatFlags.LineLimit;
                RectangleF rect = e.PageBounds;
                rect.X += Borders.Left;
                rect.Width -= (Borders.Left + Borders.Right);
                rect.Y += Borders.Top;
                rect.Height -= (Borders.Top + Borders.Bottom);
                rect.Height = (rect.Height / TextFont.Size) * TextFont.Size;
                g.DrawString(s, TextFont, brush, rect, format);
                int charFitted, linesFilled;
                g.MeasureString(s, TextFont, rect.Size, format, out charFitted, out linesFilled);
                e.HasMorePages = (charFitted < s.Length);
                _offset += charFitted;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Prints the specified text.
        /// </summary>
        public override bool Print
            (
                string text
            )
        {
            _offset = 0;
            _text = text;

            return base.Print(text);
        }

        #endregion
    }
}
