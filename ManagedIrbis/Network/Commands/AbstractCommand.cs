/* AbstractCommand.cs -- abstract command of IRBIS protocol
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

using ManagedIrbis.Network.Resolving;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Network.Commands
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
        public virtual int[] GoodReturnCodes
        {
            get
            {
                return new int[0];
            }
        }

        /// <summary>
        /// Problem resolver (if specified)
        /// </summary>
        [CanBeNull]
        public AbstractResolver ProblemResolver { get; set; }

        /// <summary>
        /// Relax (may be malformed) server response.
        /// </summary>
        public bool RelaxResponse { get; set; }

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
                [NotNull] ServerResponse response
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
        public virtual ClientQuery CreateQuery ()
        {
            ClientQuery result = new ClientQuery
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
        public virtual ServerResponse Execute
            (
                [NotNull] ClientQuery query
            )
        {
            Code.NotNull(query, "query");

            byte[] request = query.EncodePacket();
            byte[] answer = Connection.Socket.ExecuteRequest(request);

            ServerResponse result = new ServerResponse
                (
                    Connection,
                    answer,
                    request,
                    RelaxResponse
                );

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
