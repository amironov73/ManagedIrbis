// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Infrastructure.Commands
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
        public IrbisConnection Connection { get; private set; }

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
        /// Relax (may be malformed) server response.
        /// </summary>
        public bool RelaxResponse { get; set; }

        /// <summary>
        /// Does the command require established connection?
        /// </summary>
        public virtual bool RequireConnection { get { return true; } }

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

            Connection = connection;
        }

        #endregion

        #region Private members

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

            int returnCode = response.ReturnCode;
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

        /// <summary>
        /// Parse client query.
        /// </summary>
        public virtual void ParseClientQuery
            (
                [NotNull] byte[] clientQuery
            )
        {
            Code.NotNull(clientQuery, "clientQuery");
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
