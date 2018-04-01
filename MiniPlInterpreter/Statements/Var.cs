using MiniPlInterpreter.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Statements
{
    class Var<T> : IStatement<T>
    {
        public Token Name { get; }
        public Token Type { get; }
        public IExpression<T> Initializer { get; }

        public Var(Token name, Token type, IExpression<T> initializer)
        {
            Name = name;
            Type = type;
            Initializer = initializer;
        }

        public void Accept(IVisitor<T> visitor)
        {
            visitor.VisitVarStmt(this);
        }
    }
}
