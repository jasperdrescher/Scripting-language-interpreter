using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter
{
    public class Environment
    {
        public Environment enclosing;
	    private Dictionary<string, object> values = new Dictionary<string, object>();

        public Environment()
        {
            enclosing = null;
        }

        public Environment(Environment enclosing)
        {
            this.enclosing = enclosing;
        }

        public object Get(Token name)
        {
            if (values.ContainsKey(name.lexeme))
            {
                return values[name.lexeme];
            }

            if (enclosing != null) return enclosing.Get(name);

            Console.WriteLine("Undefined variable '" + name.lexeme + "'.");

            return null;
        }

        public void assign(Token name, object value)
        {
            if (values.ContainsKey(name.lexeme))
            {
                values[name.lexeme] = value;
                return;
            }

            if (enclosing != null)
            {
                enclosing.assign(name, value);
                return;
            }

            Console.WriteLine("Undefined variable '" + name.lexeme + "'.");
        }

        public void define(string name, object value)
        {
            values[name] = value;
        }

        public Environment ancestor(int distance)
        {
            Environment environment = this;
            for (int i = 0; i < distance; i++)
            {
                environment = environment.enclosing;
            }

            return environment;
        }

        public object getAt(int distance, string name)
        {
            return ancestor(distance).values[name];
        }

        public void assignAt(int distance, Token name, object value)
        {
            ancestor(distance).values[name.lexeme] = value;
        }
    }
}
