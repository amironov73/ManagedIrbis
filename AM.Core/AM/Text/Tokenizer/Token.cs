/* Token.cs -- токен
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text.Tokenizer
{
    /// <summary>
    /// Текстовый токен.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [DebuggerDisplay("Kind={Kind} Value='{Value}'")]
    public class Token
    {
        #region Properties

        /// <summary>
        /// Тип токена.
        /// </summary>
        public TokenKind Kind
        {
            get { return _kind; }
        }

        /// <summary>
        /// Номер колонки.
        /// </summary>
        public int Column
        {
            get { return _column; }
        }

        /// <summary>
        /// Номер строки.
        /// </summary>
        public int Line
        {
            get { return _line; }
        }

        /// <summary>
        /// Значение.
        /// </summary>
        [CanBeNull]
        public string Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Признак конца текста?
        /// </summary>
        public bool IsEOF
        {
            get { return Kind == TokenKind.EOF; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Token
            (
                TokenKind kind,
                [CanBeNull] string value,
                int line,
                int column
            )
        {
            _kind = kind;
            _value = value;
            _line = line;
            _column = column;
        }

        #endregion

        #region Private members

        private int _line;
        private int _column;
        internal string _value;
        private TokenKind _kind;

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format
                (
                    "Kind: {0}, Column: {1}, Line: {2}, Value: {3}", 
                    Kind, 
                    Column, 
                    Line, 
                    Value
                );
        }

        #endregion
    }
}
