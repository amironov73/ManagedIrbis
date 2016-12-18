// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SynonymEngine.cs --
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

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure.Commands;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Morphology
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SynonymEngine
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

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
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
            Database = "SYNON";
            Prefix = "K=";
        }

        #endregion

        #region Private members

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

            string expression = string.Format
                (
                    "\"{0}{1}\"",
                    Prefix,
                    word
                );

            SearchReadCommand command
                = Connection.CommandFactory.GetSearchReadCommand();
            command.Database = Database;
            command.SearchExpression = expression;

            Connection.ExecuteCommand(command);

            MarcRecord[] records = command.Records
                .ThrowIfNull("command.Records");

            if (records.Length == 0)
            {
                return new string[0];
            }

            SynonymEntry[] entries = records.Select
                (
                    // ReSharper disable ConvertClosureToMethodGroup

                    record => SynonymEntry.Parse(record)
                    
                    // ReSharper restore ConvertClosureToMethodGroup
                )
                .ToArray();

            string[] result = entries.SelectMany
                (
                    entry => GetSynonyms(entry, word)
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get synonyms for the word.
        /// </summary>
        [NotNull]
        public string[] GetSynonyms
            (
                [NotNull] SynonymEntry entry,
                [NotNull] string word
            )
        {
            Code.NotNull(entry, "entry");
            Code.NotNull(word, "word");

            List<string> result = new List<string>
            {
                entry.MainWord
            };
            if (!ReferenceEquals(entry.Synonyms, null))
            {
                result.AddRange(entry.Synonyms);
            }

            int index = result.FindIndex
                (
                    // ReSharper disable ConvertClosureToMethodGroup
                    
                    s => word.SameString(s)
                    
                    // ReSharper restore ConvertClosureToMethodGroup
                );
            if (index >= 0)
            {
                result.RemoveAt(index);
            }

            return result.ToArray();
        }

        #endregion
    }
}
