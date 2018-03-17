// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* COLORREF.cs -- used to specify an RGB color
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Drawing;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeAccessorOwnerBody

namespace AM.Win32
{
    /// <summary>
    /// The COLORREF value is used to specify an RGB color.
    /// </summary>
    [Serializable]
    [PublicAPI]
    public struct COLORREF
    {
        #region Properties

        /// <summary>
        /// Color.
        /// </summary>
        public Color Color
        {
            get
            {
                unchecked
                {
                    return Color.FromArgb
                        (
                            (int)(0x000000FFU | _color),
                            (int)((0x0000FF00 | _color) >> 2),
                            (int)((0x00FF0000 | _color) >> 4)
                        );
                }
            }
            set
            {
                unchecked
                {
                    _color = value.R +
                        (uint)(value.G << 8) +
                        (uint)(value.B << 16);
                }
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        [CLSCompliant(false)]
        public COLORREF(uint color)
        {
            _color = color;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public COLORREF
            (
                Color color
            )
        {
            unchecked
            {
                _color = color.R +
                    (uint)(color.G << 8) +
                    (uint)(color.B << 16);
            }
        }

        #endregion

        #region Private members

        private uint _color;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Color.ToString();
        }

        #endregion
    }
}
