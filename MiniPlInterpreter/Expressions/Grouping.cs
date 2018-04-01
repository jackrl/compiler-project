using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Expressions
{
    class Grouping : IExpression
    {
        public IExpression Expression { get; }

        public Grouping(IExpression expr)
        {
            Expression = expr;
        }

        public object Accept(IVisitor visitor)
        {
            return visitor.VisitGroupingExpr(this);
        }
    }
}
