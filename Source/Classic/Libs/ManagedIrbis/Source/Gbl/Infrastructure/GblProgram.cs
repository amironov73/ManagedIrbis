// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GblProgram.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Gbl.Infrastructure
{
    /// <summary>
    /// GBL program.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GblProgram
    {
        #region Properties

        /// <summary>
        /// Nodes.
        /// </summary>
        [NotNull]
        public GblNodeCollection Nodes { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblProgram()
        {
            Nodes = new GblNodeCollection(null);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Initialize the program.
        /// </summary>
        public void Initialize
            (
                [NotNull] GblContext context,
                [NotNull] GblParser parser
            )
        {
            foreach (GblNode node in Nodes)
            {
                node.Initialize(context, parser);
            }
        }

        /// <summary>
        /// Execute the program.
        /// </summary>
        public void Execute
            (
                [NotNull] GblContext context
            )
        {
            Code.NotNull(context, "context");

            while (context.Advance())
            {
                MarcRecord record = context.CurrentRecord;
                if (ReferenceEquals(record, null))
                {
                    Log.Warn("GblProgram::Execute: current record = null");
                    break;
                }

                foreach (GblNode node in Nodes)
                {
                    node.Execute(context);
                }

                record = context.CurrentRecord;
                if (!ReferenceEquals(record, null)
                    && record.Modified)
                {
                    context.Logger.WriteLine("Record updated: {0}", record.Mfn);
                    context.RecordSource.WriteRecord(record);
                }
            }
        }

        #endregion
    }
}
