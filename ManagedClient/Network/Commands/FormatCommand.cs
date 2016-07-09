/* FormatCommand.cs -- format records on IRBIS-server
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Network.Commands
{
    /// <summary>
    /// Format records on IRBIS-server.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FormatCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

        /// <summary>
        /// Format specification.
        /// </summary>
        [CanBeNull]
        public string FormatSpecification { get; set; }

        /// <summary>
        /// List of MFNs to format.
        /// </summary>
        [NotNull]
        public List<int> MfnList { get; private set; }

        /// <summary>
        /// Virtual record to format.
        /// </summary>
        [CanBeNull]
        public IrbisRecord VirtualRecord { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FormatCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
            MfnList = new List<int>();
        }

        #endregion

        #region AbstractCommand members

        /// <summary>
        /// Create client query.
        /// </summary>
        public override IrbisClientQuery CreateQuery()
        {
            IrbisClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.FormatRecord;

            return result;
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        public override IrbisServerResponse Execute
            (
                IrbisClientQuery query
            )
        {
            Code.NotNull(query, "query");

            string database = Database ?? Connection.Database;
            query.Add(database);

            string preparedFormat = IrbisFormat.PrepareFormat
                (
                    FormatSpecification
                );

            query.Add
                (
                    new TextWithEncoding
                        (
                            preparedFormat,
                            IrbisEncoding.Ansi
                        )
                );

            if (MfnList.Count == 0)
            {
                query.Add(-2);
                query.Add(VirtualRecord);
            }
            else
            {
                query.Add(MfnList.Count);
                foreach (int mfn in MfnList)
                {
                    query.Add(mfn);
                }
            }

            IrbisServerResponse result = base.Execute(query);

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <summary>
        /// Verify object state.
        /// </summary>
        public override bool Verify
            (
                bool throwOnError
            )
        {
            bool result =
                !string.IsNullOrEmpty(FormatSpecification);

            if (result)
            {
                result = !ReferenceEquals(VirtualRecord, null)
                    || (MfnList.Count > 0);
            }

            if (result)
            {
                result = base.Verify(throwOnError);
            }

            if (!result && throwOnError)
            {
                throw new VerificationException();
            }

            return result;
        }

        #endregion
    }
}
