using System;

namespace CLox
{
    internal class Return : Exception
    {
        public object Value { get; }

        public Return(object value)
        {
            Value = value;
        }
    }
}