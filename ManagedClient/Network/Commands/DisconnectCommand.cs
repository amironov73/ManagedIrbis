/* DisconnectCommand.cs -- 
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
    public sealed class DisconnectCommand
        : AbstractCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DisconnectCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region AbstractCommand members

        //public override int[] GoodReturnCodes
        //{
        //    get { return new [] {-3335}; }
        //}

        /// <summary>
        /// Check response.
        /// </summary>
        public override void CheckResponse
            (
                IrbisServerResponse response
            )
        {
            // Nothing to do
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        public override IrbisServerResponse Execute
            (
                IrbisClientQuery query
            )
        {
            query.CommandCode = CommandCode.UnregisterClient;
            query.Arguments.Add(Connection.Username);

            IrbisServerResponse result = base.Execute(query);

            return result;
        }

        #endregion
    }
}
