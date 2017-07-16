// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftPrettyPrinter.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;

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
        /// Nesting level.
        /// </summary>
        public int Level { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftPrettyPrinter()
        {
            _writer = new StringWriter();
        }

        #endregion

        #region Private members

        private readonly StringWriter _writer;

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
                _writer.Write(text);
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
                _writer.Write(obj);
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
            if (!string.IsNullOrEmpty(text))
            {
                _writer.Write(text);
            }

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
                Write(' ');
                Write(' ');
                Write(' ');
                Write(' ');
            }

            return this;
        }

        /// <summary>
        /// Write newline.
        /// </summary>
        [NotNull]
        public PftPrettyPrinter WriteLine()
        {
            _writer.WriteLine();

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
                _writer.WriteLine(text);
            }

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
                _writer.WriteLine(obj);
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
            if (!string.IsNullOrEmpty(text))
            {
                _writer.WriteLine(text);
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
