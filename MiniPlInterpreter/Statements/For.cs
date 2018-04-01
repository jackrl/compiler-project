using MiniPlInterpreter.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Statements
{
    class For<T> : IStatement<T>
    {
        public Token ControlVar { get; }
        public IExpression<T> Start { get; }
        public IExpression<T> End { get; }
        public List<IStatement<T>> Statements { get; set; }

        public For(Token controlVar, IExpression<T> start, IExpression<T> end, List<IStatement<T>> statements)
        {
            ControlVar = controlVar;
            Start = start;
            End = end;
            Statements = statements;
        }

        public void Accept(IVisitor<T> visitor)
        {
            visitor.VisitForStmt(this);
        }
    }
}
