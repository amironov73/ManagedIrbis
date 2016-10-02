/* PftMfn.cs -- вывод номера записи
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Для вывода номера записи в файле документов служит
    /// команда MFN, формат которой:
    /// 
    /// MFN или MFN(d),
    /// 
    /// где d - количество выводимых на экран цифр.
    /// Если параметр(d) опущен, то по умолчанию
    /// предполагается 6 цифр.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftMfn
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Width of the output.
        /// </summary>
        public int Width { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftMfn()
        {
            Width = 0;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftMfn
        (
            int width
        )
        {
            Code.Nonnegative(width, "width");

            Width = width;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftMfn
        (
            [NotNull] PftToken token
        )
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Mfn);

            Width = 0;

            try
            {
                if (!ReferenceEquals(token.Text, null))
                {
                    Width = int.Parse(token.Text);
                    if (Width < 0)
                    {
                        throw new PftSyntaxException(token);
                    }
                }
            }
            catch (Exception exception)
            {
                throw new PftSyntaxException(token, exception);
            }

        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftNode members

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (context.Record != null)
            {
                string text = context.Record.Mfn
                    .ToInvariantString();

                context.Write
                    (
                        this,
                        text
                    );
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc />
        public override void Write
            (
                StreamWriter writer
            )
        {
            writer.Write("mfn");
            if (Width > 0)
            {
                writer.Write('(');
                writer.Write(Width);
                writer.Write(')');
            }
        }

        #endregion
    }
}
