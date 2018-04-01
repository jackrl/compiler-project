using System;
using System.Collections.Generic;
using System.Text;
using MiniPlInterpreter.Expressions;

namespace MiniPlInterpreter.Statements
{
    class ExpressionStmt : IStatement
    {
        public IExpression Expression { get; }

        public ExpressionStmt(IExpression expr)
        {
            Expression = expr;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitExpressionStmt(this);
        }
    }
}
