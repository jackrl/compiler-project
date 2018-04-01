using MiniPlInterpreter.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Statements
{
    class Print<T> : IStatement<T>
    {
        public IExpression<T> Expression { get; }


        public Print(IExpression<T> expr)
        {
            Expression = expr;
        }

        public void Accept(IVisitor<T> visitor)
        {
            visitor.VisitPrintStmt(this);
        }
    }
}
