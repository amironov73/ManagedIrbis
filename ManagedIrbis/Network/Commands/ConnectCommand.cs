/* ConnectCommand.cs -- connect to the IRBIS64 server
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Network.Commands
{
    /// <summary>
    /// Connect to the IRBIS64 server.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ConnectCommand
        : AbstractCommand
    {
        #region Constants

        /// <summary>
        /// Response specification.
        /// </summary>
        public const string ResponseSpecification = "AIIIAAAAAAT";

        #endregion

        #region Properties

        #endregion

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
        public override ServerResponse Execute
            (
                ClientQuery query
            )
        {
            Code.NotNull(query, "query");

            query.CommandCode = CommandCode.RegisterClient;
            query.Arguments.Add(Connection.Username);
            query.Arguments.Add(Connection.Password);

            ServerResponse result = base.Execute(query);

            //string[] decoded = PacketInterpreter.Interpret
            //    (
            //        result.Packet,
            //        ResponseSpecification
            //    );
            //IrbisNetworkDebugger.Log(decoded);

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
            Verifier<ConnectCommand> verifier
                = new Verifier<ConnectCommand>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Connection.Username, "username")
                .NotNullNorEmpty(Connection.Password, "password");

            return verifier.Result;
        }

        #endregion
    }
}
