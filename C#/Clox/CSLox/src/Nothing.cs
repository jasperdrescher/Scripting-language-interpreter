using System;

namespace CSLox
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