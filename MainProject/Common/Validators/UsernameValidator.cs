using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Validators
{
    public class UsernameValidator
    {
        private static Regex sUserNameAllowedRegEx = new Regex(
               @"^[a-zA-Z]{1}[a-zA-Z0-9\._\-]{0,23}[^.-]$",
               RegexOptions.Compiled
           );
        private static Regex sUserNameIllegalEndingRegEx = new Regex(
                @"(\.|\-|\._|\-_)$",
                RegexOptions.Compiled
            );
        public static bool IsValidUsername(string userName)
        {
            if (string.IsNullOrEmpty(userName)
                || !sUserNameAllowedRegEx.IsMatch(userName)
                || sUserNameIllegalEndingRegEx.IsMatch(userName))
            {
                return false;
            }
            return true;
        }
    }
}
