namespace QOAM.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public const string IssnRegexPattern = @"^\d{4}-\d{3}(\d|X)$";

        private static readonly Regex issnRegex = new Regex(IssnRegexPattern, RegexOptions.Compiled);

        public static bool IsValidISSN(this string str)
        {
            if (str == null || !issnRegex.IsMatch(str))
            {
                return false;
            }

            var digits = str.Replace("-", "").Take(7).Select(d => int.Parse(d.ToString())).ToArray();
            var checkDigit = int.Parse(str.Substring(8).Replace("X", "10"));
            var sum = 0;

            for (var i = 0; i < 7; ++i)
            {
                sum += (8 - i) * digits[i];
            }

            var remainder = sum % 11;

            return remainder == 0 ? checkDigit == 0 : 11 - remainder == checkDigit;
        }

        public static string TrimSafe(this string str)
        {
            return str == null ? null : str.Trim();
        }

        public static string Truncate(this string str, int length, string truncation = "...")
        {
            if (str == null)
            {
                return null;
            }

            if (length <= 0)
            {
                return string.Empty;
            }

            if (str.Length <= length)
            {
                return str;
            }

            if (truncation.Length <= length)
            {
                return str.Substring(0, length - truncation.Length) + truncation;
            }

            return str.Substring(0, length);
        }

        public static HashSet<string> ToLinesSet(this string str)
        {
            return str.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Where(s => s.Trim().Length > 0).Select(s => s.Trim()).ToSet(StringComparer.InvariantCultureIgnoreCase);
        }
    }
}