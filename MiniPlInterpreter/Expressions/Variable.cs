using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Expressions
{
    class Variable : IExpression
    {
        public Token Name { get; }

        public Variable(Token name)
        {
            Name = name;
        }

        public object Accept(IVisitor visitor)
        {
            return visitor.VisitVariableExpr(this);
        }
    }
}
