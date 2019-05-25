using System;

namespace CLox
{
    internal class Nothing
    {
        private Nothing()
        {
            throw new InvalidOperationException("Do not instantiate Nothing");
        }

        public static Nothing AtAll => null;
    }
}