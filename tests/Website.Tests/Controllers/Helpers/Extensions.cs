namespace QOAM.Website.Tests.Controllers.Helpers
{
    using System.Collections.Generic;

    public static class Extensions
    {
        public static IEnumerable<int> To(this int initialValue, int maxValue)
        {
            for (var i = initialValue; i <= maxValue; i++)
            {
                yield return i;
            }
        }
    }
}