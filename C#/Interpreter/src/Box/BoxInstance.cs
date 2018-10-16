using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Box
{
    public class BoxInstance
    {
        private BoxClass klass;
        private Dictionary<string, object> fields = new Dictionary<string, object>();

        public BoxInstance(BoxClass klass)
        {
            this.klass = klass;
        }

        public object Get(Token name)
        {
            if (fields.ContainsKey(name.lexeme))
            {
                return fields[name.lexeme];
            }

            BoxFunction method = klass.findMethod(this, name.lexeme);
            if (method != null) return method;

            Console.WriteLine("Undefined property '" + name.lexeme + "'.");

            return null;
        }

        void set(Token name, object value)
        {
            fields[name.lexeme] = value;
        }

        public string toString()
        {
            return klass.name + " instance";
        }
    }
}
