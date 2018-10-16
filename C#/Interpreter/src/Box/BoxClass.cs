using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Box
{
    public class BoxClass
    {
        public string name;
        public BoxClass superclass;
        private Dictionary<string, BoxFunction> methods;

        public BoxClass(string name, BoxClass superclass, Dictionary<string, BoxFunction> methods)
        {
            this.superclass = superclass;
            this.name = name;
            this.methods = methods;
        }

        public BoxFunction findMethod(BoxInstance instance, string name)
        {
            if (methods.ContainsKey(name))
            {
                return methods[name].bind(instance);
            }

            if (superclass != null)
            {
                return superclass.findMethod(instance, name);
            }

            return null;
        }

        public string toString()
        {
            return name;
        }

        public object call(Interpreter interpreter, List<object> arguments)
        {
            BoxInstance instance = new BoxInstance(this);
            BoxFunction initializer = methods["init"];

            if (initializer != null)
            {
                initializer.bind(instance).call(interpreter, arguments);
            }

            return instance;
        }

        public int arity()
        {
            BoxFunction initializer = methods["init"];
            if (initializer == null) return 0;
            return initializer.arity();
        }
    }
}
