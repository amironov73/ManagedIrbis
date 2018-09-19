// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CommandMapper.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    /// Maps codes to command
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class CommandMapper
    {
        #region Properties

        /// <summary>
        /// Engine.
        /// </summary>
        [NotNull]
        public IrbisServerEngine Engine { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CommandMapper
            (
                [NotNull] IrbisServerEngine engine
            )
        {
            Code.NotNull(engine, "engine");

            Engine = engine;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Map the command.
        /// </summary>
        [NotNull]
        public virtual ServerCommand MapCommand
            (
                [NotNull] WorkData data
            )
        {
            Code.NotNull(data, "data");

            ServerCommand result;
            ClientRequest request = data.Request;

            if (ReferenceEquals(request.CommandCode1, null)
                || request.CommandCode1 != request.CommandCode2)
            {
                throw new IrbisException();
            }

            string commandCode = request.CommandCode1.ToUpperInvariant();

            switch (commandCode)
            {
                case "!":
                    result = new ListFilesCommand(data);
                    break;

                case "1":
                    result = new ServerVersionCommand(data);
                    break;

                case "8":
                    result = new UpdateIniFileCommand(data);
                    break;

                case "A":
                    result = new ConnectCommand(data);
                    break;

                case "B":
                    result = new DisconnectCommand(data);
                    break;

                case "C":
                    result = new ReadRecordCommand(data);
                    break;

                case "D":
                    result = new WriteRecordCommand(data);
                    break;

                case "G":
                    result = new FormatRecordCommand(data);
                    break;

                case "H":
                    result = new ReadTermsCommand(data);
                    break;

                case "K":
                    result = new SearchCommand(data);
                    break;

                case "L":
                    result = new ReadFileCommand(data);
                    break;

                case "N":
                    result = new NopCommand(data);
                    break;

                case "O":
                    result = new GetMaxMfnCommand(data);
                    break;

                default:
                    throw new IrbisException();
            }

            return result;
        }

        #endregion
    }
}
