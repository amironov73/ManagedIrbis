// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftProgram.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// AST root node.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PftProgram
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Procedures.
        /// </summary>
        [NotNull]
        public PftProcedureManager Procedures { get; internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftProgram()
        {
            Procedures = new PftProcedureManager();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            Procedures.Deserialize(reader);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            Code.NotNull(context, "context");

            try
            {
                context.Procedures = Procedures;
                base.Execute(context);
            }
            catch (PftBreakException exception)
            {
                // It was break operator

                Log.TraceException
                    (
                        "PftProgram::Execute",
                        exception
                    );

                if (!ReferenceEquals(context.Parent, null))
                {
                    throw;
                }
            }
            catch (PftExitException exception)
            {
                // It was exit operator

                Log.TraceException
                    (
                        "PftProgram::Execute",
                        exception
                    );

                if (!ReferenceEquals(context.Parent, null))
                {
                    throw;
                }
            }
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            Procedures.Serialize(writer);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            PftPrettyPrinter printer = new PftPrettyPrinter();
            PrettyPrint(printer);
            string result = printer.ToString();

            return result;
        }

        #endregion
    }
}
