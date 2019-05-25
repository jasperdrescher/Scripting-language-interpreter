using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Interpreter.Utils
{
    // A few utility functions to handle Java behaviour in C#.
    public static partial class Utils
    {
        public static string IndexedSubstring(this string str, int start, int end)
        {
            return str.Substring(start, end - start);
        }
    }
}
