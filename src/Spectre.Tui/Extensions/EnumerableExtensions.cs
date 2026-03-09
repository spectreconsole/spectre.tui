namespace Spectre.Tui;

internal static class EnumerableExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        // List.Reverse clashes with IEnumerable<T>.Reverse, so this method only exists
        // so we won't have to cast List<T> to IEnumerable<T>.
        public IEnumerable<T> ReverseEnumerable()
        {
            ArgumentNullException.ThrowIfNull(source);

            return source.Reverse();
        }

        public void ForEach(Action<T> action)
        {
            ArgumentNullException.ThrowIfNull(source);

            foreach (var item in source)
            {
                action(item);
            }
        }

        public IEnumerable<(int Index, bool First, bool Last, T Item)> Enumerate()
        {
            ArgumentNullException.ThrowIfNull(source);
            return Enumerate(source.GetEnumerator());
        }

        public IEnumerable<(T First, TSecond Second, TThird Third)> ZipThree<TSecond, TThird>(IEnumerable<TSecond> second, IEnumerable<TThird> third)
        {
            return source.Zip(second, (a, b) => (a, b))
                .Zip(third, (a, b) => (a.a, a.b, b));
        }
    }

    private static IEnumerable<(int Index, bool First, bool Last, T Item)> Enumerate<T>(IEnumerator<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var first = true;
        var last = !source.MoveNext();

        for (var index = 0; !last; index++)
        {
            var current = source.Current;
            last = !source.MoveNext();
            yield return (index, first, last, current);
            first = false;
        }
    }
}