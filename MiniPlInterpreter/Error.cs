using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter
{
    public class Error
    {
        public int Line { get; set; }
        public string Message { get; set; }

        public Error(int line, String message)
        {
            Line = line;
            Message = message;
        }

        public override string ToString()
        {
            return "[line " + Line + "] Error: " + Message;
        }
    }
}
