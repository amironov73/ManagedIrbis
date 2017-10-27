// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftX.cs -- вставляет n пробелов
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using AM;
using AM.IO;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Команда горизонтального позиционирования.
    /// Вставляет n пробелов.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftX
        : PftNode
    {
        #region Properties

        /// <inheritdoc cref="PftNode.ConstantExpression" />
        public override bool ConstantExpression
        {
            get { return true; }
        }

        /// <inheritdoc cref="PftNode.RequiresConnection" />
        public override bool RequiresConnection
        {
            get { return false; }
        }

        /// <summary>
        /// Количество добавляемых пробелов.
        /// </summary>
        public int Shift { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftX()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftX
            (
                int shift
            )
        {
            Shift = shift;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftX
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.X);

            try
            {
                Shift = int.Parse
                (
                    token.Text.ThrowIfNull("token.Text")
                );
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "PftX::Constructor",
                        exception
                    );

                throw new PftSyntaxException(token, exception);
            }
        }

        #endregion

        #region Private members

        private void _Execute
            (
                [NotNull] PftContext context
            )
        {
            if (Shift > 0)
            {
                context.Write
                    (
                        this,
                        new string(' ', Shift)
                    );
            }
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.CompareNode" />
        internal override void CompareNode
            (
                PftNode otherNode
            )
        {
            base.CompareNode(otherNode);

            PftX otherX = (PftX) otherNode;
            bool result = Shift == otherX.Shift;

            if (!result)
            {
                throw new PftSerializationException();
            }
        }

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .WriteLine("if (CurrentField != null)")
                .WriteIndent()
                .WriteLine("{")
                .WriteIndent()
                .IncreaseIndent()
                .WriteIndent()
                .WriteLine("if (FirstRepeat(CurrentField))")
                .WriteIndent()
                .WriteLine("{")
                .IncreaseIndent()
                .WriteIndent()
                .WriteLine("Context.Write(new string(' ', {0}));", Shift)
                .DecreaseIndent()
                .WriteIndent()
                .WriteLine("}")
                .DecreaseIndent()
                .WriteIndent()
                .WriteLine("}")
                .WriteIndent()
                .WriteLine("else")
                .WriteIndent()
                .WriteLine("{")
                .IncreaseIndent()
                .WriteIndent()
                .WriteLine("Context.Write(new string(' ', {0}));", Shift)
                .DecreaseIndent()
                .WriteIndent()
                .WriteLine("}");

            compiler.EndMethod(this);
            compiler.MarkReady(this);
        }

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            Shift = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (context.CurrentField != null)
            {
                if (context.CurrentField.IsFirstRepeat(context))
                {
                    _Execute(context);
                }
            }
            else
            {
                _Execute(context);
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            writer.WritePackedInt32(Shift);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer
                .SingleSpace()
                .Write
                    (
                        "x{0}", // Всегда в нижнем регистре
                        Shift.ToInvariantString()
                    )
                .SingleSpace();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="PftNode.ToString" />
        public override string ToString()
        {
            return "x" + Shift.ToInvariantString();
        }

        #endregion
    }
}
