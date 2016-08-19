/* COLORREF.cs -- used to specify an RGB color 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// The COLORREF value is used to specify an RGB color.
    /// </summary>
    [Serializable]
    //[StructLayout ( LayoutKind.Sequential )]
    public struct COLORREF
    {
        #region Properties

        /// <summary>
        /// Color.
        /// </summary>
        /// <value></value>
        public Color Color
        {
            get
            {
                return Color.FromArgb ( (int) ( 0x000000FFU | _color ),
                (int) ( ( 0x0000FF00 | _color ) >> 2 ),
                (int) ( ( 0x00FF0000 | _color ) >> 4 ) );
            }
            set
            {
                _color = ( (uint) value.R ) +
                (uint) ( value.G << 8 ) +
                (uint) ( value.B << 16 );
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="color">Raw color value.</param>
        [CLSCompliant ( false )]
        public COLORREF ( uint color )
        {
            _color = color;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="color">Color.</param>
        public COLORREF ( Color color )
        {
            _color = ( (uint) color.R ) +
                (uint) ( color.G << 8 ) +
                (uint) ( color.B << 16 );
        }

        #endregion

        #region Private members

        private uint _color;

        #endregion

        #region Object members

        /// <summary>
        /// Gets string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString ()
        {
            return _color.ToString ();
        }

        #endregion
    }
}
