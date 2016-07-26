/* PrintTableCommand.cs --
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

namespace ManagedIrbis.Network.Commands
{
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PrintTableCommand
        : AbstractCommand
    {
        #region Properties

        public TableDefinition Definition { get; set; }

        public string Result { get; set; }

        #endregion

        #region Construction

        public PrintTableCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region AbstractCommand members

        public override void CheckResponse
            (
                ServerResponse response
            )
        {
            Code.NotNull(response, "response");

            // Ignore the result
            response._returnCodeRetrieved = true;
        }

        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.Print;

            // "7"         PRINT
            // "IBIS"      database
            // "@tabf1w"   table
            // ""          headers
            // ""          mod
            // "T=A$"      search
            // "0"         min
            // "0"         max
            // ""          sequential
            // ""          mfn list

            result
                .Add(Definition.DatabaseName)
                .Add(Definition.Table)
                .Add(string.Empty) // instead of headers
                .Add(Definition.Mod)
                .AddUtf8(Definition.SearchQuery)
                .Add(Definition.MinMfn)
                .Add(Definition.MaxMfn)
                .AddUtf8(Definition.SequentialQuery)
                .Add(string.Empty) // instead of MFN list
                ;

            return result;
        }

        public override ServerResponse Execute
            (
                ClientQuery query
            )
        {
            Code.NotNull(query, "query");

            ServerResponse result = base.Execute(query);

            Result = "{\\rtf1 "
                + result.RemainingUtfText()
                + "}";

            return result;
        }

        #endregion
    }
}
