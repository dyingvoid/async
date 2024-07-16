public static class Extensions {
    public static Func<T1, T3> Compose<T1, T2, T3>(this Func<T1, T2> f, Func<T2, T3> g) => (n) => g(f(n));
    public static void Print<T>(this IEnumerable<T> collection)
    {
        Console.WriteLine(string.Join(", ", collection));
    }
}
