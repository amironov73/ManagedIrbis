// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordSource.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Gbl.Infrastructure
{
    /// <summary>
    /// Record source.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class RecordSource
    {
        #region Public methods

        /// <summary>
        /// Get next record (if any).
        /// </summary>
        [CanBeNull]
        public abstract MarcRecord GetNextRecord();

        /// <summary>
        /// Get record count.
        /// </summary>
        public abstract int GetRecordCount();

        /// <summary>
        /// Reset the source.
        /// </summary>
        public abstract void Reset();

        #endregion
    }
}