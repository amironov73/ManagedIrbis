/* PftUiDebugger.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI.Pft
{
    /// <summary>
    /// PFT debugger for WinForms.
    /// </summary>
    public sealed class PftUiDebugger
        : PftDebugger
    {
        #region Properties

        /// <summary>
        /// Form.
        /// </summary>
        [NotNull]
        public PftDebuggerForm Form { get { return _form; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftUiDebugger
            (
                [NotNull] PftContext context
            )
            : base(context)
        {
            _form = new PftDebuggerForm();
        }

        #endregion

        #region Private members

        private readonly PftDebuggerForm _form;

        #endregion

        #region PftDebugger members

        /// <inheritdoc/>
        public override void Activate
            (
                PftDebugEventArgs eventArgs
            )
        {
            _form.ShowDialog();
        }

        #endregion
    }
}
