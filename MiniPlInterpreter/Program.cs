using System;

namespace MiniPlInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: minipl [script]");
            }
            else if (args.Length == 1)
            {
                Console.WriteLine("Running script: " + args[0]);
                //runFile(args[0]);
            }
            else
            {
                Console.WriteLine("Starting prompt");
                //runPrompt();
            }
        }
    }
}
