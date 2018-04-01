using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Expressions
{
    interface IExpression<T>
    {
        T Accept(IVisitor<T> visitor);
    }
}
