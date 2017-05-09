// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftIteration.cs -- 
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

using ManagedIrbis.Pft.Infrastructure.Ast;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Итерация для <see cref="PftParallelFor"/>,
    /// <see cref="PftParallelForEach"/>, <see cref="PftParallelGroup"/>
    /// и <see cref="PftParallelWith"/>.
    /// </summary>
    internal sealed class PftIteration //-V3073
        : IDisposable
    {
        #region Properties

        public PftGroup Group { get; private set; }

        public object Data { get; private set; }

        public NonNullCollection<PftNode> Nodes { get; set; }

        public PftContext Context { get; set; }

        public int Index { get; private set; }

        public Task Task { get; private set; }

        public Exception Exception { get; private set; }

        public string Result { get { return Context.Text; } }

        private Action<PftIteration, object> _Action { get; set; }

        #endregion

        #region Construction

        public PftIteration
            (
                [NotNull] PftContext context,
                [NotNull] NonNullCollection<PftNode> nodes,
                int index,
                [NotNull] Action<PftIteration, object> action,
                [NotNull] object data,
                bool withGroup
            )
        {
            Context = context.Push();
            Nodes = nodes.CloneNodes();
            Index = index;
            if (withGroup)
            {
                Group = new PftGroup();
                Context.CurrentGroup = Group;
            }
            Context.Index = Index;
            Exception = null;
            _Action = action;
            Data = data;

            Task = new Task(_Run);
            Task.Start();
        }

        #endregion

        #region Private members

        private void _Run()
        {
            try
            {
                _Action(this, Data);
            }
            catch (Exception exception)
            {
                Exception = exception;
            }
        }

        #endregion

        #region Public methods

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Context.Pop();
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return Index.ToString();
        }

        #endregion
    }
}
