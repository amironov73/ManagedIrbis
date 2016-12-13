// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SiberianDimensions.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI.Grid
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SiberianDimensions
    {
        #region Properties

        /// <summary>
        /// Height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Width.
        /// </summary>
        public int Width { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianDimensions()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianDimensions
            (
                int width,
                int height
            )
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianDimensions
            (
                Size size
            )
        {
            Width = size.Width;
            Height = size.Height;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Convert to <see cref="Size"/>.
        /// </summary>
        public Size ToSize()
        {
            Size result = new Size(Width, Height);

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format
                (
                    "{{Width={0}, Height={1}}}",
                    Width,
                    Height
                );
        }

        #endregion
    }
}
