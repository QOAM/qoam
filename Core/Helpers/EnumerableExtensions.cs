namespace QOAM.Core.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using Validation;

    public static class EnumerableExtensions
    {
        public static HashSet<T> ToSet<T>(this IEnumerable<T> value)
        {
            Requires.NotNull(value, "value");

            return new HashSet<T>(value);
        }

        public static HashSet<T> ToSet<T>(this IEnumerable<T> value, IEqualityComparer<T> equalityComparer)
        {
            Requires.NotNull(value, "value");
            Requires.NotNull(equalityComparer, "equalityComparer");

            return new HashSet<T>(value, equalityComparer);
        }

        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            Requires.NotNull(source, "source");
            
            while (source.Any())
            {
                yield return source.Take(chunksize);
                source = source.Skip(chunksize);
            }
        }
    }
}