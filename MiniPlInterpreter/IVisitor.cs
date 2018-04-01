using MiniPlInterpreter.Expressions;
using MiniPlInterpreter.Statements;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter
{
    interface IVisitor
    {
        // Expressions
        object VisitLiteralExpr(Literal expr);
        object VisitUnaryExpr(Unary expr);
        object VisitBinaryExpr(Binary expr);
        object VisitGroupingExpr(Grouping expr);
        object VisitVariableExpr(Variable expr);
        object VisitAssignExpr(Assign expr);
        object VisitLogicalExpr(Logical expr);

        // Statements
        void VisitVarStmt(Var stmt);
        void VisitPrintStmt(Print stmt);
        void VisitReadStmt(Read stmt);
        void VisitForStmt(For stmt);
        void VisitAssertStmt(Assert stmt);
        void VisitExpressionStmt(ExpressionStmt stmt);
    }
}
