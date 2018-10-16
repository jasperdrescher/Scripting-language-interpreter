using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Box
{
    public class BoxFunction : BoxCallable
    {
        private Stmt.Function declaration;
        private Environment closure;
        private bool isInitializer;

        public BoxFunction(Stmt.Function declaration, Environment closure, bool isInitializer)
        {
            this.isInitializer = isInitializer;
            this.closure = closure;
            this.declaration = declaration;
        }

        public BoxFunction bind(BoxInstance instance)
        {
            Environment environment = new Environment(closure);
            environment.define("this", instance);
            return new BoxFunction(declaration, environment, isInitializer);
        }

        public string toString()
        {
            return "<fn " + declaration.name.lexeme + ">";
        }

        public int arity()
        {
            return declaration.parameters.Count;
        }

        public object call(Interpreter interpreter, List<object> arguments)
        {
            Environment environment = new Environment(closure);
            for (int i = 0; i < declaration.parameters.Count; i++) {
                environment.define(declaration.parameters[i].lexeme, arguments[i]);
            }

            try
            {
                interpreter.executeBlock(declaration.body, environment);
            }
            catch (Return returnValue)
            {
                if (isInitializer) return closure.getAt(0, "this");
                return returnValue.value;
            }

            if (isInitializer) return closure.getAt(0, "this");

            return null;
        }
    }
}
