using System;
using System.Collections.Generic;

namespace MiniPlInterpreter
{
    public class Scanner
    {
        public List<Error> Errors { get { return errors; } }

        private readonly string source;
        private readonly List<Token> tokens = new List<Token>();
        private readonly List<Error> errors = new List<Error>();
        private int start = 0;
        private int current = 0;
        private int line = 1;

        private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>()
        {
            { "var" , TokenType.VAR },
            { "for" , TokenType.FOR },
            { "end" , TokenType.END },
            { "in" , TokenType.IN },
            { "do" , TokenType.DO },
            { "read" , TokenType.READ },
            { "print" , TokenType.PRINT },
            { "int" , TokenType.INTEGER },
            { "string" , TokenType.STRING },
            { "bool", TokenType.BOOL },
            { "assert" , TokenType.ASSERT }
        };

        public Scanner(string source)
        {
            this.source = source;
        }

        public List<Token> ScanTokens()
        {
            while (!ReachedEnd())
            {
                // We are at the beginning of the next lexeme.
                start = current;
                ScanToken();
            }

            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case ' ':
                case '\r':
                case '\t':
                    break;
                case '\n':
                    line++;
                    break;
                case '+': AddToken(TokenType.PLUS); break;
                case '-': AddToken(TokenType.MINUS); break;
                case '*': AddToken(TokenType.STAR); break;
                case '<': AddToken(TokenType.LESS); break;
                case '&': AddToken(TokenType.AND); break;
                case '!': AddToken(TokenType.BANG); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;

                case ':': AddToken(Match('=') ? TokenType.ASSIGNMENT : TokenType.COLON); break;
                case '.':
                    if (Match('.'))
                    {
                        AddToken(TokenType.RANGE);
                        break;
                    }
                    errors.Add(new Error(line, "Expected '.' after '.'"));
                    break;
                case '/':
                    if (Match('/'))
                    {
                        // Single line comment
                        while (Peek() != '\n' && !ReachedEnd()) Advance();
                    }
                    else if (Match('*'))
                    {
                        // Multiline comment
                        bool commentEnded = false;
                        while (!commentEnded) {
                            while (Peek() != '*' && !ReachedEnd())
                            {
                                Advance();
                            }
                            if (ReachedEnd())
                            {
                                errors.Add(new Error(line, $"Reached end of file without closing block comment, expected '*/'"));
                                break;
                            }

                            Advance();
                            if (Peek() == '/')
                            {
                                Advance();
                                commentEnded = true;
                            }
                        }
                        
                    }
                    else
                    {
                        AddToken(TokenType.SLASH);
                    }
                    break;

                case '"': String(); break;

                default:
                    if (Char.IsDigit(c))
                    {
                        Number();
                    }
                    else if (Char.IsLetter(c)){
                        Identifier();
                    }
                    else
                    {
                        errors.Add(new Error(line, $"Unexpexted character '{c}'"));
                    }
                    break;
            }
        }

        private char Advance()
        {
            current++;
            return source[current - 1];
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, Object literal)
        {
            string text = source.Substring(start, current - start);
            tokens.Add(new Token(type, text, literal, line));
        }

        private bool Match(char expected)
        {
            if (ReachedEnd()) return false;
            if (source[current] != expected) return false;

            current++;
            return true;
        }

        private char Peek()
        {
            if (ReachedEnd()) return '\0';
            return source[current];
        }

        // Unescape scapable characters
        private void String()
        {
            while (Peek() != '"' && Peek() != '\n' && !ReachedEnd())
            {
                Advance();
            }

            if (Peek() == '\n' || ReachedEnd())
            {
                errors.Add(new Error(line, "Unterminated string"));
                return;
            }

            Advance();

            String value = source.Substring(start + 1, current - start - 2);
            AddToken(TokenType.STRING, value);
        }

        private void Number()
        {
            while (Char.IsDigit(Peek())) Advance();

            AddToken(TokenType.INTEGER,
                int.Parse(source.Substring(start, current - start)));
        }

        private void Identifier()
        {
            while (Char.IsLetterOrDigit(Peek()) || Peek() == '_') Advance();

            String text = source.Substring(start, current - start);

            TokenType type = TokenType.IDENTIFIER; ;
            if (keywords.ContainsKey(text)) type = keywords[text];

            AddToken(type);
        }

        private bool ReachedEnd()
        {
            return current >= source.Length;
        }
    }
}