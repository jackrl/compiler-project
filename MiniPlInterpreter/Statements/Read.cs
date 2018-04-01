using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Statements
{
    class Read<T> : IStatement<T>
    {
        public Token Name { get; }

        public Read(Token name)
        {
            Name = name;
        }

        public void Accept(IVisitor<T> visitor)
        {
            visitor.VisitReadStmt(this);
        }
    }
}
