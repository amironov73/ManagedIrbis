/* MorphologyEngine.cs -- работа с морфологией
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Morphology
{
#if NOTDEF

    /// <summary>
    /// Работа с морфологией
    /// </summary>
    public sealed class MorphologyEngine
    {
        #region Properties

        /// <summary>
        /// ИРБИС-клиент.
        /// </summary>
        [NotNull]
        public ManagedClient64 Client
        {
            get { return _client; }
        }

        /// <summary>
        /// Провайдер морфологии.
        /// </summary>
        [NotNull]
        public MorphologyProvider Provider
        {
            get { return _provider; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MorphologyEngine
            (
                [NotNull] ManagedClient64 client
            )
        {
            Code.NotNull(() => client);

            _client = client;
            _provider = new IrbisMorphologyProvider
                (
                    client
                );
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MorphologyEngine
            (
                [NotNull] ManagedClient64 client,
                [NotNull] MorphologyProvider provider
            )
        {
            Code.NotNull(() => client);
            Code.NotNull(() => provider);

            _client = client;
            _provider = provider;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MorphologyEngine
            (
                [NotNull] ManagedClient64 client,
                [NotNull] string prefix,
                [NotNull] string database
            )
        {
            Code.NotNull(() => client);

            _client = client;
            _provider = new IrbisMorphologyProvider
                (
                    prefix,
                    database,
                    client
                );
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MorphologyEngine
            (
                MorphologyProvider provider
            )
        {
            _provider = provider;
        }

        #endregion

        #region Private members

        private readonly MorphologyProvider _provider;

        private readonly ManagedClient64 _client;

        #endregion

        #region Public methods

        public string RewriteQuery
            (
                string queryText
            )
        {
            return Provider.RewriteQuery(queryText);
        }

        public int[] Search
            (
                string format,
                params object[] args
            )
        {
            string original = string.Format(format, args);
            string rewritten = RewriteQuery(original);
            return _client.Search(rewritten);
        }

        public IrbisRecord[] SearchRead
            (
                string format,
                params object[] args
            )
        {
            string original = string.Format(format, args);
            string rewritten = RewriteQuery(original);
            return _client.SearchRead(rewritten);
        }

        public IrbisRecord SearchReadOneRecord
            (
                string format,
                params object[] args
            )
        {
            string original = string.Format(format, args);
            string rewritten = RewriteQuery(original);
            return _client.SearchReadOneRecord(rewritten);
        }

        public string[] SearchFormat
            (
                string expression,
                string format
            )
        {
            string rewritten = RewriteQuery(expression);
            return _client.SearchFormat(rewritten, format);
        }

        #endregion
    }

#endif
}
