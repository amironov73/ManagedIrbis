// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftPrettyPrinter.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Text
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftPrettyPrinter
    {
        #region Properties

        /// <summary>
        /// Column.
        /// </summary>
        public int Column { get; private set; }

        /// <summary>
        /// Whether the printer is empty?
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _writer.GetStringBuilder().Length != 0;
            }
        }

        /// <summary>
        /// Indent width.
        /// </summary>
        public int IndentWidth { get; set; }

        /// <summary>
        /// Last character.
        /// </summary>
        public char LastCharacter { get; private set;}

        /// <summary>
        /// Nesting level.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// RightBorder.
        /// </summary>
        public int RightBorder { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftPrettyPrinter()
        {
            _writer = new StringWriter();
            RightBorder = 78;
            IndentWidth = 2;
        }

        #endregion

        #region Private members

        private readonly StringWriter _writer;

        private void _RecalculateColumn()
        {
            StringBuilder builder = _writer.GetStringBuilder();

            int pos = builder.Length - 1;
            while (pos >= 0)
            {
                char chr = builder[pos];
                if (chr == '\r' || chr == '\n')
                {
                    break;
                }
                pos--;
            }

            Column = builder.Length - pos - 1;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Decrease the <see cref="Level"/>.
        /// </summary>
        [NotNull]
        public PftPrettyPrinter DecreaseLevel()
        {
            Level--;
            if (Level < 0)
            {
                throw new IrbisException();
            }

            return this;
        }

        /// <summary>
        /// Eat the last character.
        /// </summary>
        public bool EatLastCharacter()
        {
            StringBuilder builder = _writer.GetStringBuilder();

            bool result = false;
            while (builder.Length > 0 && !result)
            {
                char chr = builder[builder.Length - 1];
                if (chr != '\n' && chr != '\r')
                {
                    LastCharacter = builder.Length > 1
                        ? builder[builder.Length - 2]
                        : '\0';
                    result = true;
                }
                builder.Length--;
            }

            return result;
        }

        /// <summary>
        /// Eat trailing new line.
        /// </summary>
        public bool EatNewLine()
        {
            StringBuilder builder = _writer.GetStringBuilder();

            bool result = false, flag = false;
            while (builder.Length > 0 && !result)
            {
                char chr = builder[builder.Length - 1];
                if (chr == '\n' || chr == '\r')
                {
                    LastCharacter = builder.Length > 1
                        ? builder[builder.Length - 2]
                        : '\0';

                    builder.Length--;
                    flag = true;
                }
                else
                {
                    result = true;
                }
            }

            if (flag)
            {
                _RecalculateColumn();
            }

            return result;
        }

        /// <summary>
        /// Eat trailing whitespace.
        /// </summary>
        public bool EatWhitespace()
        {
            StringBuilder builder = _writer.GetStringBuilder();

            bool result = false, flag = false;
            while (builder.Length > 0 && !result)
            {
                char chr = builder[builder.Length - 1];
                if (char.IsWhiteSpace(chr))
                {
                    LastCharacter = builder.Length > 1
                        ? builder[builder.Length - 2]
                        : '\0';

                    builder.Length--;
                    flag = true;
                }
                else
                {
                    result = true;
                }
            }

            if (flag)
            {
                _RecalculateColumn();
            }

            return result;
        }

        /// <summary>
        /// Increase the <see cref="Level"/>.
        /// </summary>
        [NotNull]
        public PftPrettyPrinter IncreaseLevel()
        {
            Level++;

            return this;
        }

        /// <summary>
        /// Write the text.
        /// </summary>
        [NotNull]
        public PftPrettyPrinter Write
            (
                char chr
            )
        {
            _writer.Write(chr);
            if (chr == '\r' || chr == '\n')
            {
                Column = 0;
            }
            else
            {
                LastCharacter = chr;
                Column++;
            }

            return this;
        }

        /// <summary>
        /// Write the text.
        /// </summary>
        [NotNull]
        public PftPrettyPrinter Write
            (
                [CanBeNull] string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                foreach (char c in text)
                {
                    Write(c);
                }
            }

            return this;
        }

        /// <summary>
        /// Write the object.
        /// </summary>
        [NotNull]
        public PftPrettyPrinter Write
            (
                [CanBeNull] object obj
            )
        {
            if (!ReferenceEquals(obj, null))
            {
                string text = obj.ToString();
                Write(text);
            }

            return this;
        }

        /// <summary>
        /// Write the formatted text.
        /// </summary>
        [NotNull]
        public PftPrettyPrinter Write
            (
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNull(format, "format");

            string text = string.Format(format, args);
            Write(text);

            return this;
        }

        /// <summary>
        /// Write the indent.
        /// </summary>
        [NotNull]
        public PftPrettyPrinter WriteIndent()
        {
            for (int i = 0; i < Level; i++)
            {
                for (int j = 0; j < IndentWidth; j++)
                {
                    Write(' ');
                }
            }

            return this;
        }

        /// <summary>
        /// Write indent if needed.
        /// </summary>
        [NotNull]
        public PftPrettyPrinter WriteIndendIfNeeded()
        {
            int delta = IndentWidth * Level - Column;
            while (delta > 0)
            {
                Write(' ');
                delta--;
            }

            return this;
        }

        /// <summary>
        /// Write newline.
        /// </summary>
        [NotNull]
        public PftPrettyPrinter WriteLine()
        {
            Write(Environment.NewLine);

            return this;
        }

        /// <summary>
        /// Write the text.
        /// </summary>
        [NotNull]
        public PftPrettyPrinter WriteLine
            (
                [CanBeNull] string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                Write(text);
            }
            Write(Environment.NewLine);

            return this;
        }

        /// <summary>
        /// Write the object.
        /// </summary>
        [NotNull]
        public PftPrettyPrinter WriteLine
            (
                [CanBeNull] object obj
            )
        {
            if (!ReferenceEquals(obj, null))
            {
                string text = obj.ToString();
                WriteLine(text);
            }

            return this;
        }

        /// <summary>
        /// Write the formatted text.
        /// </summary>
        [NotNull]
        public PftPrettyPrinter WriteLine
            (
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNull(format, "format");

            string text = string.Format(format, args);
            WriteLine(text);

            return this;
        }

        /// <summary>
        /// Write new line if needed.
        /// </summary>
        [NotNull]
        public PftPrettyPrinter WriteLineIfNeeded()
        {
            if (Column > RightBorder)
            {
                WriteLine();
            }

            return this;
        }

        /// <summary>
        /// Write nodes.
        /// </summary>
        [NotNull]
        public PftPrettyPrinter WriteNodes
            (
                [NotNull] IList<PftNode> nodes
            )
        {
            Code.NotNull(nodes, "nodes");

            foreach (PftNode node in nodes)
            {
                WriteIndendIfNeeded();
                node.PrettyPrint(this);
                WriteLineIfNeeded();
            }

            return this;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            if (Level != 0)
            {
                Log.Warn
                    (
                        "PftPrettyPrinter::ToString: "
                        + "Level="
                        + Level
                    );
            }

            return _writer.ToString();
        }

        #endregion
    }
}
