// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisTransactionEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Transactions
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisTransactionEventArgs
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        public IrbisProvider Provider { get; private set; }

        /// <summary>
        /// Context.
        /// </summary>
        public IrbisTransactionContext Context { get; private set; }

        /// <summary>
        /// Item.
        /// </summary>
        public IrbisTransactionItem Item { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisTransactionEventArgs
            (
                [NotNull] IrbisProvider provider,
                [NotNull] IrbisTransactionContext context,
                [NotNull] IrbisTransactionItem item
            )
        {
            Code.NotNull(provider, "provider");
            Code.NotNull(context, "context");
            Code.NotNull(item, "item");

            Provider = provider;
            Context = context;
            Item = item;
        }

        #endregion
    }
}
