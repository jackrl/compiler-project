using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Expressions
{
    class Variable<T> : IExpression<T>
    {
        public Token Name { get; }

        public Variable(Token name)
        {
            Name = name;
        }

        public T Accept(IVisitor<T> visitor)
        {
            return visitor.VisitVariableExpr(this);
        }
    }
}
