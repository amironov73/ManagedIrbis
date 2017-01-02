// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Search
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SearchManager
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection
        {
            get { return _connection; }
        }

        /// <summary>
        /// Search history.
        /// </summary>
        [NotNull]
        public NonNullCollection<SearchResult> SearchHistory { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchManager
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            _connection = connection;
            SearchHistory = new NonNullCollection<SearchResult>();
        }

        #endregion

        #region Private members

        private readonly IrbisConnection _connection;

        #endregion

        #region Public methods

        /// <summary>
        /// Load search scenarios.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public SearchScenario[] LoadSearchScenarios
            (
                [NotNull] FileSpecification file
            )
        {
            Code.NotNull(file, "file");

            string text = Connection.ReadTextFile(file);
            if (string.IsNullOrEmpty(text))
            {
                return new SearchScenario[0];
            }
            using (StringReader reader = new StringReader(text))
            using (IniFile iniFile = new IniFile())
            {
                iniFile.Read(reader);
                SearchScenario[] result
                    = SearchScenario.ParseIniFile(iniFile);

                return result;
            }
        }

        /// <summary>
        /// Search.
        /// </summary>
        [NotNull]
        public FoundLine[] Search
            (
                [NotNull] string database,
                [NotNull] string expression,
                [CanBeNull] string prefix
            )
        {
            Code.NotNullNorEmpty(database, "database");
            Code.NotNullNorEmpty(expression, "expression");

            SearchParameters parameters = new SearchParameters
            {
                Database = database,
                SearchExpression = expression,
                FormatSpecification = IrbisFormat.Brief
            };

            SearchCommand command 
                = Connection.CommandFactory.GetSearchCommand();
            command.ApplyParameters(parameters);

            Connection.ExecuteCommand(command);

            FoundLine[] result = command.Found
                .ThrowIfNull("command.Found")
                .Select
                (
                    item => new FoundLine
                    {
                        Mfn = item.Mfn,
                        Description = item.Text
                    }
                )
                .ToArray();

            if (!string.IsNullOrEmpty(prefix))
            {
                int prefixLength = prefix.Length;

                foreach (FoundLine line in result)
                {
                    string description = line.Description;
                    if (string.IsNullOrEmpty(description))
                    {
                        continue;
                    }
                    if (description.StartsWith(prefix))
                    {
                        line.Description = description
                            .Substring(prefixLength);
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
