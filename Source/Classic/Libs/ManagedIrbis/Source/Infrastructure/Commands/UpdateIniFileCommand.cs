// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UpdateIniFileCommand.cs -- unlock the database
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Commands
{
    /// <summary>
    /// Unlock the database on the server.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class UpdateIniFileCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// Text to update.
        /// </summary>
        [CanBeNull]
        public string[] Lines { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public UpdateIniFileCommand
            (
                [NotNull] IIrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region AbstractCommand members

        /// <inheritdoc />
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.UpdateIniFile;

            if (Lines != null)
            {
                foreach (string line in Lines)
                {
                    result.AddAnsi(line);
                }
            }

            return result;
        }

        /// <inheritdoc />
        public override ServerResponse Execute
            (
                ClientQuery query
            )
        {
            Code.NotNull(query, "query");

            ServerResponse result = base.Execute(query);
            result.GetReturnCode();

            return result;
        }

        #endregion
    }
}
