// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisTransactionManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

using AM;

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
    public sealed class IrbisTransactionManager
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Context.
        /// </summary>
        [NotNull]
        public IrbisTransactionContext Context { get; private set; }

        /// <summary>
        /// Connection.
        /// </summary>
        public IrbisProvider Provider { get; private set; }

        /// <summary>
        /// In transaction now?
        /// </summary>
        public bool InTransactionNow
        {
            get
            {
                return Context.Items.Count != 0;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisTransactionManager
            (
                [NotNull] IrbisProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            Context = new IrbisTransactionContext();
            Provider = provider;

            //Connection.Transaction += _EventHandler;
        }

        #endregion

        #region Private members

        //private void _EventHandler
        //    (
        //        object sender,
        //        IrbisTransactionEventArgs eventArgs
        //    )
        //{
        //    Context.Items.Add(eventArgs.Item);
        //}

        #endregion

        #region Public methods

        /// <summary>
        /// Begin transaction.
        /// </summary>
        public void BeginTransaction
            (
                string name
            )
        {
            Context = new IrbisTransactionContext
                (
                    name,
                    Context
                );
        }

        /// <summary>
        /// Commit transaction.
        /// </summary>
        public void CommitTransaction()
        {
            IrbisTransactionContext context = Context.ParentContext
                ?? new IrbisTransactionContext();

            Context = context;

            // TODO implement
        }

        /// <summary>
        /// Rollback transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            IrbisTransactionContext context = Context.ParentContext
                                              ?? new IrbisTransactionContext();

            Context = context;

            // TODO implement
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            // TODO implement
        }

        #endregion
    }
}
