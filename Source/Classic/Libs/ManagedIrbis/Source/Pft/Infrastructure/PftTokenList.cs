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
        private readonly PftToken[] _tokens;

        #endregion

        #region Public methods

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
        /// Get span.
        /// </summary>
        [CanBeNull]
        public PftTokenList Segment
            (
                params PftTokenKind[] stop
            )
        {
            int _savePosition = _position;

            int level = 0;
            while (!IsEof)
            {
                if (Current.Kind == PftTokenKind.LeftParenthesis)
                {
                    level++;
                }
                else if (Current.Kind == PftTokenKind.RightParenthesis)
                {
                    if (level != 0)
                    {
                        level--;
                    }
                    else
                    {
                        if (Array.IndexOf(stop, Current.Kind) >= 0)
                        {
                            List<PftToken> tokens = new List<PftToken>();

                            for (
                                    int position = _savePosition;
                                    position < _position;
                                    position++
                               )
                            {
                                tokens.Add(_tokens[position]);
                            }

                            PftTokenList result = new PftTokenList(tokens);

                            return result;
                        }
                    }
                }
                else if (Array.IndexOf(stop, Current.Kind) >= 0)
                {
                    List<PftToken> tokens = new List<PftToken>();

                    for (
                            int position = _savePosition;
                            position < _position;
                            position++
                       )
                    {
                        tokens.Add(_tokens[position]);
                    }

                    PftTokenList result = new PftTokenList(tokens);

                    return result;
                }
                MoveNext();
            }

            _position = _savePosition;

            return null;
        }

        /// <summary>
        /// Get array of tokens.
        /// </summary>
        [NotNull]
        public PftToken[] ToArray()
        {
            return _tokens;
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
