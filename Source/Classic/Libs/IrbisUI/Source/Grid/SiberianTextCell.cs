/* SiberianTextCell.cs -- 
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
using System.Windows.Forms.VisualStyles;
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
    public class SiberianTextCell
        : SiberianCell
    {
        #region Properties

        /// <summary>
        /// Text.
        /// </summary>
        [CanBeNull]
        public string Text { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region SiberianCell members

        /// <inheritdoc/>
        public override void Paint
            (
                PaintEventArgs args
            )
        {
            Graphics graphics = args.Graphics;
            Rectangle rectangle = args.ClipRectangle;

            string text = string.Format
                (
                    "{0},{1}",
                    Column.Index,
                    Row.Index
                );

            Color foreColor = Color.Black;
            if (Row == Grid.CurrentRow)
            {
                foreColor = Color.White;
            }

            TextFormatFlags flags 
                = TextFormatFlags.TextBoxControl
                | TextFormatFlags.EndEllipsis
                | TextFormatFlags.NoPrefix
                | TextFormatFlags.VerticalCenter;

            TextRenderer.DrawText
                (
                    graphics,
                    text,
                    Grid.Font,
                    rectangle,
                    foreColor,
                    flags
                );
        }

        #endregion

        #region Object members

        #endregion
    }
}
