public class DeadLock
{
    private object _lock1 = new object();
    private object _lock2 = new object();

    public DeadLock()
    {
        var task1 = Task.Run(() =>
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Start");
            
            lock(_lock1)
            {
                Task.Delay(2000).Wait();
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} first lock");
                lock(_lock2)
                {
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} lock 2");
                }
            } 
        });

        var task2 = Task.Run(() =>
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Start");
            
            lock(_lock2)
            {
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} second lock");
                lock(_lock1)
                {
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} lock 1");
                }
            }
        });

        Console.WriteLine("Awaiting...");
        Task.WaitAll(task1, task2);
        Console.WriteLine("No deadlock");
    }
}