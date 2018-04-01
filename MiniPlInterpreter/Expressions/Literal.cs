using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Expressions
{
    class Literal<T> : IExpression<T>
    {
        public Object Value { get; }

        public Literal(Object value)
        {
            Value = value;
        }

        public T Accept(IVisitor<T> visitor)
        {
            return visitor.VisitLiteralExpr(this);
        }
    }
}
