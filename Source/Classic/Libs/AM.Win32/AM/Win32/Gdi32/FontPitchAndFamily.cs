// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FontFamily.cs -- the pitch and family of the font for CreateFont method
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// The pitch and family of the font for CreateFont method.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum FontPitchAndFamily
    {
        /// <summary>
        /// Default pitch.
        /// </summary>
        DEFAULT_PITCH = 0,

        /// <summary>
        /// Fixed width font.
        /// </summary>
        FIXED_PITCH = 1,

        /// <summary>
        /// Variable width font.
        /// </summary>
        VARIABLE_PITCH = 2,

        /// <summary>
        /// Mono width font.
        /// </summary>
        MONO_FONT = 8,

        /// <summary>
        /// Use default font.
        /// </summary>
        FF_DONTCARE = 0 << 4,

        /// <summary>
        /// Fonts with variable stroke width and with serifs.
        /// MS Serif is an example.
        /// </summary>
        FF_ROMAN = 1 << 4,

        /// <summary>
        /// Fonts with variable stroke width and without serifs.
        /// MS Sans Serif is an example.
        /// </summary>
        FF_SWISS = 2 << 4,

        /// <summary>
        /// Fonts with constant stroke width, with or without serifs.
        /// Pica, Elite, and Courier New are examples.
        /// </summary>
        FF_MODERN = 3 << 4,

        /// <summary>
        /// Fonts designed to look like handwriting.
        /// Script and Cursive are examples.
        /// </summary>
        FF_SCRIPT = 4 << 4,

        /// <summary>
        /// Novelty fonts. Old English is an example.
        /// </summary>
        FF_DECORATIVE = 5 << 4
    }
}
