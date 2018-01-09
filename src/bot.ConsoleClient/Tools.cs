using System;
using System.Collections.Generic;
using System.Text;

namespace bot.ConsoleClient
{
    public static class Tools
    {
        public static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            var a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}
