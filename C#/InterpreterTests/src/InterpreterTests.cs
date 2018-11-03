using Microsoft.VisualStudio.TestTools.UnitTesting;
using Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter.Tests
{
    class TestOutput : WritableOutput
    {
        string haha;
        public void WriteLine(string output)
        {
            haha += output + "\n";
        }

        public void Write(string output)
        {
            haha += output;
        }

        public string GetHaha()
        {
            return haha;
        }
    }

    [TestClass()]
    public class InterpreterTests
    {
        [TestMethod()]
        public void interpretTest()
        {
            TestOutput funnycsharp = new TestOutput();
            Interpreter interpreter = new Interpreter(funnycsharp);

            List<Stmt> statements = new List<Stmt>();
            statements.Add(new Stmt.Print(new Expr.Literal("Hello World")));

            interpreter.interpret(statements);

            Assert.AreEqual(funnycsharp.GetHaha(), "Hello World\n");
        }
    }
}