/* ConnectedClient.cs -- 
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
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ConnectedClient
        : AbstractClient
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConnectedClient()
        {
            _ownConnection = true;
            Connection = new IrbisConnection();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConnectedClient
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            _ownConnection = false;
            Connection = connection;
        }

        #endregion

        #region Private members

        private readonly bool _ownConnection;

        #endregion

        #region Public methods

        /// <summary>
        /// Connect.
        /// </summary>
        public void Connect()
        {
            Connection.Connect();
        }

        /// <summary>
        /// Disconnect.
        /// </summary>
        public void Disconnect()
        {
            Connection.Dispose();
        }

        /// <summary>
        /// Parse connection string.
        /// </summary>
        public void ParseConnectionString
            (
                [NotNull] string connectionString
            )
        {
            Code.NotNullNorEmpty(connectionString, "connectionString");
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc/>
        public override void Dispose()
        {
            if (_ownConnection)
            {
                Connection.Dispose();
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
