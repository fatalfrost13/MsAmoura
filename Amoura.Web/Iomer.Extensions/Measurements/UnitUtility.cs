using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Iomer.Extensions.Measurements
{
    public static class UnitUtility
    {
        public static string ToFeetInches(this decimal value)
        {
            var feet = Convert.ToInt32(Math.Floor(value / 12));
            var inches = Convert.ToInt32(value % 12);

            var result = new List<string>();
            if (feet > 0)
            {
                result.Add(string.Format("{0}'", feet));
            }
            if (inches > 0)
            {
                result.Add(string.Format("{0}\"", inches));
            }

            return string.Join(" ", result.ToArray());
        }

        public static readonly string FeetInchRegex = "([0-9.]*')?\\s*([0-9.]*\")?";

        public static decimal ParseFeetInches(this string value)
        {
            var measure = 0M;
            var parseReg = new Regex(FeetInchRegex);

            if (parseReg.IsMatch(value))
            {
                var match = parseReg.Match(value);
                var ix = 0;
                foreach (Group group in match.Groups)
                {
                    if (ix > 0)
                    {
                        if (group.Value.Contains('\'') && !group.Value.Contains('"'))
                        {
                            measure += decimal.Parse(group.Value.TrimEnd(" '".ToCharArray())) * 12M;
                        }
                        else if (group.Value.Contains('"') && !group.Value.Contains('\''))
                        {
                            measure += decimal.Parse(group.Value.TrimEnd(" \"".ToCharArray()));
                        }
                    }
                    ix++;
                }
            }

            if (measure == 0M)
            {
                // try plain old parse for feet
                if (decimal.TryParse(value, out measure))
                {
                    measure *= 12;
                }
            }

            return measure;
        }
    }
}
