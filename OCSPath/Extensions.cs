namespace OCSPath
{
    internal static class Extensions
    {
        public static T Deque<T>(this SortedSet<T> set)
        {
            T item = set.First();
            set.Remove(item);
            return item;
        }
    }
}