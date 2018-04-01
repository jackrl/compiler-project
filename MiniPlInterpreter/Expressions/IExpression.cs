using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Expressions
{
    interface IExpression
    {
        object Accept(IVisitor visitor);
    }
}
