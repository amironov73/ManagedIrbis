// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisTransactionItem.cs --
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
    /// Данные об элементе транзакции.
    /// </summary>
    public sealed class IrbisTransactionItem
    {
        #region Properties

        /// <summary>
        /// Момент времени.
        /// </summary>
        public DateTime Moment { get; set; }

        /// <summary>
        /// Произведенное действие: создание записи,
        /// модификация, удаление.
        /// </summary>
        public IrbisTransactionAction Action { get; set; }

        /// <summary>
        /// Имя базы данных, в которой происходило действие.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// MFN записи, с которой происходило действие.
        /// </summary>
        public int Mfn { get; set; }

        #endregion
    }
}
