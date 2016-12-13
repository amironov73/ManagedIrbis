// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SiberianUtility.cs -- 
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
using System.Windows.Forms;

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
    public static class SiberianUtility
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Measure given cell text.
        /// </summary>
        public static void MeasureText
            (
                this SiberianGrid grid,
                string text,
                SiberianDimensions dimensions
            )
        {
            Size size = dimensions.ToSize();
            Font font = grid.Font;
            TextFormatFlags flags = TextFormatFlags.Left
                | TextFormatFlags.Top
                | TextFormatFlags.NoPrefix;

            Size result = TextRenderer.MeasureText
                (
                    text,
                    font,
                    size,
                    flags
                );

            dimensions.Width = result.Width;
            dimensions.Height = result.Height;
        }

        #endregion

        #region Object members

        #endregion
    }
}
