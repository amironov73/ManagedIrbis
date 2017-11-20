// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DatabaseStatCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Linq;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure.Commands
{
    /// <summary>
    /// Database stat.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class DatabaseStatCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// Client query definition.
        /// </summary>
        public StatDefinition Definition { get; set; }

        /// <summary>
        /// Result of the command.
        /// </summary>
        public string Result { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DatabaseStatCommand
            (
                [NotNull] IIrbisConnection connection
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

        /// <inheritdoc cref="AbstractCommand.CheckResponse" />
        public override void CheckResponse
            (
                ServerResponse response
            )
        {
            Code.NotNull(response, "response");

            // Ignore the result
            response.RefuseAnReturnCode();
        }

        /// <inheritdoc cref="AbstractCommand.CreateQuery" />
        public override ClientQuery CreateQuery()
        {
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.DatabaseStat;

            // "2"               STAT
            // "IBIS"            database
            // "v200^a,10,100,1" field
            // "T=A$"            search
            // "0"               min
            // "0"               max
            // ""                sequential
            // ""                mfn list

            string items = string.Join
                (
                    IrbisText.IrbisDelimiter,
                    Definition.Items
                        .Select(item => item.ToString())
                        .ToArray()
                );

            result
                .Add(Definition.DatabaseName)
                .Add(items)
                .AddUtf8(Definition.SearchQuery)
                .Add(Definition.MinMfn)
                .Add(Definition.MaxMfn)
                .AddUtf8(Definition.SequentialQuery)
                .Add(string.Empty) // instead of MFN list
                ;

            return result;
        }

        /// <inheritdoc cref="AbstractCommand.Execute" />
        public override ServerResponse Execute
            (
                ClientQuery query
            )
        {
            Code.NotNull(query, "query");

            ServerResponse result = base.Execute(query);

            Result = "{\\rtf1 "
                + result.RemainingUtfText()
                + "}";

            return result;
        }

        #endregion
    }
}
