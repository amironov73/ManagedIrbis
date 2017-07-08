// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordSorter.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using AM.Collections;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Batch;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class RecordSorter
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Provider.
        /// </summary>
        [NotNull]
        [XmlIgnore]
        [JsonIgnore]
        public IrbisProvider Provider { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        [XmlElement("expression")]
        [JsonProperty("expression")]
        public string Expression
        {
            get { return _expression; }
            set
            {
                if (!ReferenceEquals(_formatter, null))
                {
                    _formatter.Dispose();
                }
                _formatter = null;
                _expression = value;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordSorter()
        {
            Provider = new LocalProvider();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordSorter
            (
                [NotNull] IrbisProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            Provider = provider;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordSorter
            (
                [NotNull] IrbisProvider provider,
                [NotNull] string expression
            )
        {
            Code.NotNull(provider, "provider");
            Code.NotNullNorEmpty(expression, "expression");

            Provider = provider;
            _expression = expression;
        }

        #endregion

        #region Private members

        private string _expression;

        private PftFormatter _formatter;

        #endregion

        #region Public methods

        /// <summary>
        /// Sort records.
        /// </summary>
        [NotNull]
        public List<MarcRecord> SortRecords
            (
                [NotNull] IEnumerable<MarcRecord> sourceRecords
            )
        {
            Code.NotNull(sourceRecords, "sourceRecords");

            string expression = Expression;
            if (string.IsNullOrEmpty(expression))
            {
                return sourceRecords.ToList();
            }

            ConnectedClient connected
                = Provider as ConnectedClient;
            List<Pair<string, MarcRecord>> list
                = new List<Pair<string, MarcRecord>>();

            if (!ReferenceEquals(connected, null))
            {
                IrbisConnection connection = connected.Connection;
                foreach (MarcRecord record in sourceRecords)
                {
                    string formatted = connection.FormatRecord
                        (
                            expression,
                            record
                        );
                    Pair<string, MarcRecord> pair
                        = new Pair<string, MarcRecord>
                        (
                            formatted,
                            record
                        );
                    list.Add(pair);
                }
            }
            else
            {
                if (ReferenceEquals(_formatter, null))
                {
                    _formatter = new PftFormatter();
                    _formatter.SetProvider(Provider);
                    _formatter.ParseProgram(expression);
                }

                foreach (MarcRecord record in sourceRecords)
                {
                    string formatted = _formatter.Format
                    (
                        record
                    );
                    Pair<string, MarcRecord> pair
                        = new Pair<string, MarcRecord>
                        (
                            formatted,
                            record
                        );
                    list.Add(pair);
                }
            }

            list.Sort
                (
                    (left, right) => NumberText.Compare
                    (
                        left.First,
                        right.First
                    )
                );

            List<MarcRecord> result = list
                .Select(pair => pair.Second)
                .ToList();

            return result;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (!ReferenceEquals(_formatter, null))
            {
                _formatter.Dispose();
                _formatter = null;
            }
        }

        #endregion
    }
}
