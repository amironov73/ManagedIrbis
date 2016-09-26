/* QTokenList.cs --
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
    public sealed class QTokenList
    {
        #region Properties

        /// <summary>
        /// Current token.
        /// </summary>
        public QToken Current
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

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public QTokenList
            (
                IEnumerable<QToken> tokens
            )
        {
            _tokens = tokens.ToArray();
            _position = 0;
        }

        #endregion

        #region Private members

        private int _position;
        private readonly QToken[] _tokens;

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
        /// Require next token.
        /// </summary>
        public QTokenList RequireNext()
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
