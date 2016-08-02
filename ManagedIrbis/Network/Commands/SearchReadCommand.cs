/* SearchReadCommand.cs -- search and read records from IRBIS-server
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

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Network.Commands
{
    /// <summary>
    /// Search and read records from IRBIS-server.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SearchReadCommand
        : SearchCommand
    {
        #region Properties

        /// <summary>
        /// Format specification (always ALL).
        /// </summary>
        [NotNull]
        public override string FormatSpecification
        {
            get { return IrbisFormat.All; }
            set
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Read records.
        /// </summary>
        [CanBeNull]
        public MarcRecord[] Records { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchReadCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region Private members

        private MarcRecord _ConvertRecord
            (
                FoundItem item
            )
        {
            MarcRecord result = new MarcRecord
            {
                HostName = Connection.Host,
                Database = Database ?? Connection.Database
            };
            ProtocolText.ParseResponseForAllFormat
                (
                    item.Text.ThrowIfNull("item.Text"),
                    result
                );
            Debug.Assert
                (
                    item.Mfn == result.Mfn,
                    "item.Mfn == result.Mfn"
                );

            return result;
        }

        #endregion

        #region AbstractCommand members

        public override ServerResponse Execute
            (
                ClientQuery clientQuery
            )
        {
            ServerResponse result = base.Execute(clientQuery);

            if (result.ReturnCode == 0)
            {
                Records = Found
                    .ThrowIfNull("Found")
                    .AsParallel()
                    .AsOrdered()
                    .Select
                    (
                        // ReSharper disable ConvertClosureToMethodGroup
                        item => _ConvertRecord(item)
                        // ReSharper restore ConvertClosureToMethodGroup
                    )
                    .ToArray();
            }

            return result;
        }

        #endregion
    }
}
