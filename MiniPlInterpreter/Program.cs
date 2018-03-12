using System;
using System.Collections.Generic;

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
                RunFile(args[0]);
            }
            else
            {
                Console.WriteLine("Starting prompt");
                RunPrompt();
            }
        }

        private static void RunFile(String path)
        {
            try
            {
                string source = System.IO.File.ReadAllText(path);
                Run(source);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Couldn't read file: {path}");
            }
            
        }

        private static void RunPrompt()
        {
            while (true) { 
                Console.Write("> ");
                var input = Console.ReadLine();
                if (input.Equals("quit")) break;
                Run(input);
            }
        }

        private static void Run(String source)
        {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.ScanTokens();

            // For now, just print the tokens.
            Console.WriteLine("Tokens:");
            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
            var errors = scanner.Errors;
            if (errors.Count > 0)
            {
                Console.WriteLine("Errors:");
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
            }
        }
    }
}
