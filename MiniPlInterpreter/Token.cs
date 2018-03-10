using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter
{
    public class Token
    {
        public TokenType Type { get; }
        public string Lexeme { get; }
        public Object Literal { get; }
        public int Line { get; }

        public Token(TokenType type, string lexeme, Object literal, int line)
        {
            Type = type;
            Lexeme = lexeme;
            Literal = literal;
            Line = line;
        }

        public override string ToString()
        {
            return Type + " " + Lexeme + " " + Literal;
        }
    }
}
