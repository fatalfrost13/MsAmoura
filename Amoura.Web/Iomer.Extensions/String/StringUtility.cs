using System.Text;
using System.Text.RegularExpressions;

namespace Iomer.Extensions.String
{
    public static class StringUtility
    {
        //private readonly static string Imageregex = @"(?<=img\s+.*src\=[\x27\x22])(?<Url>[^\x27\x22]*)(?=[\x27\x22])";
        //private readonly static string Escapedimageregex = @"(?<=img\s+.*src\=\\[\x27\x22])(?<Url>[^\x27\x22]*)(?=\\[\x27\x22])";
        //private readonly static string Hyperlinkregex = @"(?<=a\s+.*href\=[\x27\x22])(?<Url>[^\x27\x22]*)(?=[\x27\x22])";
        //private readonly static string Relativepathregex = @"^((../)+)(?<Path>.+)$";

/*
        private readonly static string Htmltagsregex = "</?(a(?:bbr|cronym|ddress|pplet|rea)?|" +
            "b(?:ase(?:font)?|do|ig|lockquote|ody|r|utton)?|c(?:aption|enter|ite|o(?:de|l(?:group)?))|" +
            "d(?:[dlt]|el|fn|i[rv])|em|f(?:ieldset|o(?:nt|rm)|rame(?:set)?)|h(?:[1-6r]|ead|tml)|" +
            "i(?:frame|mg|n(?:put|s)|sindex)?|kbd|l(?:abel|egend|i(?:nk)?)|m(?:ap|e(?:nu|ta))|no(?:frames|script)|" +
            "o(?:bject|l|p(?:tgroup|tion))|p(?:aram|re)?|q|s(?:amp|cript|elect|mall|pan|" +
            "t(?:rike|rong|yle)|u[bp])?|t(?:able|body|[dhrt]|extarea|foot|head|itle)|ul?|var)\b(?:[^>\"']|\"[^\"]*\"|'[^']*')*>";        
*/

        public static string RemoveHTML(this string s)
        {
            var regex = new Regex(@"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", RegexOptions.Singleline);
            var test = regex.Replace(s, string.Empty).Replace("\n", " ").Replace("\r", " ").Trim();
            return test;
        }
        public static string GetNumberOfWords(this string s, int wordCount)
        {
            var cleanString = s.RemoveHTML();
            var countRegex = @"((?:(\S+\s+){1," + wordCount + @"})\w+)";
            var test = Regex.Match(cleanString, countRegex).Value;
            return test;
        }

        public static bool IsValidInteger(this string s)
        {
            var regex = new Regex(@"^\d+$", RegexOptions.Compiled);
            return regex.IsMatch(s);
        }
        public static readonly string PostalCodeRegex = @"^(([A-Za-z])(\d)([A-Za-z])([-\s]*)(\d)([A-Za-z])(\d))$";
        public static bool IsValidPostalCode(this string s)
        {
            var postalReg = new Regex(PostalCodeRegex);
            return postalReg.Match(s).Success;
        }
        public static string ToPostalCode(this string s)
        {
            var postalReg = new Regex(PostalCodeRegex);
            if (!string.IsNullOrWhiteSpace(s) && postalReg.Match(s).Success)
            {
                return postalReg.Replace(s, "$2$3$4 $6$7$8").ToUpper();
            }
            return s;
        }
        public static readonly string PhoneRegex = @"^(\+?1)?[-. ]?\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})[ -\.]?([e|E]?x?t?[ -\.]?([0-9]{1,5}))?$";
        public static string ToPhoneNumber(this string s)
        {
            var regexObj = new Regex(PhoneRegex);
            if (!string.IsNullOrWhiteSpace(s) && regexObj.IsMatch(s))
            {
                return !string.IsNullOrWhiteSpace(regexObj.Match(s).Groups[6].Value) ? regexObj.Replace(s, "$2.$3.$4.x$6").Trim() : regexObj.Replace(s, "$2.$3.$4").Trim();
            }
            return s;
        }
        public static readonly string WebRegex = @"^(http[s]?://)?((([a-zA-Z0-9-]+)\.)?([a-zA-Z0-9-.]+)*\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|asia|coop|info|jobs|mobi|museum|name|travel)))$";
        public static string ToWebUrl(this string s)
        {
            var regexObj = new Regex(WebRegex);
            if (!string.IsNullOrWhiteSpace(s) && regexObj.IsMatch(s)) return regexObj.Replace(s, "$2");
            return s;
        }
        public static readonly string PositiveCurrencyRegex = @"^\$?[+]?[1-9]+[0-9]{0,2}(?:,?[0-9]{3})*(?:\.[0-9]{2})?$";
        public static readonly string CurrencyRegex = @"^\$?[+-]?[0-9]{1,3}(?:,?[0-9]{3})*(?:\.[0-9]{2})?$";
        public static bool IsValidCurrency(this string s, out decimal value, string cultureInfo = "en-CA")
        {
            return decimal.TryParse(s,
                System.Globalization.NumberStyles.Currency,
                System.Globalization.CultureInfo.GetCultureInfo(cultureInfo),
                out value);
        }
        public static readonly string EmailRegex = "(^[_a-zA-Z0-9-]+(\\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\\.[a-zA-Z0-9-]+)*\\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name))$)";
        public static bool IsValidEmail(this string email)
        {
            var emailReg = new Regex(EmailRegex);
            return emailReg.Match(email).Success;
        }

        private const string CleanStringRegex = @"[^a-zA-Z0-9_]+";

        public static string CleanString(this string s)
        {
            return Regex.Replace(s, CleanStringRegex, string.Empty);
        }
        public static bool ContainsSpecialCharacters(this string s)
        {
            var stringReg = new Regex(CleanStringRegex, RegexOptions.Compiled);
            return stringReg.Match(s).Success;
        }
        public static string RemoveSpecialCharacters(this string str)
        {
            return Regex.Replace(str, CleanStringRegex, string.Empty, RegexOptions.Compiled);
        }
        public static int AsInt32(this string value)
        {
            int parsed;
            int.TryParse(value.Replace(",", ""), out parsed);
            return parsed;
        }

		public static string RemoveHtmlEntities(this string source)
		{
			return Regex.Replace(source, "&[a-zA-Z]+?;", string.Empty, RegexOptions.Compiled);
		}

		public static string SubstringWord(this string value, int length)
		{
			StringBuilder result = new StringBuilder(length);

			if (value.Length > length)
			{
				string[] words = value.Split(' ', '\n');
				for (int ix = 0; ix < words.Length; ix++)
				{
					if ((result.Length + words[ix].Length) >= length)
					{
						break;
					}
					result.AppendFormat("{0} ", words[ix]);
				}
				result.Append("...");
			}
			else
			{
				result.Append(value);
			}
			return result.ToString();
		}
    }
}
