using MiniPlInterpreter.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Statements
{
    class Var : IStatement
    {
        public Token Name { get; }
        public Token Type { get; }
        public IExpression Initializer { get; }

        public Var(Token name, Token type, IExpression initializer)
        {
            Name = name;
            Type = type;
            Initializer = initializer;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitVarStmt(this);
        }
    }
}
