namespace ProjectGym.Utilities
{
    public static class ListExtensions
    {
        public static IEnumerable<T> SelectNotNull<T>(this IEnumerable<T?> values) => (IEnumerable<T>)values.Where(x => x is not null);
    }
}
