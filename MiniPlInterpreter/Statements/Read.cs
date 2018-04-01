using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Statements
{
    class Read : IStatement
    {
        public Token Name { get; }

        public Read(Token name)
        {
            Name = name;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitReadStmt(this);
        }
    }
}
