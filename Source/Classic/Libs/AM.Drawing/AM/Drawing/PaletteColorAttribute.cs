// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PaletteColorAttribute.cs
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace AM.Drawing
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public class PaletteColorAttribute
        : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="PaletteColorAttribute"/> class.
        /// </summary>
        public PaletteColorAttribute
            (
                string color
            )
        {
            Color = color;
        }

        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <value>The color.</value>
        public string Color { get; private set; }
    }
}
