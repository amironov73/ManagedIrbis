/* IrbisTextCache.cs -- 
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

using ManagedIrbis.Menus;
using ManagedIrbis.Network;

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
    public sealed class MenuCache
        : MemoryCache<FileSpecification, MenuFile>
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
        public MenuCache
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            _connection = connection;
            Requester = _MenuRequester;
        }

        #endregion

        #region Private members

        private readonly IrbisConnection _connection;

        private MenuFile _MenuRequester
            (
                FileSpecification fileSpecification
            )
        {
            RequestCount++;

            return MenuFile.ReadFromServer
                (
                    Connection,
                    fileSpecification
                );
        }

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion

    }
}
