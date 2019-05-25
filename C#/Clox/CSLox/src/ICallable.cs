using System.Collections.Generic;

namespace CSLox
{
    internal interface ICallable
    {
        int Arity();
        object Call(Interpreter interpreter, List<object> arguments);
    }
}
