using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Text.Tokenizer
{
    public class Token
    {
        int line;
        int column;
        string value;
        TokenKind kind;

        public Token
            (
                TokenKind kind, 
                string value, 
                int line, 
                int column
            )
        {
            this.kind = kind;
            this.value = value;
            this.line = line;
            this.column = column;
        }

        public int Column
        {
            get { return this.column; }
        }

        public TokenKind Kind
        {
            get { return this.kind; }
        }

        public int Line
        {
            get { return this.line; }
        }

        public string Value
        {
            get { return this.value; }
        }
    }
}
