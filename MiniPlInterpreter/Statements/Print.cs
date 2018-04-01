using MiniPlInterpreter.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Statements
{
    class Print : IStatement
    {
        public IExpression Expression { get; }


        public Print(IExpression expr)
        {
            Expression = expr;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitPrintStmt(this);
        }
    }
}
