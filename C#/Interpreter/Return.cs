using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter
{
    public class Return : Exception
    {
        public object value;

        public Return(object value)
        {
            this.value = value;
        }
    }
}
