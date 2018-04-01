using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static MiniPlInterpreter.Error;

namespace MiniPlInterpreter
{
    class Environment
    {
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();
        private readonly Queue<string> inputBuffer = new Queue<string>();
        private readonly TextReader input;
        private readonly TextWriter output;

        public Environment(TextReader input, TextWriter output)
        {
            this.input = input;
            this.output = output;
        }

        public Error Define(Token token, object value)
        {
            if (values.ContainsKey(token.Lexeme))
            {
                return new Error(token.Line, ErrorType.RUNTIME, "0016", $"Variable '{token.Lexeme}' has already been defined.");
            }
            values[token.Lexeme] = value;

            return null;
        }

        public object Get(Token name)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                return values[name.Lexeme];
            }

            return null;
        }

        public Error Assign(Token name, object value, bool ParseInteger)
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
                        return new Error(name.Line, ErrorType.RUNTIME, "0017", $"Input '{value.ToString()}' should be an integer to be assignable to '{name.Lexeme}'.");
                    }
                }
                else
                {
                    return new Error(name.Line, ErrorType.RUNTIME, "0018", $"Can't assign value of type {value.GetType()} into variable '{name.Lexeme}' of type {values[name.Lexeme].GetType()}.");
                }
            }
            else
            {
                return new Error(name.Line, ErrorType.RUNTIME, "0019", $"Undefined variable '{name.Lexeme}'.");
            }

            return null;
        }

        public void Write(object value)
        {
            output.Write(value.ToString());
        }

        public object Read()
        {
            if (inputBuffer.Count == 0)
            {
                string[] words = input.ReadLine().Split();
                foreach (var word in words)
                {
                    inputBuffer.Enqueue(word);
                }
            }
            return inputBuffer.Dequeue();
        }
    }
}
