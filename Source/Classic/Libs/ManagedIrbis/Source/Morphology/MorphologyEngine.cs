// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MorphologyEngine.cs -- morphology engine
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Morphology
{
    /// <summary>
    /// Morphology engine.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class MorphologyEngine
    {
        #region Properties

        /// <summary>
        /// Client connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection
        {
            get { return _connection; }
        }

        /// <summary>
        /// Morphology provider.
        /// </summary>
        [NotNull]
        public MorphologyProvider Provider
        {
            get { return _provider; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MorphologyEngine
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            _connection = connection;
            _provider = new IrbisMorphologyProvider
                (
                    connection
                );
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MorphologyEngine
            (
                [NotNull] IrbisConnection connection,
                [NotNull] MorphologyProvider provider
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(provider, "provider");

            _connection = connection;
            _provider = provider;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MorphologyEngine
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string prefix,
                [NotNull] string database
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(prefix, "prefix");
            Code.NotNullNorEmpty(database, "database");

            _connection = connection;
            _provider = new IrbisMorphologyProvider
                (
                    prefix,
                    database,
                    connection
                );
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MorphologyEngine
            (
                [NotNull] MorphologyProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            _provider = provider;
        }

        #endregion

        #region Private members

        private readonly MorphologyProvider _provider;

        private readonly IrbisConnection _connection;

        #endregion

        #region Public methods

        /// <summary>
        /// Rewrite the query.
        /// </summary>
        [NotNull]
        public string RewriteQuery
            (
                [NotNull] string queryText
            )
        {
            Code.NotNullNorEmpty(queryText, "queryText");

            MorphologyProvider provider = Provider.ThrowIfNull("Provider");

            return provider.RewriteQuery(queryText);
        }

        /// <summary>
        /// Search with query rewritting.
        /// </summary>
        [NotNull]
        public int[] Search
            (
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNullNorEmpty(format, "format");

            string original = string.Format(format, args);
            string rewritten = RewriteQuery(original);

            IrbisConnection connection = Connection.ThrowIfNull("Connection");

            return connection.Search(rewritten);
        }

        /// <summary>
        /// Search and read records with query rewritting.
        /// </summary>
        [NotNull]
        public MarcRecord[] SearchRead
            (
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNullNorEmpty(format, "format");

            string original = string.Format(format, args);
            string rewritten = RewriteQuery(original);

            IrbisConnection connection = Connection.ThrowIfNull("Connection");

            return connection.SearchRead(rewritten);
        }

        /// <summary>
        /// Search and read first found record using query rewritting.
        /// </summary>
        [CanBeNull]
        public MarcRecord SearchReadOneRecord
            (
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNullNorEmpty(format, "format");

            string original = string.Format(format, args);
            string rewritten = RewriteQuery(original);

            IrbisConnection connection = Connection.ThrowIfNull("Connection");

            return connection.SearchReadOneRecord(rewritten);
        }

        /// <summary>
        /// Search and format found records using query rewritting.
        /// </summary>
        [NotNull]
        public FoundItem[] SearchFormat
            (
                [NotNull] string expression,
                [NotNull] string format
            )
        {
            Code.NotNullNorEmpty(expression, "expression");
            Code.NotNullNorEmpty(format, "format");

            string rewritten = RewriteQuery(expression);

            IrbisConnection connection = Connection.ThrowIfNull("Connection");

            return connection.SearchFormat(rewritten, format);
        }

        #endregion
    }
}
