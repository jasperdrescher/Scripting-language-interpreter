using System.CodeDom;
using System.Collections.Generic;

namespace CSLox
{
    internal class LoxInstance
    {
        private readonly LoxClass loxClass;
        private readonly Dictionary<string, object> fields = new Dictionary<string, object>();

        public LoxInstance(LoxClass loxClass)
        {
            this.loxClass = loxClass;
        }

        public object Get(Token name)
        {
            if (fields.ContainsKey(name.Lexeme))
            {
                return fields[name.Lexeme];
            }

            LoxFunction method = loxClass.FindMethod(this, name.Lexeme);
            if (method != null)
            {
                return method;
            }

            throw new RuntimeError(name, $"Undefined property '{name.Lexeme}'.");
        }

        public void Set(Token name, object value)
        {
            fields[name.Lexeme] = value;
        }

        public override string ToString()
        {
            return $"{loxClass.Name} instance";
        }
    }
}