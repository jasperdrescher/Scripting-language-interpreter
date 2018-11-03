using Microsoft.VisualStudio.TestTools.UnitTesting;
using Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter.Tests
{
    [TestClass()]
    public class ParserTests
    {
        [TestMethod()]
        public void parseTest()
        {
            List<Token> tokens = new List<Token>();
            tokens.Add(new Token(TokenType.PRINT, "print", null, 2));
            tokens.Add(new Token(TokenType.STRING, "\"hello world\"", null, 2));
            tokens.Add(new Token(TokenType.SEMICOLON, ";", null, 2));
            tokens.Add(new Token(TokenType.EOF, "", null, 2));

            Parser parser = new Parser(tokens);
            List<Stmt> statements = parser.parse();

            Assert.IsTrue(statements[0] is Stmt.Print);
            Assert.AreEqual(statements.Count, 1);
        }
    }
}