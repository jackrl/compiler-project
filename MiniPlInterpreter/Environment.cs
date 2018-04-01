using System;
using System.Collections.Generic;
using System.Text;
using static MiniPlInterpreter.Error;

namespace MiniPlInterpreter
{
    class Environment
    {
        private readonly Dictionary<string, Object> values = new Dictionary<string, Object>();

        public Error Define(Token token, Object value)
        {
            if (values.ContainsKey(token.Lexeme))
            {
                return new Error(token.Line, ErrorType.SEMANTIC, "0008", $"Variable '{token.Lexeme}' has already been defined.");
            }
            values[token.Lexeme] = value;

            return null;
        }

        public Object Get(Token name)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                return values[name.Lexeme];
            }

            return null;
        }

        public Error Assign(Token name, Object value, bool ParseInteger)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                if(values[name.Lexeme].GetType() == value.GetType())
                {
                    values[name.Lexeme] = value;
                }
                else if (ParseInteger
                    && values[name.Lexeme].GetType().Equals(typeof(int))
                    && value.GetType().Equals(typeof(string)))
                {
                    
                    if (Int32.TryParse(value.ToString(), out int parsedInt))
                    {
                        values[name.Lexeme] = parsedInt;
                    }
                    else
                    {
                        return new Error(name.Line, ErrorType.SEMANTIC, "0008", $"Input '{value.ToString()}' should be an integer to be assignable to '{name.Lexeme}'.");
                    }
                }
                else
                {
                    return new Error(name.Line, ErrorType.SEMANTIC, "0006", $"Can't assign value of type {value.GetType()} into variable '{name.Lexeme}' of type {values[name.Lexeme].GetType()}.");
                }
            }
            else
            {
                return new Error(name.Line, ErrorType.SEMANTIC, "0007", $"Undefined variable '{name.Lexeme}'.");
            }

            return null;
        }
    }
}
