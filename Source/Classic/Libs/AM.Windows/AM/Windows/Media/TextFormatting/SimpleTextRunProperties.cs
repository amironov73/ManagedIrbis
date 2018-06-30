// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SimpleTextRunProperties.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
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
    public sealed class SimpleTextRunProperties
        : TextRunProperties
    {
        #region Private members

        private Typeface _typeface;
        private double _fontRenderingEmSize;
        private double _fontHintingEmSize;
        private TextDecorationCollection _textDecorations;
        private Brush _foregroundBrush;
        private Brush _backgroundBrush;
        private CultureInfo _cultureInfo;
        private TextEffectCollection _textEffects;

        #endregion

        #region TextRunProperties members

        /// <inheritdoc />
        public override Typeface Typeface
        {
            get { return _typeface; }
        }

        /// <inheritdoc />
        public override double FontRenderingEmSize
        {
            get { return _fontRenderingEmSize; }
        }

        /// <inheritdoc />
        public override double FontHintingEmSize
        {
            get { return _fontHintingEmSize; }
        }

        /// <inheritdoc />
        public override TextDecorationCollection TextDecorations
        {
            get { return _textDecorations; }
        }

        /// <inheritdoc />
        public override Brush ForegroundBrush
        {
            get { return _foregroundBrush; }
        }

        /// <inheritdoc />
        public override Brush BackgroundBrush
        {
            get { return _backgroundBrush; }
        }

        /// <inheritdoc />
        public override CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
        }

        /// <inheritdoc />
        public override TextEffectCollection TextEffects
        {
            get { return _textEffects; }
        }

        #endregion
    }
}
