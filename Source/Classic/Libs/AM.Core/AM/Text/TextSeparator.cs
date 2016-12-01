// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextSeparator.cs -- separates nested text from inner.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text
{
    /// <summary>
    /// Separates nested text from inner.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class TextSeparator
    {
        #region Constants

        /// <summary>
        /// Default closing sequence.
        /// </summary>
        public const string DefaultClose = "%>";

        /// <summary>
        /// Default opening sequence.
        /// </summary>
        public const string DefaultOpen = "<%";

        #endregion

        #region Properties

        /// <summary>
        /// Closing sequence.
        /// </summary>
        [NotNull]
        public string Close
        {
            get
            {
                return new string(_close);
            }
            set
            {
                Code.NotNullNorEmpty(value, "value");

                _close = value.ToCharArray();
            }
        }

        /// <summary>
        /// Nested text opening sequence.
        /// </summary>
        [NotNull]
        public string Open
        {
            get
            {
                return new string(_open);
            }
            set
            {
                Code.NotNullNorEmpty(value, "value");

                _open = value.ToCharArray();
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextSeparator()
            : this
            (
                DefaultOpen,
                DefaultClose
            )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextSeparator
            (
                [NotNull] string open,
                [NotNull] string close
            )
        {
            Code.NotNullNorEmpty(open, "open");
            Code.NotNullNorEmpty(close, "close");

            Open = open;
            Close = close;
        }

        #endregion

        #region Private members

        private char[] _close;

        private char[] _open;

        //=================================================

        /// <summary>
        /// Handle text chunk.
        /// </summary>
        /// <remarks>Must be overridden.</remarks>
        protected virtual void HandleChunk
            (
                bool inner,
                string text
            )
        {
            // Nothing to do here
        }

        #endregion

        #region Public methods

        //=================================================

        /// <summary>
        /// Separate text.
        /// </summary>
        public bool SeparateText
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            return SeparateText
                (
                    new StringReader(text)
                );
        }

        //=================================================

        /// <summary>
        /// Separate text.
        /// </summary>
        public bool SeparateText
            (
                [NotNull] TextReader reader
            )
        {
            Code.NotNull(reader, "reader");

            bool inner = false;
            char[] array = _open;
            StringBuilder buffer = new StringBuilder();

            while (true)
            {
                int depth = 0;

                while (true)
                {
                    int i = reader.Read();
                    if (i < 0)
                    {
                        goto DONE;
                    }

                    char c = (char)i;
                    if (c == array[depth])
                    {
                        depth++;
                        if (depth == array.Length)
                        {
                            HandleChunk
                                (
                                    inner,
                                    buffer.ToString()
                                );

                            depth = 0;
                            buffer.Length = 0;
                            inner = !inner;
                            array = inner ? _close : _open;
                        }
                    }
                    else
                    {
                        if (depth != 0)
                        {
                            buffer.Append(array, 0, depth);
                            depth = 0;
                        }
                        buffer.Append(c);
                    }
                }
            }

        DONE:

            if (buffer.Length != 0)
            {
                HandleChunk
                    (
                        inner,
                        buffer.ToString()
                    );
            }

            return inner;
        }

        //=================================================

        #endregion
    }
}
