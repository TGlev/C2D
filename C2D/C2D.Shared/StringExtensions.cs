using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2D.Shared
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public static bool ContainsNoCase(this string source, string toCheck)
        {
            return source?.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
