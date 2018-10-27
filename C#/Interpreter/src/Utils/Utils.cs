using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter.Utils
{
    public static partial class Utils
    {
        public static string IndexedSubstring(this string str, int start, int end)
        {
            return str.Substring(start, end - start + 1);
        }
    }
}
