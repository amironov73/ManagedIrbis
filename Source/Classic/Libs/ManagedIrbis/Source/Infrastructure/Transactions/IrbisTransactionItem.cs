// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisTransactionItem.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Transactions
{
    /// <summary>
    /// Данные об элементе транзакции.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
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
        [CanBeNull]
        public string Database { get; set; }

        /// <summary>
        /// MFN записи, с которой происходило действие.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Шифр документа.
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        /// Версия документа.
        /// </summary>
        public int Version { get; set; }

        #endregion
    }
}
