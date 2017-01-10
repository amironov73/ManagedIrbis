// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldPainter.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FieldPainter
        : IDisposable
    {
        #region Nested classes

        class TextSegment
        {
            #region Properties

            public bool Code { get; private set; }

            public string Text { get; private set; }

            #endregion

            #region Construction

            public TextSegment(bool code, string text)
            {
                Code = code;
                Text = text;
            }

            #endregion

            #region Object members

            public override string ToString()
            {
                return string.Format
                    (
                        "Code: {0}, Text: {1}",
                        Code,
                        Text
                    );
            }

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Color of hat sign and code letter.
        /// </summary>
        public Color CodeColor
        {
            get { return _codeColor; }
            set
            {
                _codeColor = value;

                if (!ReferenceEquals(_codeBrush, null))
                {
                    _codeBrush.Dispose();
                }

                _codeBrush = new SolidBrush(value);
            }
        }

        /// <summary>
        /// Color of ordinary text.
        /// </summary>
        public Color TextColor
        {
            get { return _textColor; }
            set
            {
                _textColor = value;

                if (!ReferenceEquals(_textBrush, null))
                {
                    _textBrush.Dispose();
                }

                _textBrush = new SolidBrush(value);
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldPainter()
            : this(Color.Red, Color.Black)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldPainter
            (
                Color codeColor,
                Color textColor
            )
        {
            CodeColor = codeColor;
            TextColor = textColor;
        }

        #endregion

        #region Private members

        private Color _codeColor, _textColor;

        private Brush _codeBrush, _textBrush;

        private static TextSegment[] _SplitText
            (
                [NotNull] string text
            )
        {
            List<TextSegment> result = new List<TextSegment>();

            int start = 0, offset = 0, length = text.Length;
            TextSegment segment;

            while (offset < length)
            {
                if (text[offset] == '^')
                {
                    if (offset != start)
                    {
                        segment = new TextSegment
                            (
                                false,
                                text.Substring(start, offset - start)
                            );
                        result.Add(segment);
                    }

                    if (offset + 1 < length)
                    {
                        segment = new TextSegment
                            (
                                true,
                                text.Substring(offset, 2)
                            );
                        result.Add(segment);

                        start = offset + 2;
                        offset++;
                    }
                    else
                    {
                        segment = new TextSegment
                            (
                                true,
                                text.Substring(offset, 1)
                            );
                        result.Add(segment);

                        start = offset + 1;
                    }
                }

                offset++;
            }

            if (offset != start)
            {
                segment = new TextSegment
                    (
                        false,
                        text.Substring(start, offset - start)
                    );
                result.Add(segment);
            }

            return result.ToArray();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Draw line of field text.
        /// </summary>
        public void DrawLine
            (
                [NotNull] Graphics graphics,
                [NotNull] Font font,
                PointF position,
                [CanBeNull] string text
            )
        {
            Code.NotNull(graphics, "graphics");
            Code.NotNull(font, "font");

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            using (StringFormat format = new StringFormat())
            {
                format.Alignment = StringAlignment.Near;
                format.FormatFlags = StringFormatFlags.NoWrap
                    | StringFormatFlags.MeasureTrailingSpaces;

                SizeF em = graphics.MeasureString
                    (
                        "m",
                        font,
                        position,
                        format
                    );
                float em6 = em.Width / 5f;

                TextSegment[] segments = _SplitText(text);
                foreach (TextSegment segment in segments)
                {
                    SizeF size = graphics.MeasureString
                        (
                            segment.Text,
                            font,
                            position,
                            format
                        );

                    Brush brush = segment.Code
                        ? _codeBrush
                        : _textBrush;

                    graphics.DrawString
                        (
                            segment.Text,
                            font,
                            brush,
                            position,
                            format
                        );

                    position.X += size.Width - em6;
                }
            }
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!ReferenceEquals(_codeBrush, null))
            {
                _codeBrush.Dispose();
                _codeBrush = null;
            }

            if (!ReferenceEquals(_textBrush, null))
            {
                _textBrush.Dispose();
                _textBrush = null;
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format
                (
                    "CodeColor: {0}, TextColor: {1}",
                    CodeColor,
                    TextColor
                );
        }

        #endregion
    }
}
