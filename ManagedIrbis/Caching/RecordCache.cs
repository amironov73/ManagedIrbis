/* RecordCache.cs -- cache of records
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

using AM;
using AM.Caching;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Caching
{
    /// <summary>
    /// Cache of <see cref="MarcRecord"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RecordCache
        : MemoryCache<int,MarcRecord>
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get { return _connection; } }

        /// <summary>
        /// Request count.
        /// </summary>
        public int RequestCount { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordCache
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            _connection = connection;
            Requester = _RecordRequester;
        }

        #endregion

        #region Private members

        private readonly IrbisConnection _connection;

        private MarcRecord _RecordRequester
            (
                int mfn
            )
        {
            RequestCount++;

            return Connection.ReadRecord(mfn);
        }

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
