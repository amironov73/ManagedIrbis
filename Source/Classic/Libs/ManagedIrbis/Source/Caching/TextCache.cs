// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisTextCache.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WINMOBILE && !PocketPC

#region Using directives

using AM.Caching;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

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
        public TextCache
            (
                [NotNull] IIrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            _connection = connection;
            Requester = _TextRequester;
        }

        #endregion

        #region Private members

        private readonly IIrbisConnection _connection;

        private string _TextRequester
            (
                FileSpecification fileSpecification
            )
        {
            RequestCount++;

            return Connection.ReadTextFile(fileSpecification);
        }

        #endregion
    }
}

#endif

