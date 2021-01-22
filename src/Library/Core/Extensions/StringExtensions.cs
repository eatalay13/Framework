using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public static bool IsEmail(this string str)
        {
            Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                RegexOptions.CultureInvariant | RegexOptions.Singleline);

            return regex.IsMatch(str);
        }

        public static string ToShortName(this string str)
        {
            var splits = str.Split(" ");

            return string.Concat(splits[0][0], splits[1][0]);
        }
    }
}
