namespace QOAM.Core.Helpers
{
    public static class StringExtensions
    {
        public static string TrimSafe(this string str)
        {
            return str == null ? null : str.Trim();
        }
    }
}