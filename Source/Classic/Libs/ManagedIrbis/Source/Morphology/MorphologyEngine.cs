// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MorphologyEngine.cs -- morphology engine
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
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
        public IrbisProvider Connection { get; private set; }

        /// <summary>
        /// Morphology provider.
        /// </summary>
        [NotNull]
        public MorphologyProvider Morphology { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MorphologyEngine
            (
                [NotNull] IrbisProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            Connection = provider;
            Morphology = new IrbisMorphologyProvider(provider);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MorphologyEngine
            (
                [NotNull] IrbisProvider provider,
                [NotNull] MorphologyProvider morphology
            )
        {
            Code.NotNull(provider, "provider");
            Code.NotNull(morphology, "morphology");

            Connection = provider;
            Morphology = morphology;
        }

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

            MorphologyProvider provider = Morphology.ThrowIfNull("Provider");

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
            int[] result = Connection.Search(rewritten);

            return result;
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
            int[] found = Connection.Search(rewritten);
            if (found.Length == 0)
            {
                return EmptyArray<MarcRecord>.Value;
            }

            List<MarcRecord> result = new List<MarcRecord>(found.Length);
            foreach (int mfn in found)
            {
                MarcRecord record = Connection.ReadRecord(mfn);
                if (!ReferenceEquals(record, null))
                {
                    result.Add(record);
                }
            }
            if (result.Count == 0)
            {
                return EmptyArray<MarcRecord>.Value;
            }

            return result.ToArray();
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
            int[] found = Connection.Search(rewritten);
            if (found.Length == 0)
            {
                return null;
            }

            MarcRecord result = Connection.ReadRecord(found[0]);

            return result;
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
            int[] found = Connection.Search(rewritten);
            if (found.Length == 0)
            {
                return EmptyArray<FoundItem>.Value;
            }

            List<FoundItem> result = new List<FoundItem>(found.Length);
            foreach (int mfn in found)
            {
                MarcRecord record = Connection.ReadRecord(mfn);
                if (!ReferenceEquals(record, null))
                {
                    string text = Connection.FormatRecord(record, format);
                    if (!string.IsNullOrEmpty(text))
                    {
                        FoundItem item = new FoundItem
                        {
                            Mfn = mfn,
                            Record = record,
                            Text = text
                        };
                        result.Add(item);
                    }
                }
            }

            return result.ToArray();
        }

        #endregion
    }
}
