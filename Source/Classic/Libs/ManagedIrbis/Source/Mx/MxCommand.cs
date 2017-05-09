// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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
        : IDisposable
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
        /// Initialize commands before using.
        /// </summary>
        public virtual void Initialize
            (
                [NotNull] MxExecutive executive
            )
        {
            Code.NotNull(executive, "executive");

            // Nothing to do here
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        public virtual bool Execute
            (
                [NotNull] MxExecutive executive,
                [NotNull] MxArgument[] arguments
            )
        {
            Code.NotNull(executive, "executive");
            Code.NotNull(arguments, "arguments");

            OnBeforeExecute();

            executive.WriteLine("Connect");

            OnAfterExecute();

            return true;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public virtual void Dispose()
        {
            // Nothing to do here
        }

        #endregion

        #region Object members

        #endregion
    }
}
