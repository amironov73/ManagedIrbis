// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisTransactionManager.cs --
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
        public IrbisTransactionContext Context { get; private set; }

        /// <summary>
        /// Connection.
        /// </summary>
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// In transaction now?
        /// </summary>
        public bool InTransactionNow
        {
            get
            {
                return (Context.Items.Count != 0);
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisTransactionManager
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Context = new IrbisTransactionContext();
            Connection = connection;

            //Connection.Transaction += _EventHandler;
        }

        #endregion

        #region Private members

        private void _EventHandler
            (
                object sender,
                IrbisTransactionEventArgs eventArgs
            )
        {
            Context.Items.Add
                (
                    eventArgs.Item
                );
        }

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
            Context = Context.ParentContext;
            if (ReferenceEquals(Context, null))
            {
                Context = new IrbisTransactionContext();
            }
        }

        /// <summary>
        /// Rollback transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            throw new NotImplementedException("Rollback transaction");
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (!ReferenceEquals(Connection, null))
            {
                // Connection.Transaction -= _EventHandler;
            }
        }

        #endregion
    }
}
