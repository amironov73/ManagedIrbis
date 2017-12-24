// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisTransactionContext.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Collections;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Transactions
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisTransactionContext
    {
        #region Properties

        /// <summary>
        /// Transaction items.
        /// </summary>
        [NotNull]
        public NonNullCollection<IrbisTransactionItem> Items { get; private set; }

        /// <summary>
        /// Name of the context (optional).
        /// </summary>
        [CanBeNull]
        public string Name { get; private set; }

        /// <summary>
        /// Parent context.
        /// </summary>
        [CanBeNull]
        public IrbisTransactionContext ParentContext
        {
            get; private set;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisTransactionContext()
        {
            Items = new NonNullCollection<IrbisTransactionItem>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisTransactionContext
            (
                [NotNull] string name
            )
            : this()
        {
            Code.NotNullNorEmpty(name, "name");

            Name = name;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisTransactionContext
            (
                [NotNull] IrbisTransactionContext parentContext
            )
            : this()
        {
            Code.NotNull(parentContext, "parentContext");

            ParentContext = parentContext;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisTransactionContext
            (
                [NotNull] string name,
                [NotNull] IrbisTransactionContext parentContext
            )
            : this()
        {
            Code.NotNullNorEmpty(name, "name");
            Code.NotNull(parentContext, "parentContext");

            Name = name;
            ParentContext = parentContext;
        }

        #endregion
    }
}
