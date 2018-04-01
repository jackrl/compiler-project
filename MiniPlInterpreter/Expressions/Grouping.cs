using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Expressions
{
    class Grouping<T> : IExpression<T>
    {
        public IExpression<T> Expression { get; }

        public Grouping(IExpression<T> expr)
        {
            Expression = expr;
        }

        public T Accept(IVisitor<T> visitor)
        {
            return visitor.VisitGroupingExpr(this);
        }
    }
}
