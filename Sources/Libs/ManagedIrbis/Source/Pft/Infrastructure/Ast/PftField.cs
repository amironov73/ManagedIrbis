/* PftField.cs -- ссылка на поле
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM;
using AM.Collections;
using AM.Text;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Ссылка на поле.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftField
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Left hand.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftNode> LeftHand { get; private set; }

        /// <summary>
        /// Right hand.
        /// </summary>
        [NotNull]
        public NonNullCollection<PftNode> RightHand { get; private set; }

        /// <summary>
        /// Command.
        /// </summary>
        public char Command { get; set; }

        /// <summary>
        /// Embedded.
        /// </summary>
        [CanBeNull]
        public string Embedded { get; set; }

        /// <summary>
        /// Отступ.
        /// </summary>
        public int Indent { get; set; }

        /// <summary>
        /// Начальный номер повторения.
        /// </summary>
        public int IndexFrom { get; set; }

        /// <summary>
        /// Конечный номер повторения.
        /// </summary>
        public int IndexTo { get; set; }

        /// <summary>
        /// Смещение.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Длина.
        /// </summary>
        public int Length { get; set; }


        /// <summary>
        /// Subfield.
        /// </summary>
        public char SubField { get; set; }

        /// <summary>
        /// Tag.
        /// </summary>
        [CanBeNull]
        public string Tag { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftField()
        {
            LeftHand = new NonNullCollection<PftNode>();
            RightHand = new NonNullCollection<PftNode>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftField
            (
                [NotNull] string text
            )
            : this()
        {
            Code.NotNullNorEmpty(text, "text");

            Parse(text);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftField
            (
                [NotNull] PftToken token
            )
            : this()
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.V);

            try
            {
                Parse
                    (
                        token.Text
                        .ThrowIfNull("token.Text")
                    );
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

        /// <summary>
        /// Parse the text.
        /// </summary>
        public void Parse
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            TextNavigator navigator = new TextNavigator(text);
            char c = navigator.ReadChar();
            StringBuilder builder = new StringBuilder();

            switch (c)
            {
                case 'd':
                case 'D':
                    Command = 'd';
                    break;

                case 'n':
                case 'N':
                    Command = 'n';
                    break;

                case 'v':
                case 'V':
                    Command = 'v';
                    break;

                default:
                    throw new PftSyntaxException(navigator);
            }

            c = navigator.ReadChar();
            if (!c.IsArabicDigit())
            {
                throw new PftSyntaxException(navigator);
            }
            builder.Append(c);

            while (true)
            {
                c = navigator.PeekChar();
                if (!c.IsArabicDigit())
                {
                    break;
                }
                navigator.ReadChar();
                builder.Append(c);
            }
            Tag = builder.ToString();

            if (c == '@')
            {
                builder.Length = 0;
                navigator.ReadChar();
                while (true)
                {
                    c = navigator.PeekChar();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    navigator.ReadChar();
                    builder.Append(c);
                }
                if (builder.Length == 0)
                {
                    throw new PftSyntaxException(navigator);
                }
                Embedded = builder.ToString();
            }

            if (c == '^')
            {
                navigator.ReadChar();
                if (navigator.IsEOF)
                {
                    throw new PftSyntaxException(navigator);
                }
                c = navigator.ReadChar();
                if (!SubFieldCode.IsValidCode(c))
                {
                    throw new PftSyntaxException(navigator);
                }
                SubField = c;
            }

            if (c == '[')
            {
                navigator.ReadChar();
                navigator.SkipWhitespace();

                string index;

                if (navigator.PeekChar() == '-')
                {
                    navigator.ReadChar();
                    navigator.SkipWhitespace();
                    index = navigator.ReadInteger();
                    if (string.IsNullOrEmpty(index))
                    {
                        throw new PftSyntaxException(navigator);
                    }
                    int indexTo = int.Parse
                        (
                            index,
                            CultureInfo.InvariantCulture
                        );
                    IndexTo = indexTo;
                }
                else
                {
                    index = navigator.ReadInteger();
                    if (string.IsNullOrEmpty(index))
                    {
                        throw new PftSyntaxException(navigator);
                    }
                    int indexFrom = int.Parse
                        (
                            index,
                            CultureInfo.InvariantCulture
                        );
                    IndexFrom = indexFrom;
                    IndexTo = indexFrom;
                }

                navigator.SkipWhitespace();
                if (navigator.SkipChar('-'))
                {
                    navigator.SkipWhitespace();
                    index = navigator.ReadInteger();
                    if (string.IsNullOrEmpty(index))
                    {
                        IndexTo = 0;
                    }
                    else
                    {
                        int indexTo = int.Parse
                            (
                                index,
                                CultureInfo.InvariantCulture
                            );
                        IndexTo = indexTo;
                    }
                }

                navigator.SkipWhitespace();
                if (navigator.ReadChar() != ']')
                {
                    throw new PftSyntaxException(navigator);
                }
            }

            if (c == '*')
            {
                builder.Length = 0;
                navigator.ReadChar();

                while (true)
                {
                    c = navigator.PeekChar();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    builder.Append(c);
                    navigator.ReadChar();
                }

                if (builder.Length == 0)
                {
                    throw new PftSyntaxException(navigator);
                }
                Offset = int.Parse
                    (
                        builder.ToString(),
                        CultureInfo.InvariantCulture
                    );
            }

            if (c == '.')
            {
                builder.Length = 0;
                navigator.ReadChar();

                while (true)
                {
                    c = navigator.PeekChar();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    builder.Append(c);
                    navigator.ReadChar();
                }

                if (builder.Length == 0)
                {
                    throw new PftSyntaxException(navigator);
                }
                Length = int.Parse
                    (
                        builder.ToString(),
                        CultureInfo.InvariantCulture
                    );
            }

            if (c == '(')
            {
                builder.Length = 0;
                navigator.ReadChar();

                while (true)
                {
                    c = navigator.PeekChar();
                    if (!c.IsArabicDigit())
                    {
                        break;
                    }
                    builder.Append(c);
                    navigator.ReadChar();
                }

                if (builder.Length == 0)
                {
                    throw new PftSyntaxException(navigator);
                }
                Indent = int.Parse
                    (
                        builder.ToString(),
                        CultureInfo.InvariantCulture
                    );
            }

            if (c == '#')
            {
                navigator.ReadChar();

                string index = navigator.ReadInteger();
                if (string.IsNullOrEmpty(index))
                {
                    throw new PftSyntaxException(navigator);
                }
                int indexFrom = int.Parse(index);
                IndexFrom = indexFrom;
                IndexTo = indexFrom;
            }
        }

        #endregion

        #region PftNode members

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            OnAfterExecution(context);
        }

        #endregion
    }
}
