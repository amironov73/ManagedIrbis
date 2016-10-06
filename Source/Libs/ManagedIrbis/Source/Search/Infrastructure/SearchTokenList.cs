/* SearchTokenList.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// List of tokens.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SearchTokenList
    {
        #region Properties

        /// <summary>
        /// Current token.
        /// </summary>
        internal SearchToken Current
        {
            get { return _tokens[_position]; }
        }

        /// <summary>
        /// EOF reached?
        /// </summary>
        internal bool IsEof { get { return _position >= _tokens.Length; } }

        /// <summary>
        /// How many tokens?
        /// </summary>
        public int Length { get { return _tokens.Length; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        internal SearchTokenList
            (
                IEnumerable<SearchToken> tokens
            )
        {
            _tokens = tokens.ToArray();
            _position = 0;
        }

        #endregion

        #region Private members

        private int _position;
        private readonly SearchToken[] _tokens;

        #endregion

        #region Public methods

        /// <summary>
        /// Move to next token.
        /// </summary>
        internal bool MoveNext()
        {
            _position++;

            return _position < _tokens.Length;
        }

        /// <summary>
        /// Require next token.
        /// </summary>
        internal SearchTokenList RequireNext()
        {
            if (!MoveNext())
            {
                throw new SearchSyntaxException();
            }

            return this;
        }

        /// <summary>
        /// Require next token.
        /// </summary>
        internal SearchTokenList RequireNext
            (
                SearchTokenKind tokenKind
            )
        {
            if (!MoveNext())
            {
                throw new SearchSyntaxException();
            }

            if (Current.Kind != tokenKind)
            {
                throw new SearchSyntaxException();
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
