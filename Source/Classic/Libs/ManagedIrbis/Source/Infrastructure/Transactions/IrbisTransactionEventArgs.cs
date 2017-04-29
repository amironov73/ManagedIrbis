// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisTransactionEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

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
    public sealed class IrbisTransactionEventArgs
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        public IrbisConnection Connection { get; internal set; }

        /// <summary>
        /// Context.
        /// </summary>
        public IrbisTransactionContext Context { get; internal set; }

        /// <summary>
        /// Item.
        /// </summary>
        public IrbisTransactionItem Item { get; internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisTransactionEventArgs
            (
                [NotNull] IrbisConnection connection,
                [NotNull] IrbisTransactionContext context,
                [NotNull] IrbisTransactionItem item
            )
        {
            Connection = connection;
            Context = context;
            Item = item;
        }

        #endregion
    }
}
