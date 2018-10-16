using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter
{
    public class Token
    {
        public TokenType type;
        public string lexeme;
        public object literal;
        public int line;

        Token(TokenType type, string lexeme, object literal, int line)
        {
            this.type = type;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
        }

        public string toString()
        {
            return type + " " + lexeme + " " + literal;
        }
    }
}
