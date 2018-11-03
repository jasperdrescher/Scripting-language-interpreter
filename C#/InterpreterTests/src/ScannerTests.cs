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
    public class ScannerTests
    {
        [TestMethod()]
        public void scanTokensTest()
        {
            Scanner scanner = new Scanner("print \"Hello World!\";");
            List<Token> tokens = scanner.scanTokens();

            Assert.AreEqual(tokens[0].type, TokenType.PRINT);
            Assert.AreEqual(tokens[1].type, TokenType.STRING);
            Assert.AreEqual(tokens[2].type, TokenType.SEMICOLON);
            Assert.AreEqual(tokens[3].type, TokenType.EOF);
            Assert.AreEqual(tokens.Count, 4);
        }
    }
}