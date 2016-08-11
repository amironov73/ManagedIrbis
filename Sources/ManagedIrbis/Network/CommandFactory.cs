/* CommandFactory.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Text;
using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Network.Commands;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Network
{
    /// <summary>
    /// Command factory.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class CommandFactory
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; protected set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CommandFactory
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
        }

        static CommandFactory()
        {
            _superFactory = connection 
                => new CommandFactory(connection);
        }

        #endregion

        #region Private members

        private static Func<IrbisConnection, CommandFactory>
            _superFactory;

        #endregion

        #region Public methods

        /// <summary>
        /// Get ConnectCommand.
        /// </summary>
        [NotNull]
        public virtual ConnectCommand GetConnectCommand()
        {
            return new ConnectCommand(Connection);
        }

        /// <summary>
        /// Get CreateDatabaseCommand.
        /// </summary>
        [NotNull]
        public virtual CreateDatabaseCommand GetCreateDatabaseCommand()
        {
            return new CreateDatabaseCommand(Connection);
        }

        /// <summary>
        /// Get DatabaseStatCommand.
        /// </summary>
        [NotNull]
        public virtual DatabaseStatCommand GetDatabaseStatCommand()
        {
            return new DatabaseStatCommand(Connection);
        }

        /// <summary>
        /// Get default <see cref="CommandFactory"/>.
        /// </summary>
        [NotNull]
        public static CommandFactory GetDataultFactory
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            CommandFactory result = _superFactory(connection);

            return result;
        }

        /// <summary>
        /// Get FormatCommand.
        /// </summary>
        [NotNull]
        public virtual FormatCommand GetFormatCommand()
        {
            return new FormatCommand(Connection);
        }

        /// <summary>
        /// Get GblCommand.
        /// </summary>
        [NotNull]
        public virtual GblCommand GetGblCommand()
        {
            return new GblCommand(Connection);
        }

        /// <summary>
        /// Get GblVirtualCommand.
        /// </summary>
        [NotNull]
        public virtual GblVirtualCommand GetGblVirtualCommand()
        {
            return new GblVirtualCommand(Connection);
        }

        /// <summary>
        /// Get ListFilesCommand.
        /// </summary>
        [NotNull]
        public virtual ListFilesCommand GetListFilesCommand()
        {
            return new ListFilesCommand(Connection);
        }

        /// <summary>
        /// Get PrintTableCommand.
        /// </summary>
        [NotNull]
        public virtual PrintTableCommand GetPrintTableCommand()
        {
            return new PrintTableCommand(Connection);
        }

        /// <summary>
        /// Get ReadBinaryFileCommand();
        /// </summary>
        [NotNull]
        public virtual ReadBinaryFileCommand GetReadBinaryFileCommand()
        {
            return new ReadBinaryFileCommand(Connection);
        }

        /// <summary>
        /// Get ReadFileCommand.
        /// </summary>
        [NotNull]
        public virtual ReadFileCommand GetReadFileCommand()
        {
            return new ReadFileCommand(Connection);
        }

        /// <summary>
        /// Get ReadPostingsCommand.
        /// </summary>
        [NotNull]
        public virtual ReadPostingsCommand GetReadPostingsCommand()
        {
            return new ReadPostingsCommand(Connection);
        }

        /// <summary>
        /// Get ReadRecordCommand.
        /// </summary>
        [NotNull]
        public virtual ReadRecordCommand GetReadRecordCommand()
        {
            return new ReadRecordCommand(Connection);
        }

        /// <summary>
        /// Get ReadTermsCommand.
        /// </summary>
        [NotNull]
        public virtual ReadTermsCommand GeReadTermsCommand()
        {
            return new ReadTermsCommand(Connection);
        }

        /// <summary>
        /// Get SearchCommand.
        /// </summary>
        [NotNull]
        public virtual SearchCommand GetSearchCommand()
        {
            return new SearchCommand(Connection);
        }

        /// <summary>
        /// Get SearchReadCommand.
        /// </summary>
        [NotNull]
        public virtual SearchReadCommand GetSearchReadCommand()
        {
            return new SearchReadCommand(Connection);
        }

        /// <summary>
        /// Get UniversalCommand.
        /// </summary>
        [NotNull]
        public virtual UniversalCommand GetUniversalCommand
            (
                [NotNull] string commandCode,
                params object[] arguments
            )
        {
            return new UniversalCommand
                (
                    Connection,
                    commandCode,
                    arguments
                );
        }

        /// <summary>
        /// Get UniversalTextCommand.
        /// </summary>
        [NotNull]
        public virtual UniversalTextCommand GetUniversalTextCommand
            (
                [NotNull] string commandCode,
                [NotNull] string[] lines,
                [NotNull] Encoding encoding
            )
        {
            return new UniversalTextCommand
                (
                    Connection,
                    commandCode,
                    lines,
                    encoding
                );
        }

        /// <summary>
        /// Get UpdateUserListCommand.
        /// </summary>
        [NotNull]
        public virtual UpdateUserListCommand GetUpdateUserListCommand()
        {
            return new UpdateUserListCommand(Connection);
        }

        /// <summary>
        /// Get WriteFileCommand.
        /// </summary>
        [NotNull]
        public virtual WriteFileCommand GetWriteFileCommand()
        {
            return new WriteFileCommand(Connection);
        }

        /// <summary>
        /// Get WriteRecordCommand.
        /// </summary>
        [NotNull]
        public virtual WriteRecordCommand GetWriteRecordCommand()
        {
            return new WriteRecordCommand(Connection);
        }

        /// <summary>
        /// Get WriteRecordsCommand.
        /// </summary>
        [NotNull]
        public virtual WriteRecordsCommand GetWriteRecordsCommand()
        {
            return new WriteRecordsCommand(Connection);
        }

        /// <summary>
        /// Set Super Factory.
        /// </summary>
        [NotNull]
        public static Func<IrbisConnection, CommandFactory> SetSuperFactory
            (
                [NotNull] Func<IrbisConnection, CommandFactory> superFactory
            )
        {
            Code.NotNull(superFactory, "superFactory");

            Func<IrbisConnection, CommandFactory> result = _superFactory;
            _superFactory = superFactory;

            return result;
        }

        #endregion
    }
}
