// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftC.cs -- табуляция в указанную позицию строки
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

using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Команда горизонтального позиционирования.
    /// Перемещает виртуальный курсор в n-ю позицию строки
    /// (табуляция в указанную позицию строки).
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftC
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Новая позиция курсора.
        /// </summary>
        public int NewPosition { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftC()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftC
            (
                int position
            )
        {
            NewPosition = position;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftC
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.C);

            try
            {
                NewPosition = int.Parse
                (
                    token.Text.ThrowIfNull("token.Text")
                );
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "PftC::Constructor",
                        exception
                    );

                throw new PftSyntaxException(token, exception);
            }
        }


        #endregion

        #region Private members

        private void _Execute
            (
                PftContext context
            )
        {
            int current = context.Output.GetCaretPosition();
            int delta = NewPosition - current;
            if (delta > 0)
            {
                context.Write
                    (
                        this,
                        new string(' ', delta)
                    );
            }
            else
            {
                context.WriteLine(this);
                context.Write
                    (
                        this,
                        new string(' ', NewPosition - 1)
                    );
            }
        }

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

            NewPosition = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (!ReferenceEquals(context.CurrentField, null))
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

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            // Всегда в нижнем регистре
            printer.Write
                (
                    " c{0} ",
                    NewPosition.ToInvariantString()
                );
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            writer.WritePackedInt32(NewPosition);
        }

        #endregion
    }
}
