// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftContextSaver.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Ast;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Save state of <see cref="PftContext"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftContextSaver
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Saved context.
        /// </summary>
        [NotNull]
        public PftContext Context { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftContextSaver
            (
                [NotNull] PftContext context,
                bool clear
            )
        {
            Code.NotNull(context, "context");

            Context = context;
            _index = context.Index;
            _currentGroup = context.CurrentGroup;
            _currentField = context.CurrentField;
            _record = context.Record;

            if (clear)
            {
                context.Index = 0;
                context.CurrentGroup = null;
                context.CurrentField = null;
            }
        }

        #endregion

        #region Private members

        private int _index;
        private PftGroup _currentGroup;
        private PftField _currentField;
        private MarcRecord _record;

        #endregion

        #region IDisposable members

        /// <inheritdoc/>
        public void Dispose()
        {
            Context.Index = _index;
            Context.CurrentGroup = _currentGroup;
            Context.CurrentField = _currentField;
            Context.Record = _record;
        }

        #endregion
    }
}
