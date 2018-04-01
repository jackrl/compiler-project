using System;
using System.Collections.Generic;
using System.Text;
using MiniPlInterpreter.Expressions;

namespace MiniPlInterpreter.Statements
{
    class ExpressionStmt<T> : IStatement<T>
    {
        public IExpression<T> Expression { get; }

        public ExpressionStmt(IExpression<T> expr)
        {
            Expression = expr;
        }

        public void Accept(IVisitor<T> visitor)
        {
            visitor.VisitExpressionStmt(this);
        }
    }
}
