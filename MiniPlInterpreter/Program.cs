using MiniPlInterpreter.Expressions;
using MiniPlInterpreter.Statements;
using System;
using System.Collections.Generic;

namespace MiniPlInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            string helpMessage = "MiniPlInterpreter:\n  With no arguments: Run programs on the command line by writing them on one line\n  With file path: Run the file";
            if (args.Length > 1)
            {
                Console.WriteLine(helpMessage);
            }
            else if (args.Length == 1)
            {
                if (args[0] == "--help" || args[0] == "-h" || args[0] == "help") Console.WriteLine(helpMessage);
                else
                {
                    Console.WriteLine("Running script: " + args[0]);
                    RunFile(args[0]);
                    Console.WriteLine("Press enter to close...");
                    Console.ReadLine();
                }
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
            Console.WriteLine("To exit enter: quit");
            while (true) { 
                Console.Write("> ");
                var input = Console.ReadLine();
                if (input.Equals("quit")) break;
                Run(input);
            }
        }

        private static void Run(String source)
        {
            // SCANNER
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.ScanTokens();

            // Scanner output
            var scanErrors = scanner.Errors;
            if (scanErrors.Count > 0)
            {
                Console.WriteLine("SCANNER:");
                Console.WriteLine("Errors:");
                foreach (var error in scanErrors)
                {
                    Console.WriteLine(error);
                }
            }

            // PARSER
            Parser parser = new Parser(tokens);
            var statements = parser.Parse();

            // Parser output
            var parseErrors = parser.Errors;
            if (parseErrors.Count > 0)
            {
                Console.WriteLine("PARSER:");
                Console.WriteLine("Errors:");
                foreach (var error in parseErrors)
                {
                    Console.WriteLine(error);
                }
            }

            // INTERPRETER
            if (scanErrors.Count == 0 && parseErrors.Count == 0)
            {
                //Change to be configurable
                Interpreter interpreter = new Interpreter(Console.In, Console.Out);

                //Interpreter output
                interpreter.Interpret(statements);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("INTERPRETER:");
                Console.WriteLine("The interpreter can't run as the scanner or/and parser have found errors.");
            }
        }
    }
}
