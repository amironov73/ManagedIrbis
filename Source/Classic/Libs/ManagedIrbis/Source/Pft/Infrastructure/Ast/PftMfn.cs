/* PftMfn.cs -- вывод номера записи
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Globalization;
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
        #region Constants

        /// <summary>
        /// Default width.
        /// </summary>
        public int DefaultWidth = 10;

        #endregion

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
            Width = DefaultWidth;
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

            Width = DefaultWidth;

            try
            {
                string text = token.Text;
                if (!string.IsNullOrEmpty(text))
                {
                    if (text.Length > 3)
                    {
                        text = text.Substring(3);
                        text = text.TrimStart('(').TrimEnd(')');

                        Width = int.Parse(text);
                        if (Width <= 0)
                        {
                            throw new PftSyntaxException(token);
                        }
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

            if (!context.BreakFlag)
            {
                if (context.Record != null)
                {
                    string text;
                    if (Width == 0)
                    {
                        text = context.Record.Mfn
                            .ToInvariantString();
                    }
                    else
                    {
                        string format = new string('0', Width);
                        text = context.Record.Mfn.ToString
                            (
                                format,
                                CultureInfo.InvariantCulture
                            );
                    }

                    context.Write
                        (
                            this,
                            text
                        );
                }
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
            if (Width > 0
                && Width != DefaultWidth)
            {
                writer.Write('(');
                writer.Write(Width);
                writer.Write(')');
            }
        }

        #endregion
    }
}
