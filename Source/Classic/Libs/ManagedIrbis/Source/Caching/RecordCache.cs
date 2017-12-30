// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordCache.cs -- cache of records
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WINMOBILE && !PocketPC

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
        public IIrbisConnection Connection { get { return _connection; } }

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
                [NotNull] IIrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            _connection = connection;
            Requester = _RecordRequester;
        }

        #endregion

        #region Private members

        private readonly IIrbisConnection _connection;

        private MarcRecord _RecordRequester
            (
                int mfn
            )
        {
            RequestCount++;

            return Connection.ReadRecord(mfn);
        }

        #endregion
    }
}

#endif

