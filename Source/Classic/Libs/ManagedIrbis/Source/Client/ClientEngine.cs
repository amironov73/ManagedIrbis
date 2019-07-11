// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ClientEngine.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.IO;
using AM.Text.Output;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Client executable engine.
    /// </summary>
    [PublicAPI]
    public sealed class ClientEngine
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Output.
        /// </summary>
        [NotNull]
        public AbstractOutput Output { get; set; }

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Client INI-file.
        /// </summary>
        [NotNull]
        public LocalCatalogerIniFile ClientIni { get; private set; }

        /// <summary>
        /// Remote INI-file.
        /// </summary>
        [NotNull]
        public RemoteCatalogerIniFile RemoteIni { get; private set; }

        /// <summary>
        /// Connection string.
        /// </summary>
        [NotNull]
        public string ConnectionString { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ClientEngine
            (
                [NotNull] string clientIniName
            )
        {
            Connection = new IrbisConnection();
            ClientIni = LocalCatalogerIniFile.Load(clientIniName);
            ConnectionString = ClientIni.BuildConnectionString();
            RemoteIni = new RemoteCatalogerIniFile(new IniFile());
            Output = new NullOutput();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Connect to the server.
        /// </summary>
        public bool Connect
            (
                [CanBeNull] string connectionString
            )
        {
            if (Connection.Connected)
            {
                WriteLine("Already connected");
                return true;
            }

            try
            {
                Connection.ParseConnectionString(ConnectionString);
                IniFile iniFile = Connection.Connect();
                RemoteIni = new RemoteCatalogerIniFile(iniFile);
            }
            catch (Exception exception)
            {
                WriteLine("Exception: " + exception.Message);
                return false;
            }

            return false;
        }

        /// <summary>
        /// Resolve the connection string.
        /// </summary>
        public bool ResolveConnectionString()
        {
            // TODO implement
            return true;
        }

        /// <summary>
        /// Write log line.
        /// </summary>
        public void WriteLine
            (
                [CanBeNull] string text
            )
        {
            Output.WriteLine(text);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable" />
        public void Dispose()
        {
            Connection.Dispose();
        }

        #endregion
    }
}
