using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter
{
    public enum TokenType
    {
        // Single-character tokens
        PLUS,           // '+'
        MINUS,          // '-'
        STAR,           // '*'
        SLASH,          // '/'
        LESS,           // '<'
        AND,            // '&'
        BANG,           // '!'
        COLON,          // ':'
        SEMICOLON,      // ';'
        LEFT_PAREN,     // '('
        RIGHT_PAREN,    // ')'
        EQUAL,          // '='

        // Multiple-character tokens
        ASSIGNMENT,     // ':='
        RANGE,          // '..'

        // Literals
        IDENTIFIER,     //
        INTEGER,        // 'int'
        STRING,         // 'string'
        BOOL,           // 'bool' just a type??

        // Keywords
        VAR,            // 'var'
        FOR,            // 'for'
        END,            // 'end'
        IN,             // 'in'
        DO,             // 'do'
        READ,           // 'read'
        PRINT,          // 'print'
        ASSERT,         // 'assert'

        EOF             // End of File
    }
}
