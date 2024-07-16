class ComplexDeadlock
{
    List<Resource> list = new List<Resource>();

    public ComplexDeadlock()
    {
        for(int i = 0; i < 2; i++)
        {
            list.Add(new Resource(i));
        }

        var t1 = Task.Factory.StartNew(() => HandleNoDeadlock(list[0], list[1]));
        var t2 = Task.Factory.StartNew(() => HandleNoDeadlock(list[1], list[0]));

        Task.WaitAll(t1, t2);
        Console.WriteLine("No deadlock");
    }

    public void HandleNoDeadlock(Resource res1, Resource res2)
    {
        Object lock1;
        Object lock2;
        var lockComparer = new LockComparer(list);

        if(lockComparer.Compare(res1, res2) > 0)
        {
            lock1 = res2;
            lock2 = res1;
        }
        else
        {
            lock1 = res1;
            lock2 = res2;
        }

        lock(lock1)
        {
            Task.Delay(2000);
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} lock1");
            lock(lock2)
            {
                res2.Value = res1.Value;
            }
        }
    }

    public void Handle(Resource res1, Resource res2)
    {
        lock(res1)
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} lock1");
            lock(res2)
            {
                res2.Value = res1.Value;
            }
        }
    }

    public class LockComparer(List<Resource> list) : Comparer<Resource>
    {
        public override int Compare(Resource? x, Resource? y)
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);

            var ix = list.IndexOf(x);
            var iy = list.IndexOf(y);

            return Math.Sign(ix - iy);
        }
    }

    public class Resource
    {
        public int Value { get; set; }

        public Resource(int value)
        {
            Value = value;
        }
    }
}