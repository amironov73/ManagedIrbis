// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SiberianCell.cs -- 
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

    public class SiberianCell
    {
        #region Events

        /// <summary>
        /// Fired on click.
        /// </summary>
        public event EventHandler<SiberianClickEventArgs> Click;

        /// <summary>
        /// Measure content.
        /// </summary>
        public event EventHandler<SiberianMeasureEventArgs> Measure;

        /// <summary>
        /// Fired when tooltip needed.
        /// </summary>
        public event EventHandler<SiberianToolTipEventArgs> ToolTip;

        #endregion

        #region Properties

        /// <summary>
        /// Column.
        /// </summary>
        [NotNull]
        public SiberianColumn Column { get; internal set; }

        /// <summary>
        /// Grid.
        /// </summary>
        [NotNull]
        public SiberianGrid Grid { get { return Row.Grid; } }

        /// <summary>
        /// Row.
        /// </summary>
        [NotNull]
        public SiberianRow Row { get; internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected SiberianCell()
        {
        }

        #endregion

        #region Private members

        /// <summary>
        /// Handle <see cref="Click"/> event.
        /// </summary>
        protected internal virtual void HandleClick
            (
                [NotNull] SiberianClickEventArgs eventArgs
            )
        {
            Click.Raise(this, eventArgs);
        }

        /// <summary>
        /// Get tooltip for the cell.
        /// </summary>
        protected internal virtual void HandleToolTip
            (
                [NotNull] SiberianToolTipEventArgs eventArgs
            )
        {
            ToolTip.Raise();
        }

        /// <summary>
        /// Measure content of the cell.
        /// </summary>
        protected internal virtual void MeasureContent
            (
                [NotNull] SiberianDimensions dimensions
            )
        {
            Code.NotNull(dimensions, "dimensions");

            EventHandler<SiberianMeasureEventArgs> handler = Measure;
            if (!ReferenceEquals(handler, null))
            {
                SiberianMeasureEventArgs eventArgs
                    = new SiberianMeasureEventArgs(dimensions);

                handler(this, eventArgs);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Close editor.
        /// </summary>
        public virtual void CloseEditor
            (
                bool accept
            )
        {
            if (!ReferenceEquals(Grid.Editor, null))
            {
                Grid.Editor.Dispose();
                Grid.Editor = null;

                Grid.Invalidate();
            }
        }

        /// <summary>
        /// Handles click on the cell.
        /// </summary>
        public virtual void OnClick
            (
                [NotNull] SiberianClickEventArgs eventArgs
            )
        {
            // Nothing to do here?
        }

        /// <summary>
        /// Draw the cell.
        /// </summary>
        public virtual void Paint
            (
                PaintEventArgs args
            )
        {
            
        }

        #endregion

        #region Object members

        #endregion
    }
}
