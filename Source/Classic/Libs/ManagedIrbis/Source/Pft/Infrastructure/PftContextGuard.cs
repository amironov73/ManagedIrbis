// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftContextGuard.cs -- 
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

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{ChildContext}")]
    public sealed class PftContextGuard
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Child context.
        /// </summary>
        [NotNull]
        public PftContext ChildContext { get; private set; }

        /// <summary>
        /// Parent context.
        /// </summary>
        [NotNull]
        public PftContext ParentContext { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftContextGuard
            (
                [NotNull] PftContext parentContext
            )
        {
            Code.NotNull(parentContext, "parentContext");

            ParentContext = parentContext;
            ChildContext = parentContext.Push();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Push again.
        /// </summary>
        [NotNull]
        public PftContext PushAgain()
        {
            ChildContext.Pop();
            ChildContext = ParentContext.Push();

            return ChildContext;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            ChildContext.Pop();
        }

        #endregion

        #region Object members

        /// <inheritdoc />
        public override string ToString()
        {
            return ChildContext.ToString();
        }

        #endregion
    }
}
