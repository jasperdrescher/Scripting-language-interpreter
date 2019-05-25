using System.Collections.Generic;

namespace CLox
{
    internal interface ICallable
    {
        int Arity();
        object Call(Interpreter interpreter, List<object> arguments);
    }
}
