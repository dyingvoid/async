using System.Collections.Immutable;
using System.Security.Cryptography;

var dict = new Atom<ImmutableDictionary<Guid, string>>(
        ImmutableDictionary<Guid, string>.Empty
);

var list = new List<Guid>();

for(int i = 0; i < 2; i++)
{
    list.Add(Guid.NewGuid());
}

var temp = dict.Value;
foreach(var guid in list)
{
    temp.Add(guid, Thread.CurrentThread.ManagedThreadId.ToString());
}

for(int i = 0; i < 4; i++)
{
    list.Add(Guid.NewGuid());
}

var tasks = new List<Task>();
foreach(var guid in list)
{
    var tempGuid = guid;
    var t = Task.Factory.StartNew(() => Foo(tempGuid));
    tasks.Add(t);
}

Task.WaitAll(tasks.ToArray());

Task Foo(Guid guid)
{
    string str = Thread.CurrentThread.ManagedThreadId.ToString();
    var temp = dict.Value;

    if (dict.Swap(d =>
    {
        if (d.ContainsKey(guid))
            return d;

        return d.Add(guid, str);
    }) != temp)
    {
        Console.WriteLine("Changed.");
        return Task.CompletedTask;
    }
    else
    {
        Console.WriteLine("Nothing.");
        return Task.CompletedTask;
    }
}
