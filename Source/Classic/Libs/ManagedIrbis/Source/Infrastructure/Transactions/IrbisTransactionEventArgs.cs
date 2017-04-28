// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisTransactionEventArgs.cs --
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
    public sealed class IrbisTransactionEventArgs
    {
        #region Properties

        public IrbisConnection Connection { get; internal set; }

        public IrbisTransactionContext Context { get; internal set; }

        public IrbisTransactionItem Item { get; internal set; }

        #endregion

        #region Construction

        public IrbisTransactionEventArgs(IrbisConnection connection, IrbisTransactionContext context, IrbisTransactionItem item)
        {
            Connection = connection;
            Context = context;
            Item = item;
        }

        #endregion
    }
}
