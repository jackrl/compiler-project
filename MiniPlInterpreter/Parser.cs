using MiniPlInterpreter.Exceptions;
using MiniPlInterpreter.Expressions;
using MiniPlInterpreter.Statements;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter
{
    class Parser
    {
        private readonly List<Token> tokens;
        private int current = 0;

        public readonly List<Error> Errors = new List<Error>();

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public List<IStatement> Parse()
        {
            var statements = new List<IStatement>();
            while (!IsAtEnd())
            {
                statements.Add(Declaration());
            }

            return statements;
        }

        private IStatement Declaration()
        {
            try
            {
                if (Match(TokenType.VAR)) return VarDeclaration();

                return Statement();
            }
            catch (ParserErrorException ex)
            {
                DiscardStatementTail();
                return null;
            }
        }

        private IStatement VarDeclaration()
        {
            var name = Consume(TokenType.IDENTIFIER, "0006", "Expected variable name.");

            Consume(TokenType.COLON, "0007", "Expected ':' after variable name.");
            Token type;
            if (Match(TokenType.STRING, TokenType.INTEGER, TokenType.BOOL))
            {
                type = Previous();
            }
            else
            {
                HandleError(Peek(), "0008", "Expected type after valriable delarantion's ':'.");
                throw new ParserErrorException();
            }


            IExpression initializer = null;
            if (Match(TokenType.ASSIGNMENT))
            {
                initializer = Expression();
            }

            Consume(TokenType.SEMICOLON, "0009", "Expected ';' after variable declaration.");
            return new Var(name, type, initializer);
        }

        private IStatement Statement()
        {
            if (Match(TokenType.PRINT)) return PrintStatement();
            if (Match(TokenType.READ)) return ReadStatement();
            if (Match(TokenType.ASSERT)) return AssertStatement();
            if (Match(TokenType.FOR)) return ForStatement();

            return ExpressionStatement();
        }

        private IStatement PrintStatement()
        {
            var value = Expression();
            Consume(TokenType.SEMICOLON, "0003", "Expected ';' after value to print.");
            return new Print(value);
        }

        private IStatement ReadStatement()
        {
            var name = Identifier();
            Consume(TokenType.SEMICOLON, "0011", "Expected ';' after varibale to read to.");
            return new Read(name);
        }
        private IStatement AssertStatement()
        {
            var token = Previous();
            IExpression assertion;
            if (Check(TokenType.LEFT_PAREN))
            {
                assertion = Expression();
            }
            else
            {
                HandleError(Peek(), "0017", "Expected '(' ath the start of the assertion expression of assert.");
                throw new ParserErrorException();
            }
            
            if(Previous().Type != TokenType.RIGHT_PAREN)
            {
                HandleError(Peek(), "0018", "Expected ')' after assertion expression of assert.");
                throw new ParserErrorException();
            }
            
            Consume(TokenType.SEMICOLON, "0019", "Expected ';' after the assertion of assert.");
            return new Assert(token, assertion);
        }

        private IStatement ForStatement()
        {
            var controlVar = Identifier();
            Consume(TokenType.IN, "0012", "Expected 'in' after the control varibale of the for loop.");
            var start = Expression();
            Consume(TokenType.RANGE, "0013", "Expected '..' after the start expression of the for loop.");
            var end = Expression();
            Consume(TokenType.DO, "0014", "Expected 'do' after the end expression of the for loop.");

            var statements = new List<IStatement>();
            while (!IsAtEnd() && !Match(TokenType.END))
            {
                try
                {
                    statements.Add(Declaration());
                }
                catch (ParserErrorException ex)
                {
                    DiscardStatementTail();
                    continue;
                }
            }
            Consume(TokenType.FOR, "0015", "Expected 'for' at the end of the loop after 'end'.");
            Consume(TokenType.SEMICOLON, "0016", "Expected ';' at the end of the loop after the ending 'for'.");

            return new For(controlVar, start, end, statements);
        }

        private IStatement ExpressionStatement()
        {
            var expr = Expression();
            Consume(TokenType.SEMICOLON, "0004", "Expected ';' after expression.");
            return new ExpressionStmt(expr);
        }

        private Token Identifier()
        {
            if (Match(TokenType.IDENTIFIER)) return Previous();
            HandleError(Peek(), "0012", "Expected identifier.");
            throw new ParserErrorException();
        }

        private IExpression Expression()
        {
            return Assignment();
        }

        private IExpression Assignment()
        {
            var expr = And();

            if (Match(TokenType.ASSIGNMENT))
            {
                var equals = Previous();
                var value = Assignment();

                if (expr.GetType().Equals(typeof(Variable)))
                {
                    Token name = ((Variable)expr).Name;
                    return new Assign(name, value);
                }

                HandleError(equals, "0010", "Invalid assignment target.");
                throw new ParserErrorException();
            }

            return expr;
        }

        private IExpression And()
        {
            var expr = Equality();

            while (Match(TokenType.AND))
            {
                var oper = Previous();
                var right = Equality();
                expr = new Logical(expr, oper, right);
            }

            return expr;
        }

        private IExpression Equality()
        {
            var expr = Addition();

            while (Match(TokenType.EQUAL, TokenType.LESS))
            {
                Token oper = Previous();
                var right = Addition();
                expr = new Binary(expr, oper, right);
            }

            return expr;
        }

        private IExpression Addition()
        {
            var expr = Multiplication();

            while (Match(TokenType.MINUS, TokenType.PLUS))
            {
                Token oper = Previous();
                var right = Multiplication();
                expr = new Binary(expr, oper, right);
            }

            return expr;
        }

        private IExpression Multiplication()
        {
            var expr = Unary();

            while (Match(TokenType.SLASH, TokenType.STAR))
            {
                Token oper = Previous();
                var right = Unary();
                expr = new Binary(expr, oper, right);
            }

            return expr;
        }

        private IExpression Unary()
        {
            if (Match(TokenType.BANG, TokenType.MINUS))
            {
                Token oper = Previous();
                var right = Unary();
                return new Unary(oper, right);
            }

            return Operand();
        }

        private IExpression Operand()
        {
            if (Match(TokenType.INTEGER, TokenType.STRING))
            {
                return new Literal(Previous().Literal);
            }

            if (Match(TokenType.IDENTIFIER))
            {
                return new Variable(Previous());
            }

            if (Match(TokenType.LEFT_PAREN))
            {
                var expr = Expression();
                Consume(TokenType.RIGHT_PAREN, "0001", "Expected ')' after expression.");
                return new Grouping(expr);
            }

            // TODO: Is this ok?
            HandleError(Peek(), "0002", "Expected expression.");
            throw new ParserErrorException();
        }

        private Boolean Match(params TokenType[] types)
        {
            foreach (var type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        private bool Check(TokenType tokenType)
        {
            if (IsAtEnd()) return false;
            return Peek().Type == tokenType;
        }

        private Token Advance()
        {
            if (!IsAtEnd()) current++;
            return Previous();
        }

        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EOF;
        }

        private Token Peek()
        {
            return tokens[current];
        }

        private Token Previous()
        {
            return tokens[current - 1];
        }

        private Token Consume(TokenType type, string errorIdentifier, string errorMessage)
        {
            if (Check(type)) return Advance();

            HandleError(Peek(), errorIdentifier, errorMessage);
            throw new ParserErrorException();
        }
        
        private void HandleError(Token token, string identifier, string message)
        {
            if (token.Type == TokenType.EOF)
            {
                Errors.Add(new Error(token.Line, Error.ErrorType.PARSER, identifier, $"Reached end of file. {message}"));
            }
            else
            {
                Errors.Add(new Error(token.Line, Error.ErrorType.PARSER, identifier, $"Token {token.Lexeme} found. {message}"));
            }
        }

        // Discard the rest of the statement if an error is found
        private void DiscardStatementTail()
        {
            Advance();

            while (!IsAtEnd())
            {
                if (Previous().Type == TokenType.SEMICOLON) return;
                switch (Peek().Type)
                {
                    case TokenType.VAR:
                    case TokenType.FOR:
                    case TokenType.READ:
                    case TokenType.PRINT:
                        return;
                }
                Advance();
            }
        }
    }
}
