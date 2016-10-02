/* PftTokenList.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using AM;
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
        /// Require next token.
        /// </summary>
        public PftTokenList RequireNext()
        {
            if (!MoveNext())
            {
                throw new IrbisException();
            }

            return this;
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

            return Current.ToString();
        }

        #endregion
    }
}
