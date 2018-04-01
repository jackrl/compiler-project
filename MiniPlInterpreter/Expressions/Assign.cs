using MiniPlInterpreter.Statements;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Expressions
{
    class Assign<T> : IExpression<T>
    {
        public Token Name { get; }
        public IExpression<T> Value { get; }

        public Assign(Token name, IExpression<T> value)
        {
            Name = name;
            Value = value;
        }

        public T Accept(IVisitor<T> visitor)
        {
            return visitor.VisitAssignExpr(this);
        }
    }
}
