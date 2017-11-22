// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SynonymEngine.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable ConvertClosureToMethodGroup

namespace ManagedIrbis.Morphology
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SynonymEngine
    {
        #region Constants

        /// <summary>
        /// Default database name.
        /// </summary>
        public const string DefaultDatabase = "SYNON";

        /// <summary>
        /// Default prefix.
        /// </summary>
        public const string DefaultPrefix = "WORD=";

        #endregion

        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisProvider Provider { get; private set; }

        /// <summary>
        /// Database name.
        /// </summary>
        public NonNullValue<string> Database { get; set; }

        /// <summary>
        /// Prefix.
        /// </summary>
        public NonNullValue<string> Prefix { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SynonymEngine
            (
                [NotNull] IrbisProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            Provider = provider;
            Database = DefaultDatabase;
            Prefix = DefaultPrefix;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get synonyms for the word.
        /// </summary>
        [NotNull]
        public string[] GetSynonyms
            (
                [NotNull] string word
            )
        {
            Code.NotNullNorEmpty(word, "word");

            string previousDatabase = Provider.Database;
            string[] result;
            try
            {
                Provider.Database = Database;
                string expression = string.Format
                    (
                        "\"{0}{1}\"",
                        Prefix,
                        word
                    );

                int[] found = Provider.Search(expression);
                if (found.Length == 0)
                {
                    return StringUtility.EmptyArray;
                }

                List<MarcRecord> records
                    = new List<MarcRecord>(found.Length);
                foreach (int mfn in found)
                {
                    MarcRecord record = Provider.ReadRecord(mfn);
                    if (!ReferenceEquals(record, null))
                    {
                        records.Add(record);
                    }
                }

                SynonymEntry[] entries = records
                    .Select(record => SynonymEntry.Parse(record))
                    .ToArray();

                result = entries
                    .SelectMany(entry => entry.Synonyms)
                    .Distinct()
                    .ToArray();
            }
            finally
            {
                Provider.Database = previousDatabase;
            }

            return result;
        }

        #endregion
    }
}
