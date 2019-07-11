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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
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

        /// <summary>
        /// Database list.
        /// </summary>
        [NotNull]
        public DatabaseInfo[] Databases { get; set; }

        /// <summary>
        /// Current database.
        /// </summary>
        [NotNull]
        public DatabaseInfo CurrentDatabase { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int ClientTimeLive { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int MaxMfn { get; set; }

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
            Databases = EmptyArray<DatabaseInfo>.Value;

            // Default values

            CurrentDatabase = new DatabaseInfo
            {
                Name = "IBIS",
                Description = string.Empty
            };
            ClientTimeLive = 100; // TODO ???
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
                ReloadContext(true);
            }
            catch (Exception exception)
            {
                WriteLine("Exception: " + exception.Message);
                return false;
            }

            return false;
        }

        /// <summary>
        /// Execute in background.
        /// </summary>
        public void Execute(Action action)
        {
            action();
        }

        /// <summary>
        /// Execute in background.
        /// </summary>
        public void Execute<T>(Action<T> action, T argument1)
        {
            action(argument1);
        }

        /// <summary>
        /// Execute in background.
        /// </summary>
        public T Execute<T>(Func<T> func)
        {
            return func();
        }

        /// <summary>
        /// Execute in background.
        /// </summary>
        public T2 Execute<T1, T2>(Func<T1, T2> func, T1 argument1)
        {
            return func(argument1);
        }

        /// <summary>
        /// Reload the context.
        /// </summary>
        /// <param name="full"></param>
        public void ReloadContext(bool full)
        {
            if (full)
            {
                string dbnam = RemoteIni.DatabaseList;
                dbnam = Path.ChangeExtension(dbnam, ".mnu");
                Databases = Connection.ListDatabases(dbnam);
                if (Databases.Length == 0)
                {
                    throw new IrbisException();
                }

                CurrentDatabase = Databases[0];
                // TODO use DefaultDB

                // TODO read 0..ISISACW.TAB

                // TODO read 0..SETPRIV.WSS

                // TODO read 0..UNICODE.MNU

                // TODO read 0..UPMNU.MNU

                // TODO read 0..WEBIRBIS.MNU

                // TODO read 0..WEBTRANSFER.MNU

                // TODO read 0..CUSTOMVKB.MNU
            }

            MaxMfn = Connection.GetMaxMfn();
        }

        /// <summary>
        /// Resolve the connection string.
        /// </summary>
        /// <returns><c>true</c> if can continue execution.</returns>
        public bool ResolveConnectionString()
        {
            // TODO implement
            return true;
        }

        /// <summary>
        /// Resolve the exception.
        /// </summary>
        /// <returns><c>true</c> if can continue execution.</returns>
        public bool ResolveException(Exception exception)
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
