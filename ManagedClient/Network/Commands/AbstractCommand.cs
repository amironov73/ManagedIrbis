/* AbstractCommand.cs -- abstract command of IRBIS protocol
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
    /// Abstract command of IRBIS protocol.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class AbstractCommand
        : IVerifiable
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
        /// Good return codes.
        /// </summary>
        // ReSharper disable VirtualMemberNeverOverriden.Global
        public virtual int[] GoodReturnCodes
        {
            get
            {
                return new int[0];
            }
        }
        // ReSharper restore VirtualMemberNeverOverriden.Global

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected AbstractCommand
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            _connection = connection;
        }

        #endregion

        #region Private members

        private readonly IrbisConnection _connection;

        #endregion

        #region Public methods

        /// <summary>
        /// Check the server response.
        /// </summary>
        public virtual void CheckResponse
            (
                [NotNull] IrbisServerResponse response
            )
        {
            Code.NotNull(response, "response");

            int returnCode = response.GetReturnCode();
            if (returnCode < 0)
            {
                int[] goodCodes = GoodReturnCodes;

                if (!goodCodes.Contains(returnCode))
                {
                    throw new IrbisException(returnCode);
                }
            }
        }

        /// <summary>
        /// Create client query.
        /// </summary>
        public virtual IrbisClientQuery CreateQuery ()
        {
            IrbisClientQuery result = new IrbisClientQuery
            {
                Workstation = Connection.Workstation,
                ClientID = Connection.ClientID,
                CommandNumber = Connection.IncrementCommandNumber(),
                UserLogin = Connection.Username,
                UserPassword = Connection.Password
            };

            return result;
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        [NotNull]
        public virtual IrbisServerResponse Execute
            (
                [NotNull] IrbisClientQuery query
            )
        {
            Code.NotNull(query, "query");

            byte[] request = query.EncodePacket();
            byte[] answer = Connection.Socket.ExecuteRequest(request);

            IrbisServerResponse result = IrbisServerResponse.Parse
                (
                    Connection,
                    answer
                );

            //string[] decoded = PacketInterpreter.Interpret
            //    (
            //        result.Packet,
            //        "AIIIAAAAAAT"
            //    );
            //IrbisNetworkDebugger.Log(decoded);

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <summary>
        /// Verify object state.
        /// </summary>
        public virtual bool Verify
            (
                bool throwOnError
            )
        {
            return true;
        }

        #endregion
    }
}
