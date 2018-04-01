using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPlInterpreter
{
    public class Error
    {
        public enum ErrorType {
            SCANNER,
            PARSER,
            SEMANTIC,
            ASSERT
        }

        public int Line { get; }
        public string Message { get; }
        public ErrorType Type { get; }
        public string Identifier { get; }

        public Error(int line, ErrorType type, string identifier, String message)
        {
            Line = line;
            Type = type;
            Identifier = identifier;
            Message = message;
        }

        public override string ToString()
        {
            string tagStr = "";
            switch (Type)
            {
                case ErrorType.SCANNER:
                    tagStr = "SCA-";
                    break;
                case ErrorType.PARSER:
                    tagStr = "PAR-";
                    break;
                case ErrorType.SEMANTIC:
                    tagStr = "SEM-";
                    break;
                default:
                    break;
            }
            tagStr += Identifier;
            if (Type == ErrorType.ASSERT) tagStr = "ASSERT";

            return $"[line {Line}] Error ({tagStr}): {Message}";
        }
    }
}
