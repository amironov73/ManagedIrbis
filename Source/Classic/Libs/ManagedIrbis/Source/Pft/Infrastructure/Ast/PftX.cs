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
                );
        }

        #endregion
    }
}
