var list = new List<int>();

for (int i = 0; i < 100; i++)
{
    list.Add(i);
}

_ = list.AsParallel()
    .Select(x =>
    {
        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} {x}");
        return x;
    })
    .ToArray();
