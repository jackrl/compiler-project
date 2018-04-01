using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Expressions
{
    class Logical : IExpression
    {
        public IExpression Left { get; }
        public Token Operator { get; }
        public IExpression Right { get; }

        public Logical(IExpression left, Token oper, IExpression right)
        {
            Left = left;
            Operator = oper;
            Right = right;
        }
        public object Accept(IVisitor visitor)
        {
            return visitor.VisitLogicalExpr(this);
        }
    }
}
