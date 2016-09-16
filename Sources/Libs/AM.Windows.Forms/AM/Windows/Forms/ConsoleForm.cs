/* ConsoleForm.cs -- form with ConsoleControl
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Form with <see cref="ConsoleControl"/> inside.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ConsoleForm
        : ModalForm
    {
        #region Properties

        /// <summary>
        /// Console control.
        /// </summary>
        [NotNull]
        public ConsoleControl Console
        {
            get { return _console; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConsoleForm()
        {
            _console = new ConsoleControl();
            int consoleWidth = _console.Width;
            int consoleHeight = _console.Height;

            Controls.Add(_console);
            ClientSize = new Size(consoleWidth, consoleHeight);
        }

        #endregion

        #region Private members

        private readonly ConsoleControl _console;

        #endregion
    }
}
