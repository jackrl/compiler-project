using MiniPlInterpreter.Exceptions;
using MiniPlInterpreter.Expressions;
using MiniPlInterpreter.Statements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static MiniPlInterpreter.Error;

namespace MiniPlInterpreter
{
    class Interpreter : IVisitor
    {
        private Environment environment;


        public Interpreter(TextReader input, TextWriter output)
        {
            environment = new Environment(input, output);
        }

        // Returns false if there were runtime errors and true if there were no runtime errors
        public bool Interpret(List<IStatement> statements)
        {
            try
            {
                foreach (var statement in statements)
                {
                    Execute(statement);
                }
                return true;
            }
            catch (RuntimeErrorException error)
            {
                Console.WriteLine(error.Message);
                return false;
            }
        }

        public void VisitPrintStmt(Print stmt)
        {
            var value = Evaluate(stmt.Expression);
            environment.Write(value);
        }

        public void VisitReadStmt(Read stmt)
        {
            var value = environment.Read();

            var error = environment.Assign(stmt.Name, value, true);
            if (error != null)
            {
                throw new RuntimeErrorException(error);
            }
        }

        public void VisitVarStmt(Var stmt)
        {
            object value = null;
            if (stmt.Initializer != null)
            {
                value = Evaluate(stmt.Initializer);
            }
            else
            {
                switch (stmt.Type.Type)
                {
                    case TokenType.INTEGER:
                        value = 0;
                        break;
                    case TokenType.STRING:
                        value = "";
                        break;
                    case TokenType.BOOL:
                        value = false;
                        break;
                }
            }

            Error error = environment.Define(stmt.Name, value);
            if (error != null) throw new RuntimeErrorException(error);
        }

        public void VisitAssertStmt(Assert stmt)
        {
            var assertion = Evaluate(stmt.Expression);

            if (!assertion.GetType().Equals(typeof(bool)))
            {
                Error error = new Error(stmt.Token.Line, ErrorType.RUNTIME, "0001", $"The assertion expressioncan't be evaluated to boolean");
                throw new RuntimeErrorException(error);
            }

            if (!(bool)assertion)
            {
                Error error = new Error(stmt.Token.Line, ErrorType.ASSERT, "", $"Assertion failed.");
                throw new RuntimeErrorException(error);
            }
        }

        public void VisitForStmt(For stmt)
        {
            var controlVar = stmt.ControlVar;
            var controlVarFromEnv = environment.Get(controlVar);
            if (controlVarFromEnv == null)
            {
                Error error = new Error(controlVar.Line, ErrorType.RUNTIME, "0002", $"Variable '{controlVar.Lexeme}' must be defined before the for loop.");
                throw new RuntimeErrorException(error);
            }
            else if (!controlVarFromEnv.GetType().Equals(typeof(int)))
            {
                Error error = new Error(controlVar.Line, ErrorType.RUNTIME, "0003", $"Variable '{controlVar.Lexeme}' must be of type integer.");
                throw new RuntimeErrorException(error);
            }

            var start = Evaluate(stmt.Start);
            if (!start.GetType().Equals(typeof(int)))
            {
                Error error = new Error(controlVar.Line, ErrorType.RUNTIME, "0004", $"The starting expression of the for loop can't be evaluated to an integer.");
                throw new RuntimeErrorException(error);
            }

            var end = Evaluate(stmt.End);
            if (!end.GetType().Equals(typeof(int)))
            {
                Error error = new Error(controlVar.Line, ErrorType.RUNTIME, "0005", $"The ending expression of the for loop can't be evaluated to an integer.");
                throw new RuntimeErrorException(error);
            }

            environment.Assign(controlVar, (int)start, false);

            while ((int)environment.Get(controlVar) <= (int)end)
            {
                foreach (var statement in stmt.Statements)
                {
                    // Check that control variable is no reassigned in the loop
                    if(statement.GetType().Equals(typeof(ExpressionStmt)))
                    {
                        var exprStmt = (ExpressionStmt)statement;
                        if (exprStmt.Expression.GetType().Equals(typeof(Assign)))
                        {
                            var assignExpr = (Assign)exprStmt.Expression;
                            if(assignExpr.Name.Lexeme == controlVar.Lexeme)
                            {
                                Error error = new Error(controlVar.Line, ErrorType.RUNTIME, "0006", $"The control variable '{controlVar.Lexeme}' must not be reassigned inside the for loop.");
                                throw new RuntimeErrorException(error);
                            }
                        }
                    }
                    Execute(statement);
                }

                environment.Assign(controlVar, (int)environment.Get(controlVar) + 1, false);
            }
        }

        public void VisitExpressionStmt(ExpressionStmt stmt)
        {
            Evaluate(stmt.Expression);
        }

        public object VisitLiteralExpr(Literal expr)
        {
            return expr.Value;
        }

        public object VisitUnaryExpr(Unary expr)
        {
            var right = Evaluate(expr.Operand);

            // TODO: Semantic errors? wrong types
            switch (expr.Operator.Type)
            {
                case TokenType.MINUS:
                    CheckIntegerOperand(expr.Operator, right);
                    return -(int)right;
                case TokenType.BANG:
                    CheckBoolOperand(expr.Operator, right);
                    return !(bool)right;
            }

            // Unreachable.
            return null;
        }

        public object VisitBinaryExpr(Binary expr)
        {
            var left = Evaluate(expr.Left);
            var right = Evaluate(expr.Right);

            Error error;

            switch (expr.Operator.Type) {
                case TokenType.PLUS:
                    if (left.GetType().Equals(typeof(int)) && right.GetType().Equals(typeof(int)))
                        return (int)left + (int)right;
                    if (left.GetType().Equals(typeof(string)) && right.GetType().Equals(typeof(string)))
                        return (String)left + (String)right;

                    error = new Error(expr.Operator.Line, ErrorType.RUNTIME, "0007", $"The operands of the operand '{expr.Operator.Lexeme}' must be integers or strings.");
                    throw new RuntimeErrorException(error);
                case TokenType.MINUS:
                    CheckIntegerOperand(expr.Operator, left, right);
                    return (int)left - (int)right;
                case TokenType.SLASH:
                    CheckIntegerOperand(expr.Operator, left, right);
                    return (int)left / (int)right;
                case TokenType.STAR:
                    CheckIntegerOperand(expr.Operator, left, right);
                    return (int)left * (int)right;
                case TokenType.EQUAL:
                    if(left.GetType().Equals(typeof(int)) && right.GetType().Equals(typeof(int)))
                        return (int)left == (int)right;
                    if(left.GetType().Equals(typeof(string)) && right.GetType().Equals(typeof(string)))
                        return (string)left == (string)right;
                    if (left.GetType().Equals(typeof(bool)) && right.GetType().Equals(typeof(bool)))
                        return (bool)left == (bool)right;

                    error = new Error(expr.Operator.Line, ErrorType.RUNTIME, "0008", $"The operands of the operand '{expr.Operator.Lexeme}' must be of the same type to be compared.");
                    throw new RuntimeErrorException(error);
                // Don't really know what kind of comparison I'm supposed to do on strings and booleans with the less operator
                // Strings: Compared alphabetically
                // Bool: C+ Style comparison with false being 0 and true being 1
                case TokenType.LESS:
                    if (left.GetType().Equals(typeof(int)) && right.GetType().Equals(typeof(int)))
                        return (int)left < (int)right;
                    if (left.GetType().Equals(typeof(string)) && right.GetType().Equals(typeof(string)))
                    {
                        if (string.Compare((string)left, (string)right, StringComparison.Ordinal) == -1)
                            return true;
                        return false;
                    }
                    if (left.GetType().Equals(typeof(bool)) && right.GetType().Equals(typeof(bool)))
                        return !(bool)left && (bool)right;

                    error = new Error(expr.Operator.Line, ErrorType.RUNTIME, "0009", $"The operands of the operand '{expr.Operator.Lexeme}' must be of the same type to be compared.");
                    throw new RuntimeErrorException(error);
            }

            return null;
        }

        public object VisitGroupingExpr(Grouping expr)
        {
            return Evaluate(expr.Expression);
        }

        public object VisitVariableExpr(Variable expr)
        {
            var value = environment.Get(expr.Name);
            if(value == null)
            {
                Error error = new Error(expr.Name.Line, ErrorType.RUNTIME, "0010", $"Variable '{expr.Name.Lexeme}' hasn't been declared.");
                throw new RuntimeErrorException(error);
            }
            return value;
        }

        public object VisitAssignExpr(Assign expr)
        {
            object value = Evaluate(expr.Value);

            var error = environment.Assign(expr.Name, value, false);
            if (error != null)
            {
                throw new RuntimeErrorException(error);
            }
            return value;
        }

        public object VisitLogicalExpr(Logical expr)
        {
            object left = Evaluate(expr.Left);
            if(!left.GetType().Equals(typeof(bool)))
            {
                Error error = new Error(expr.Operator.Line, ErrorType.RUNTIME, "0011", $"The left expression of '{expr.Operator.Lexeme}' must be evaluable to a boolean.");
                throw new RuntimeErrorException(error);
            }

            if (!(bool)left) return left;

            object right = Evaluate(expr.Right);
            if (!right.GetType().Equals(typeof(bool)))
            {
                Error error = new Error(expr.Operator.Line, ErrorType.RUNTIME, "0012", $"The rights expression of '{expr.Operator.Lexeme}' must be evaluable to a boolean.");
                throw new RuntimeErrorException(error);
            }
            return right;
        }

        private void Execute(IStatement stmt)
        {
            stmt.Accept(this);
        }

        private object Evaluate(IExpression expr)
        {
            return expr.Accept(this);
        }

        private void CheckIntegerOperand(Token oper, object operand)
        {
            if (operand.GetType().Equals(typeof(int))) return;
            Error error = new Error(oper.Line, ErrorType.RUNTIME, "0013", $"Operand of '{oper.Lexeme}' must be an integer.");
            throw new RuntimeErrorException(error);
        }

        private void CheckIntegerOperand(Token oper, object left, object right)
        {
            if (left.GetType().Equals(typeof(int)) && right.GetType().Equals(typeof(int))) return;
            
            Error error = new Error(oper.Line, ErrorType.RUNTIME, "0014", $"Both operands of '{oper.Lexeme}' must be integers.");
            throw new RuntimeErrorException(error);
        }

        private void CheckBoolOperand(Token oper, object operand)
        {
            if (operand.GetType().Equals(typeof(bool))) return;
            Error error = new Error(oper.Line, ErrorType.RUNTIME, "0015", $"Operand of '{oper.Lexeme}' must be a bool.");
            throw new RuntimeErrorException(error);
        }
    }
}
