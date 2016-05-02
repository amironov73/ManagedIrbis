/* IrbisRecord.cs -- MARC-record
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// MARC-record
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [DebuggerDisplay("[{Database}] MFN={Mfn} ({Version})")]
    public sealed class IrbisRecord
    {
        #region Properties

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        public string Database { get; set; }

        /// <summary>
        /// MFN записи
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Статус записи: удалена, блокирована и т.д.
        /// </summary>
        public RecordStatus Status { get; set; }

        /// <summary>
        /// Версия записи. Нумеруется с нуля.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Смещение предыдущей версии записи.
        /// </summary>
        public long PreviousOffset { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
