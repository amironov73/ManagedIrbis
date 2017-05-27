// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ActualizeRecordCommand.cs -- actualize record on the server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;

using AM;
using AM.Logging;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Commands
{
    /// <summary>
    /// Actualize given record or whole database on the server.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("MFN={Mfn}")]
    public class ActualizeRecordCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

        /// <summary>
        /// MFN of record to actualize.
        /// </summary>
        /// <remarks>MFN = 0 means 'actualize whole database'.
        /// </remarks>
        public int Mfn { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connection"></param>
        public ActualizeRecordCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region AbstractCommand members

        /// <inheritdoc cref="AbstractCommand.CreateQuery"/>
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.ActualizeRecord;

            string database = Database ?? Connection.Database;
            if (string.IsNullOrEmpty(database))
            {
                Log.Error
                    (
                        "ActualizeRecordCommand::CreateQuery: "
                        + "database not specified"
                    );

                throw new IrbisException("database not specified");
            }
            result.AddAnsi(database);

            result.Add(Mfn);

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public override bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ActualizeRecordCommand> verifier
                = new Verifier<ActualizeRecordCommand>
                (
                    this,
                    throwOnError
                );

            verifier
                .NotNullNorEmpty(Database, "Database")
                .Assert(Mfn >= 0, "Mfn >= 0");

            return verifier.Result;
        }

        #endregion
    }
}
