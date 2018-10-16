using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter
{
    public class Environment
    {
        public Environment enclosing;
	    private Dictionary<string, object> values = new Dictionary<string, object>();

        Environment()
        {
            enclosing = null;
        }

        Environment(Environment enclosing)
        {
            this.enclosing = enclosing;
        }

        object Get(Token name)
        {
            if (values.ContainsKey(name.lexeme))
            {
                return values[name.lexeme];
            }

            if (enclosing != null) return enclosing.Get(name);

            Console.WriteLine("Undefined variable '" + name.lexeme + "'.");

            return null;
        }

        void assign(Token name, object value)
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

        void define(string name, Object value)
        {
            values[name] = value;
        }

        Environment ancestor(int distance)
        {
            Environment environment = this;
            for (int i = 0; i < distance; i++)
            {
                environment = environment.enclosing;
            }

            return environment;
        }

        object getAt(int distance, string name)
        {
            return ancestor(distance).values[name];
        }

        void assignAt(int distance, Token name, object value)
        {
            ancestor(distance).values[name.lexeme] = value;
        }
    }
}
