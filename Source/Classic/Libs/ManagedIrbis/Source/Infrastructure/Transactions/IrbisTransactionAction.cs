// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisTransactionAction.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Infrastructure.Transactions
{
    /// <summary>
    /// Transaction action.
    /// </summary>
    public enum IrbisTransactionAction
    {
        /// <summary>
        /// Create record.
        /// </summary>
        CreateRecord = (byte)'N',

        /// <summary>
        /// Modify (update) record.
        /// </summary>
        ModifyRecord = (byte)'W',

        /// <summary>
        /// Delete record.
        /// </summary>
        DeleteRecord = (byte)'D'
    }
}
