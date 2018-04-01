using MiniPlInterpreter.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Statements
{
    class For : IStatement
    {
        public Token ControlVar { get; }
        public IExpression Start { get; }
        public IExpression End { get; }
        public List<IStatement> Statements { get; set; }

        public For(Token controlVar, IExpression start, IExpression end, List<IStatement> statements)
        {
            ControlVar = controlVar;
            Start = start;
            End = end;
            Statements = statements;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitForStmt(this);
        }
    }
}
