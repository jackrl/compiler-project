using MiniPlInterpreter.Statements;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Expressions
{
    class Assign : IExpression
    {
        public Token Name { get; }
        public IExpression Value { get; }

        public Assign(Token name, IExpression value)
        {
            Name = name;
            Value = value;
        }

        public object Accept(IVisitor visitor)
        {
            return visitor.VisitAssignExpr(this);
        }
    }
}
