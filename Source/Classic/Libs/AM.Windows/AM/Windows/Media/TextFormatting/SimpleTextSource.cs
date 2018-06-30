// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SimpleTextSource.cs --
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
    public sealed class SimpleTextSource
        : TextSource
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SimpleTextSource
            (
                IEnumerable<TextRun> runs
            )
        {
            _list = new List<TextRun>(runs);
        }

        #endregion

        #region Private members

        private List<TextRun> _list;

        #endregion

        #region TextSource members

        /// <inheritdoc cref="TextSource.GetTextRun" />
        public override TextRun GetTextRun
            (
                int textSourceCharacterIndex
            )
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="TextSource.GetPrecedingText" />
        public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText
            (
                int textSourceCharacterIndexLimit
            )
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="TextSource.GetTextEffectCharacterIndexFromTextSourceCharacterIndex" />
        public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex
            (
                int textSourceCharacterIndex
            )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
