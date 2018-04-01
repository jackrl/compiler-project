using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Expressions
{
    class Literal : IExpression
    {
        public object Value { get; }

        public Literal(object value)
        {
            Value = value;
        }

        public object Accept(IVisitor visitor)
        {
            return visitor.VisitLiteralExpr(this);
        }
    }
}
