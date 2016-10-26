/* MxCommand.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ManagedIrbis.Source.Mx;
using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Mx
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class MxCommand
    {
        #region Events

        /// <summary>
        /// Fired before <see cref="Execute"/>.
        /// </summary>
        public event EventHandler BeforeExecute;

        /// <summary>
        /// Fired after <see cref="Execute"/>.
        /// </summary>
        public event EventHandler AfterExecute;

        #endregion

        #region Properties

        /// <summary>
        /// Main name of the command.
        /// </summary>
        [NotNull]
        public string Name { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected MxCommand
            (
            )
            : this ("Unnamed")
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected MxCommand
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            Name = name;
        }

        #endregion

        #region Private members

        /// <summary>
        /// Raises <see cref="BeforeExecute"/> event.
        /// </summary>
        protected virtual void OnBeforeExecute()
        {
            BeforeExecute.Raise(this);
        }

        /// <summary>
        /// Raises <see cref="AfterExecute"/> event.
        /// </summary>
        protected virtual void OnAfterExecute()
        {
            AfterExecute.Raise(this);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Execute the command.
        /// </summary>
        public virtual void Execute
            (
                [NotNull] MxExecutive executive,
                [NotNull] MxArgument[] arguments
            )
        {
            Code.NotNull(executive, "executive");
            Code.NotNull(arguments, "arguments");

            OnBeforeExecute();

            OnAfterExecute();
        }

        #endregion

        #region Object members

        #endregion
    }
}
