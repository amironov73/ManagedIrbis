/* QAstTokenList.cs --
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
    public sealed class QAstTokenList
    {
        #region Properties

        /// <summary>
        /// Current token.
        /// </summary>
        public QAstToken Current
        {
            get { return _tokens[_position]; }
        }

        /// <summary>
        /// Has next token?
        /// </summary>
        public bool HasNext
        {
            get { return _position + 1 < _tokens.Length; }
        }

        /// <summary>
        /// EOF reached?
        /// </summary>
        public bool IsEof { get { return _position >= _tokens.Length; } }

        /// <summary>
        /// How many tokens?
        /// </summary>
        public int Length { get { return _tokens.Length; } }

        /// <summary>
        /// Current position index.
        /// </summary>
        public int Position { get { return _position; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public QAstTokenList
            (
                IEnumerable<QAstToken> tokens
            )
        {
            _tokens = tokens.ToArray();
            _position = 0;
        }

        #endregion

        #region Private members

        private int _position;
        private readonly QAstToken[] _tokens;

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

        #endregion
    }
}
