/* IrbisTextCache.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WINMOBILE && !PocketPC && !SILVERLIGHT

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

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Caching
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class TextCache
        : MemoryCache<FileSpecification,string>
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
        public TextCache
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            _connection = connection;
            Requester = _TextRequester;
        }

        #endregion

        #region Private members

        private readonly IrbisConnection _connection;

        private string _TextRequester
            (
                FileSpecification fileSpecification
            )
        {
            RequestCount++;

            return Connection.ReadTextFile(fileSpecification);
        }

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion

    }
}

#endif

