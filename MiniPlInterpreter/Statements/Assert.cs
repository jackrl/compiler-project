using MiniPlInterpreter.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Statements
{
    class Assert<T> : IStatement<T>
    {
        public Token Token { get; }
        public IExpression<T> Expression { get; }

        public Assert(Token token, IExpression<T> expr)
        {
            Token = token;
            Expression = expr;
        }

        public void Accept(IVisitor<T> visitor)
        {
            visitor.VisitAssertStmt(this);
        }
    }
}
