using System;
using static MiniPlInterpreter.Error;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter.Exceptions
{
    class RuntimeErrorException : Exception
    {
        public RuntimeErrorException()
        {
        }

        public RuntimeErrorException(Error error)
            : base(error.ToString())
        {
        }
    }
}
