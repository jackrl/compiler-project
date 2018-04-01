using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Statements
{
    interface IStatement<T>
    {
        void Accept(IVisitor<T> visitor);
    }
}
