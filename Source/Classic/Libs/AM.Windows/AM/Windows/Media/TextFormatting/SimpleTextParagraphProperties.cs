// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SimpleTextParagraphProperties.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Media.TextFormatting
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SimpleTextParagraphProperties
        : TextParagraphProperties
    {
        #region Private members

        private FlowDirection _flowDirection;
        private TextAlignment _textAlignment;
        private double _lineHeight;
        private bool _firstLineInParagraph;
        private TextRunProperties _defaultTextRunProperties;
        private TextWrapping _textWrapping;
        private TextMarkerProperties _textMarkerProperties;
        private double _indent;

        #endregion

        #region TextParagraphProperties members

        /// <inheritdoc />
        public override FlowDirection FlowDirection
        {
            get { return _flowDirection; }
        }

        /// <inheritdoc />
        public override TextAlignment TextAlignment
        {
            get { return _textAlignment; }
        }

        /// <inheritdoc />
        public override double LineHeight
        {
            get { return _lineHeight; }
        }

        /// <inheritdoc />
        public override bool FirstLineInParagraph
        {
            get { return _firstLineInParagraph; }
        }

        /// <inheritdoc />
        public override TextRunProperties DefaultTextRunProperties
        {
            get { return _defaultTextRunProperties; }
        }

        /// <inheritdoc />
        public override TextWrapping TextWrapping
        {
            get { return _textWrapping; }
        }

        /// <inheritdoc />
        public override TextMarkerProperties TextMarkerProperties
        {
            get { return _textMarkerProperties; }
        }

        /// <inheritdoc />
        public override double Indent
        {
            get { return _indent; }
        }

        #endregion
    }
}
