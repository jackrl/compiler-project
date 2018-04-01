using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Expressions
{
    class Unary<T> : IExpression<T>
    {
        public Token Operator { get; }
        public IExpression<T> Operand { get; }

        public Unary(Token oper, IExpression<T> operand)
        {
            Operator = oper;
            Operand = operand;
        }

        public T Accept(IVisitor<T> visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }
    }
}
