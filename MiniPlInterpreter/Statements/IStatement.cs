using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Statements
{
    interface IStatement
    {
        void Accept(IVisitor visitor);
    }
}
