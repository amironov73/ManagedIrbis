// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftMode.cs -- переключение режима вывода
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Переключение режима вывода полей/подполей.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftMode
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Output mode.
        /// </summary>
        public PftFieldOutputMode OutputMode { get; set; }

        /// <summary>
        /// Upper-case mode.
        /// </summary>
        public bool UpperMode { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftMode()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftMode
            (
                [NotNull] string text
            )
        {
            try
            {
                ParseText(text);
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "PftMode::Constructor",
                        exception
                    );

                throw new PftSyntaxException
                    (
                        "bad mode text",
                        exception
                    );
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftMode
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Mpl);

            try
            {
                ParseText
                    (
                        token.Text.ThrowIfNull("token.Text")
                    );
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "PftMode::Constructor",
                        exception
                    );

                throw new PftSyntaxException(token, exception);
            }
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse specified text.
        /// </summary>
        public void ParseText
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            text = text.ToLower();
            if (text.Length != 3)
            {
                Log.Error
                    (
                        "PftMode::ParseText: "
                        + "text.Length != 3: "
                        + text.ToVisibleString()
                    );

                throw new ArgumentException("mode");
            }

            switch (text[1])
            {
                case 'p':
                    OutputMode = PftFieldOutputMode.PreviewMode;
                    break;

                case 'h':
                    OutputMode = PftFieldOutputMode.HeaderMode;
                    break;

                case 'd':
                    OutputMode = PftFieldOutputMode.DataMode;
                    break;

                default:
                    Log.Error
                        (
                            "PftMode::ParseText: "
                            + "unexpected mode="
                            + text.ToVisibleString()
                        );

                    throw new ArgumentException();
            }
            switch (text[2])
            {
                case 'u':
                    UpperMode = true;
                    break;

                case 'l':
                    UpperMode = false;
                    break;

                default:
                    Log.Error
                        (
                            "PftMode::ParseText: "
                            + "unexpected mode="
                            + text.ToVisibleString()
                        );

                    throw new ArgumentException();
            }
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            context.FieldOutputMode = OutputMode;
            context.UpperMode = UpperMode;

            OnAfterExecution(context);
        }

        #endregion
    }
}
