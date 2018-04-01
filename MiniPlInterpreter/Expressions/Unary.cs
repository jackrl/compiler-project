using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Expressions
{
    class Unary : IExpression
    {
        public Token Operator { get; }
        public IExpression Operand { get; }

        public Unary(Token oper, IExpression operand)
        {
            Operator = oper;
            Operand = operand;
        }

        public object Accept(IVisitor visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }
    }
}
