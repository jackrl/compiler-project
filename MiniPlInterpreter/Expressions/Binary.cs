using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Expressions
{
    class Binary<T> : IExpression<T>
    {
        public IExpression<T> Left { get; }
        public Token Operator { get; }
        public IExpression<T> Right { get; }

        public Binary(IExpression<T> left, Token oper, IExpression<T> right)
        {
            Left = left;
            Operator = oper;
            Right = right;
        }

        public T Accept(IVisitor<T> visitor)
        {
            return visitor.VisitBinaryExpr(this);
        }
    }
}
