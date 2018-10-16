using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Box
{
    public interface BoxCallable
    {
        int arity();
        object call(Interpreter interpreter, List<Object> arguments);
    }
}
