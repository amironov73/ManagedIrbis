// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SimpleTextRunProperties.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Globalization;
using System.Windows;
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
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SimpleTextRunProperties()
            : this("Arial", 16)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SimpleTextRunProperties
            (
                [NotNull] string typeface,
                double emSize
            )
            : this(new Typeface(typeface), emSize)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SimpleTextRunProperties
            (
                [NotNull] Typeface typeface,
                double emSize
            )
        {
            Code.NotNull(typeface, "typeface");

            _typeface = typeface;
            _fontRenderingEmSize = emSize;
            _textDecorations = new TextDecorationCollection();
            _foregroundBrush = Brushes.Black;
            _backgroundBrush = Brushes.White;
            _cultureInfo = CultureInfo.CurrentCulture;
            _textEffects = new TextEffectCollection();
        }

        #endregion

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

        #region Public methods

        /// <summary>
        /// Set the <see cref="Typeface"/>.
        /// </summary>
        [NotNull]
        public SimpleTextRunProperties SetTypeface
            (
                [NotNull] Typeface typeface
            )
        {
            Code.NotNull(typeface, "typeface");

            _typeface = typeface;

            return this;
        }

        /// <summary>
        /// Set the <see cref="Typeface"/>.
        /// </summary>
        [NotNull]
        public SimpleTextRunProperties SetTypeface
            (
                [NotNull] string typeface
            )
        {
            Code.NotNullNorEmpty(typeface, "typeface");

            _typeface = new Typeface(typeface);

            return this;
        }

        /// <summary>
        /// Set the <see cref="FontRenderingEmSize"/>.
        /// </summary>
        [NotNull]
        public SimpleTextRunProperties SetFontRenderingEmSize
            (
                double emSize
            )
        {
            Code.Positive(emSize, "emSize");

            _fontRenderingEmSize = emSize;

            return this;
        }

        /// <summary>
        /// Set the <see cref="FontHintingEmSize"/>.
        /// </summary>
        [NotNull]
        public SimpleTextRunProperties SetFontHintingEmSize
            (
                double emSize
            )
        {
            Code.Positive(emSize, "emSize");

            _fontHintingEmSize = emSize;

            return this;
        }

        /// <summary>
        /// Set the <see cref="ForegroundBrush"/>.
        /// </summary>
        [NotNull]
        public SimpleTextRunProperties SetForegroundBrush
            (
                [NotNull] Brush brush
            )
        {
            Code.NotNull(brush, "brush");

            _foregroundBrush = brush;

            return this;
        }

        /// <summary>
        /// Set the <see cref="ForegroundBrush"/>.
        /// </summary>
        [NotNull]
        public SimpleTextRunProperties SetForegroundBrush
            (
                Color color
            )
        {
            _foregroundBrush = new SolidColorBrush(color);

            return this;
        }

        /// <summary>
        /// Set the <see cref="BackgroundBrush"/>.
        /// </summary>
        [NotNull]
        public SimpleTextRunProperties SetBackgroundBrush
            (
                [NotNull] Brush brush
            )
        {
            Code.NotNull(brush, "brush");

            _backgroundBrush = brush;

            return this;
        }

        /// <summary>
        /// Set the <see cref="BackgroundBrush"/>.
        /// </summary>
        [NotNull]
        public SimpleTextRunProperties SetBackgroundBrush
            (
                Color color
            )
        {
            _backgroundBrush = new SolidColorBrush(color);

            return this;
        }

        /// <summary>
        /// Set the <see cref="CultureInfo"/>.
        /// </summary>
        [NotNull]
        public SimpleTextRunProperties SetCultureInfo
            (
                [NotNull] CultureInfo culture
            )
        {
            Code.NotNull(culture, "culture");

            _cultureInfo = culture;

            return this;
        }

        #endregion

        #region TextRunProperties members

        /// <inheritdoc cref="TextRunProperties.Typeface" />
        public override Typeface Typeface
        {
            get { return _typeface; }
        }

        /// <inheritdoc cref="TextRunProperties.FontRenderingEmSize" />
        public override double FontRenderingEmSize
        {
            get { return _fontRenderingEmSize; }
        }

        /// <inheritdoc cref="TextRunProperties.FontHintingEmSize" />
        public override double FontHintingEmSize
        {
            get { return _fontHintingEmSize; }
        }

        /// <inheritdoc cref="TextRunProperties.TextDecorations" />
        public override TextDecorationCollection TextDecorations
        {
            get { return _textDecorations; }
        }

        /// <inheritdoc cref="TextRunProperties.ForegroundBrush" />
        public override Brush ForegroundBrush
        {
            get { return _foregroundBrush; }
        }

        /// <inheritdoc cref="TextRunProperties.BackgroundBrush" />
        public override Brush BackgroundBrush
        {
            get { return _backgroundBrush; }
        }

        /// <inheritdoc cref="TextRunProperties.CultureInfo" />
        public override CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
        }

        /// <inheritdoc cref="TextRunProperties.TextEffects" />
        public override TextEffectCollection TextEffects
        {
            get { return _textEffects; }
        }

        #endregion
    }
}
