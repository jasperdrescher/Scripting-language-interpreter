using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Interpreter.Box
{
    class Box
    {
        private static Interpreter interpreter = new Interpreter();
        static bool hadError = false;
        static bool hadRuntimeError = false;

        public static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: blox [script]");
                return;
            }
            else if (args.Length == 1)
            {
                runFile(args[0]);
            }
            else
            {
                runPrompt();
            }
        }

        private static void runFile(string path)
        {
            byte[] bytes = File.ReadAllBytes(Path.GetFullPath(path));

            run(new string(Encoding.UTF8.GetString(bytes).ToCharArray()));

            // Indicate an error in the exit code.           
            if (hadError)
            {
                return;
            }

            if (hadRuntimeError)
            {
                return;
            }
        }

        private static void runPrompt()
        {
            string input = Console.In.ToString();

            using (var sr = new StreamReader(input))
            {
                Console.WriteLine("> ");
                run(sr.ReadLine());
                hadError = false;
            }
        }

        private static void run(string source)
        {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.scanTokens();

            Parser parser = new Parser(tokens);
            List<Stmt> statements = parser.parse();

            // Stop if there was a syntax error.                   
            if (hadError) return;

            Resolver resolver = new Resolver(interpreter);
            resolver.resolve(statements);

            // Stop if there was a resolution error.
            if (hadError) return;

            interpreter.interpret(statements);
        }

        static void error(int line, String message)
        {
            report(line, "", message);
        }

        private static void report(int line, String where, String message)
        {
            Console.WriteLine("[line " + line + "] Error" + where + ": " + message);
            hadError = true;
        }

        static void error(Token token, String message)
        {
            if (token.type == TokenType.EOF)
            {
                report(token.line, " at end", message);
            }
            else
            {
                report(token.line, " at '" + token.lexeme + "'", message);
            }
        }

        static void runtimeError(RuntimeError error)
        {
            Console.WriteLine(error.GetMessage() + "\n[line " + error.GetToken().line + "]");
            hadRuntimeError = true;
        }
    }
}
