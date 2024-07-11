using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Country.Extensions
{
    public static class StringExtension
    {
        public static bool IsValidChoice(this string str)
        {
            return string.Equals(str.ToLower(), "y") || string.Equals(str.ToLower(), "n");
        }
    }
}
