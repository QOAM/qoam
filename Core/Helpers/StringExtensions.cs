namespace QOAM.Core.Helpers
{
    public static class StringExtensions
    {
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
    }
}