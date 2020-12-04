using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmptyWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static Guid ToGuid(this string str)
        {
            return new Guid(str);
        }

        public static int ToInt(this string str)
        {
            return int.Parse(str);
        }
    }
}
