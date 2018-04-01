using MiniPlInterpreter.Expressions;
using MiniPlInterpreter.Statements;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter
{
    interface IVisitor<T>
    {
        // Expressions
        T VisitLiteralExpr(Literal<T> expr);
        T VisitUnaryExpr(Unary<T> expr);
        T VisitBinaryExpr(Binary<T> expr);
        T VisitGroupingExpr(Grouping<T> expr);
        T VisitVariableExpr(Variable<T> expr);
        T VisitAssignExpr(Assign<T> expr);
        T VisitLogicalExpr(Logical<T> expr);

        // Statements
        void VisitVarStmt(Var<T> stmt);
        void VisitPrintStmt(Print<T> stmt);
        void VisitReadStmt(Read<T> stmt);
        void VisitForStmt(For<T> stmt);
        void VisitAssertStmt(Assert<T> stmt);
        void VisitExpressionStmt(ExpressionStmt<T> stmt);
    }
}
