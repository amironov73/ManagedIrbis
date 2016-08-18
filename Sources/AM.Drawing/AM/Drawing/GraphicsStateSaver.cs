/* GraphicsStateSaver.cs -- simple holder of graphics context state.
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Drawing
{
    /// <summary>
    /// Holds state of <see cref="T:System.Drawing.Graphics"/>
    /// class.
    /// </summary>
    [PublicAPI]
    public sealed class GraphicsStateSaver
        : IDisposable
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:GraphicsStateSaver"/> class.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        public GraphicsStateSaver(Graphics graphics)
        {
            Code.NotNull(graphics, "graphics");

            _graphics = graphics;
            _state = _graphics.Save();
        }

        /// <summary>
        /// Releases unmanaged resources and performs 
        /// other cleanup operations before the
        /// <see cref="T:AM.Drawing.GraphicsStateSaver"/> 
        /// is reclaimed by garbage collection.
        /// </summary>
        ~GraphicsStateSaver()
        {
            Dispose(false);
        }

        #endregion

        #region Private members

        /// <summary>
        /// Object of <see cref="T:System.Drawing.Graphics"/> type
        /// which state have been saved.
        /// </summary>
        private Graphics _graphics;

        /// <summary>
        /// Saved state itself.
        /// </summary>
        private GraphicsState _state;

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">if set to <c>true</c> 
        /// [disposing].</param>
        private void Dispose(bool disposing)
        {
            lock (this)
            {
                if (_state != null)
                {
                    _graphics.Restore(_state);
                    _state = null;
                }
            }
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
