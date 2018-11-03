﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Interpreter.Box
{
    class ConsoleWritableOutput : WritableOutput
    {
        public void WriteLine(string output)
        {
            Console.WriteLine(output);
        }

        public void Write(string output)
        {
            Console.Write(output);
        }
    }

    public class Box
    {
        private static ConsoleWritableOutput writableOutput = new ConsoleWritableOutput();
        private static Interpreter interpreter = new Interpreter(writableOutput);
        static bool hadError = false;
        static bool hadRuntimeError = false;

        public static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: box [script]");
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
            byte[] input = File.ReadAllBytes(path);
            string content = Encoding.UTF8.GetString(input);

            Console.WriteLine("Contents of {0} are: {1}", path, content);

            run(content);

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
            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(input));

            using (var sr = new StreamReader(memoryStream))
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

        public static void error(int line, string message)
        {
            report(line, "", message);
        }

        private static void report(int line, string where, string message)
        {
            Console.WriteLine("[line " + line + "] Error" + where + ": " + message);
            hadError = true;
        }

        public static void error(Token token, string message)
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

        public static void ReportError(InterpreterError error)
        {
            Console.WriteLine(error.GetMessage() + "\n[line " + error.GetToken().line + "]");
            hadRuntimeError = true;
        }
    }
}
