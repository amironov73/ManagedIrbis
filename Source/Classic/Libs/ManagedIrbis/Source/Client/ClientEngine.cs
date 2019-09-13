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

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

#endregion

// ReSharper disable StringLiteralTypo

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
        /// Context.
        /// </summary>
        [NotNull]
        public ResourceDictionary<string> Context { get; private set; }

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
            Context = new ResourceDictionary<string>();
            Databases = EmptyArray<DatabaseInfo>.Value;
            _fileQueue = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            // Default values

            CurrentDatabase = new DatabaseInfo
            {
                Name = "IBIS",
                Description = string.Empty
            };
            ClientTimeLive = 100; // TODO ???
        }

        #endregion

        #region Private members

        [NotNull]
        private readonly HashSet<string> _fileQueue;

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
        /// Enqueue the file to download.
        /// </summary>
        public void EnqueueTextFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNull(fileName, "fileName");

            if (!Context.Have(fileName))
            {
                _fileQueue.Add(fileName);
            }
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
        /// Read database-specific context.
        /// </summary>
        public void ReadDatabaseSpecificContext()
        {
            // TODO implement
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

                EnqueueTextFile("0..ISISACW.TAB");
                EnqueueTextFile("0..SETPRIV.WSS");
                EnqueueTextFile("0..UNICODE.MNU");
                EnqueueTextFile("0..UPMNU.MNU");
                EnqueueTextFile("0..WEBIRBIS.MNU");
                EnqueueTextFile("0..WEBTRANSFER.MNU");
                EnqueueTextFile("0..CUSTOMVKB.MNU");

                ReadEnqueuedFiles();
                ReadDatabaseSpecificContext();
            }

            MaxMfn = Connection.GetMaxMfn();
        }

        /// <summary>
        /// Read enqueued files.
        /// </summary>
        public void ReadEnqueuedFiles()
        {
            foreach (string fileName in _fileQueue)
            {
                ReadTextFile(fileName);
            }
            _fileQueue.Clear();
        }

        /// <summary>
        /// Require text file from the server.
        /// </summary>
        [CanBeNull]
        public string ReadTextFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            string result = Context.Get(fileName);
            if (!string.IsNullOrEmpty(result))
            {
                WriteLine(string.Format("Reading from cache: {0}", fileName));
            }
            else
            {
                WriteLine(string.Format("Reading file: {0}", fileName));
                FileSpecification specification = FileSpecification.Parse(fileName);
                result = Connection.ReadTextFile(specification);
                if (!string.IsNullOrEmpty(result))
                {
                    Context.Put(fileName, result);
                }
            }

            return result;
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
        /// Require text file from the server.
        /// </summary>
        [NotNull]
        public string RequireTextFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            string result = Context.Get(fileName);
            if (!string.IsNullOrEmpty(result))
            {
                WriteLine(string.Format("Reading from cache: {0}", fileName));
            }
            else
            {
                WriteLine(string.Format("Reading file: {0}", fileName));
                FileSpecification specification = FileSpecification.Parse(fileName);
                result = Connection.RequireTextFile(specification);
                Context.Put(fileName, result);
            }

            return result;
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
