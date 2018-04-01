using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Expressions
{
    class Literal : IExpression
    {
        public Object Value { get; }

        public Literal(Object value)
        {
            Value = value;
        }

        public object Accept(IVisitor visitor)
        {
            return visitor.VisitLiteralExpr(this);
        }
    }
}
