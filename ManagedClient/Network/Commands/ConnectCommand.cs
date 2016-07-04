/* ConnectCommand.cs -- 
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Network.Commands
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ConnectCommand
        : AbstractCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConnectCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
            Connection.GenerateClientID();
            Connection.ResetCommandNumber();
        }

        #endregion

        #region AbstractCommand members

        /// <summary>
        /// Execute the command.
        /// </summary>
        public override IrbisServerResponse Execute
            (
                IrbisClientQuery query
            )
        {
            query.CommandCode = CommandCode.RegisterClient;
            query.Arguments.Add(Connection.Username);
            query.Arguments.Add(Connection.Password);

            StringWriter writer = new StringWriter();
            query.Dump(writer);
            string dump = writer.ToString();

            IrbisServerResponse result = base.Execute(query);

            return result;
        }

        #endregion
    }
}
