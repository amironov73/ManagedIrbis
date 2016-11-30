// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftTokenList.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// List of tokens.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftTokenList
    {
        #region Properties

        /// <summary>
        /// Current token.
        /// </summary>
        public PftToken Current
        {
            get { return _tokens[_position]; }
        }

        /// <summary>
        /// EOF reached?
        /// </summary>
        public bool IsEof { get { return _position >= _tokens.Length; } }

        /// <summary>
        /// How many tokens?
        /// </summary>
        public int Length { get { return _tokens.Length; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftTokenList
            (
                IEnumerable<PftToken> tokens
            )
        {
            _tokens = tokens.ToArray();
            _position = 0;
        }

        #endregion

        #region Private members

        private int _position;
        private PftToken[] _tokens;

        #endregion

        #region Public methods

        /// <summary>
        /// Add a token.
        /// </summary>
        public void Add
            (
                PftTokenKind kind
            )
        {
            PftToken token = new PftToken
            {
                Kind = kind
            };
            List<PftToken> tokens = new List<PftToken>(_tokens)
            {
                token
            };
            _tokens = tokens.ToArray();
        }

        /// <summary>
        /// Dump token list.
        /// </summary>
        public void Dump
            (
                [NotNull] TextWriter writer
            )
        {
            Code.NotNull(writer, "writer");


            writer.WriteLine
                (
                    "Total tokens: {0}",
                    _tokens.Length
                );
            foreach (PftToken token in _tokens)
            {
                writer.WriteLine(token);
            }
        }

        /// <summary>
        /// Move to next token.
        /// </summary>
        public bool MoveNext()
        {
            _position++;

            return _position < _tokens.Length;
        }

        /// <summary>
        /// Peek next token.
        /// </summary>
        public PftTokenKind Peek()
        {
            int newPosition = _position + 1;
            if (newPosition >= _tokens.Length)
            {
                return PftTokenKind.None;
            }

            return _tokens[newPosition].Kind;
        }

        /// <summary>
        /// Peek token at arbitrary position.
        /// </summary>
        public PftTokenKind Peek
            (
                int delta
            )
        {
            int newPosition = _position + delta;
            if (newPosition < 0
                || newPosition >= _tokens.Length)
            {
                return PftTokenKind.None;
            }

            return _tokens[newPosition].Kind;
        }

        /// <summary>
        /// Require next token.
        /// </summary>
        public PftTokenList RequireNext()
        {
            if (!MoveNext())
            {
                throw new PftSyntaxException(Current);
            }

            return this;
        }

        /// <summary>
        /// Move to begin of the list.
        /// </summary>
        public PftTokenList Reset()
        {
            _position = 0;

            return this;
        }

        /// <summary>
        /// Restore position.
        /// </summary>
        public PftTokenList RestorePosition
            (
                int position
            )
        {
            Code.Nonnegative(position, "position");

            _position = position;

            return this;
        }

        /// <summary>
        /// Require next token.
        /// </summary>
        public PftTokenList RequireNext
            (
                PftTokenKind kind
            )
        {
            RequireNext();
            if (Current.Kind != kind)
            {
                throw new PftSyntaxException(Current);
            }

            return this;
        }

        /// <summary>
        /// Save position.
        /// </summary>
        public int SavePosition()
        {
            return _position;
        }

        /// <summary>
        /// Get segment (span) of the token list.
        /// </summary>
        [CanBeNull]
        public PftTokenList Segment
            (
                [NotNull] PftTokenKind[] stop
            )
        {
            Code.NotNull(stop, "stop");

            int savePosition = _position;
            int foundPosition = -1;

            while (!IsEof)
            {
                PftTokenKind current = Current.Kind;

                if (stop.Contains(current))
                {
                    foundPosition = _position;
                    break;
                }

                MoveNext();
            }

            if (foundPosition < 0)
            {
                _position = savePosition;
                return null;
            }

            List<PftToken> tokens = new List<PftToken>();
            for (
                    int position = savePosition;
                    position < _position;
                    position++
                )
            {
                tokens.Add(_tokens[position]);
            }

            PftTokenList result = new PftTokenList(tokens);

            return result;
        }


        /// <summary>
        /// Get segment (span) of the token list.
        /// </summary>
        [CanBeNull]
        public PftTokenList Segment
            (
                [NotNull] PftTokenKind[] open,
                [NotNull] PftTokenKind[] close,
                [NotNull] PftTokenKind[] stop
            )
        {
            Code.NotNull(open, "open");
            Code.NotNull(close, "close");
            Code.NotNull(stop, "stop");

            int savePosition = _position;
            int foundPosition = -1;

            int level = 0;
            while (!IsEof)
            {
                PftTokenKind current = Current.Kind;

                if (open.Contains(current))
                {
                    level++;
                }
                else if (close.Contains(current))
                {
                    if (level == 0)
                    {
                        if (stop.Contains(current))
                        {
                            foundPosition = _position;
                            break;
                        }
                    }
                    level--;
                }
                else if (stop.Contains(current))
                {
                    if (level == 0)
                    {
                        foundPosition = _position;
                        break;
                    }
                }

                MoveNext();
            }

            if (level != 0)
            {
                throw new PftSyntaxException(this);
            }
            if (foundPosition < 0)
            {
                _position = savePosition;
                return null;
            }

            List<PftToken> tokens = new List<PftToken>();
            for (
                    int position = savePosition;
                    position < _position;
                    position++
                )
            {
                tokens.Add(_tokens[position]);
            }

            PftTokenList result = new PftTokenList(tokens);

            return result;
        }

        /// <summary>
        /// Get array of tokens.
        /// </summary>
        [NotNull]
        public PftToken[] ToArray()
        {
            return _tokens;
        }

        /// <summary>
        /// Convert token list to text.
        /// </summary>
        public string ToText()
        {
            StringBuilder result = new StringBuilder();

            foreach (PftToken token in _tokens)
            {
                result.Append(token.Text);
            }

            return result.ToString();
        }

        #endregion

        #region Object members

        /// <inheritdoc />
        public override string ToString()
        {
            if (IsEof)
            {
                return "(EOF)";
            }

            return string.Format
                (
                    "{0} of {1}: {2}",
                    _position,
                    _tokens.Length,
                    Current
                );
        }

        #endregion
    }
}
