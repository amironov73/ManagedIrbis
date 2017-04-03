// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordFilter.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using AM;

using CodeJam;

using JetBrains.Annotations;

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
    public sealed class RecordFilter
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Client.
        /// </summary>
        [NotNull]
        [XmlIgnore]
        [JsonIgnore]
        public AbstractClient Client { get; internal set; }

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
        public RecordFilter()
        {
            Client = new LocalClient();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordFilter
            (
                [NotNull] AbstractClient client
            )
        {
            Code.NotNull(client, "client");

            Client = client;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordFilter
            (
                [NotNull] AbstractClient client,
                [NotNull] string expression
            )
        {
            Code.NotNull(client, "client");
            Code.NotNullNorEmpty(expression, "expression");

            Client = client;
            _expression = expression;
        }

        #endregion

        #region Private members

        private string _expression;

        private PftFormatter _formatter;

        #endregion

        #region Public methods

        /// <summary>
        /// Check the record.
        /// </summary>
        public bool CheckRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            string expression = Expression;
            if (string.IsNullOrEmpty(expression))
            {
                return true;
            }

            if (ReferenceEquals(_formatter, null))
            {
                _formatter = new PftFormatter();
                _formatter.SetEnvironment(Client);
                _formatter.ParseProgram(expression);
            }

            string text = _formatter.Format(record);
            bool result = CheckResult(text);

            return result;
        }

        /// <summary>
        /// Check text result.
        /// </summary>
        public static bool CheckResult
            (
                [CanBeNull] string text
            )
        {
            int value;
            if (!NumericUtility.TryParseInt32(text, out value))
            {
                return false;
            }

            return value != 0;
        }

        /// <summary>
        /// Filter records.
        /// </summary>
        [NotNull]
        public IEnumerable<MarcRecord> FilterRecords
            (
                [NotNull] IEnumerable<MarcRecord> sourceRecords
            )
        {
            Code.NotNull(sourceRecords, "sourceRecords");

            foreach (MarcRecord record in sourceRecords)
            {
                if (CheckRecord(record))
                {
                    yield return record;
                }
            }
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
