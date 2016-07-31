using System;
using System.Text.RegularExpressions;

namespace Iomer.Extensions.GUID
{
    public static class GuidUtility
    {
        public static bool IsEmpty(this Guid g)
        {
            return g.Equals(Guid.Empty);
        }
        public static Guid ToGuid(this string s)
        {
            return (s.IsValidGuid()) ? new Guid(s) : Guid.Empty;
        }
        public static bool IsValidGuid(this string expression)
        {
            var guidRegEx = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$",
                RegexOptions.Compiled);
            return guidRegEx.IsMatch(expression);
        }
    }
}
