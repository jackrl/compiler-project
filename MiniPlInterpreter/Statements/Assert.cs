using MiniPlInterpreter.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Statements
{
    class Assert : IStatement
    {
        public Token Token { get; }
        public IExpression Expression { get; }

        public Assert(Token token, IExpression expr)
        {
            Token = token;
            Expression = expr;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitAssertStmt(this);
        }
    }
}
